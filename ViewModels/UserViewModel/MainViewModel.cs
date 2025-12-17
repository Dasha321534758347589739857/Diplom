using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Diplom.Data.DB;
using Diplom.Data;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using Diplom.ViewModels.ViewModelsData;
using System.Data;
using Diplom.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Message;
using System.Xml.Linq;
using ChartDirector;
using Diplom.MatModel;
using Microsoft.EntityFrameworkCore;
using ScottPlot.PlotStyles;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using Microsoft.Win32;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using static MaterialDesignThemes.Wpf.Theme;


namespace Diplom.ViewModels.UserViewModel;

public partial class MainViewModel : ObservableObject, IRecipient<UserMessage>
{
    private User? userInput;

    [ObservableProperty]
    private string? nameUser;


    [ObservableProperty]
    private ObservableCollection<Objectt> objectss;

    [ObservableProperty]
    private Objectt? selectedObject;

    [ObservableProperty]
    private ObservableCollection<Substance> substances;
    [ObservableProperty]
    private Substance? selectedSubstance;

    [ObservableProperty]
    private ObservableCollection<EmergencySituationAtTheFacility> emergensySituationInAnObject;

    [ObservableProperty]
    private EmergencySituationAtTheFacility? selectedES;



    [ObservableProperty]
    private ObservableCollection<SubstanceInAnObject> substanceInAnObject;

    [ObservableProperty]
    private SubstanceInAnObject? selectedSIO;

    [ObservableProperty]
    private string massa = "";

    //[ObservableProperty]
    //private string time = "";

    //[ObservableProperty]
    //private string date = "";

    [ObservableProperty]
    //[NotifyCanExecuteChangedFor(nameof(SolveCommand))]
    private string scv = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
    private string kv = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
    private string xSituation = "";

    [ObservableProperty]
    //[NotifyCanExecuteChangedFor(nameof(SolveCommand))]
    private string ySituation = "";

    [ObservableProperty]
    //[NotifyCanExecuteChangedFor(nameof(SolveCommand))]
    private string xWatcher = "";

    [ObservableProperty]
    //[NotifyCanExecuteChangedFor(nameof(SolveCommand))]
    private string yWatcher = "";

    [ObservableProperty]
    private ObservableCollection<Air> airr;

    [ObservableProperty]
    private Air? selectedAir;

    [ObservableProperty]
    private ObservableCollection<Priming> pri;

    [ObservableProperty]
    private Priming? selectedPri;

    [ObservableProperty]
    private ObservableCollection<EmergencySituation> es;

    [ObservableProperty]
    private EmergencySituation? selectedEs;

    [ObservableProperty]
    private ObservableCollection<Watcher> watchers;

    [ObservableProperty]
    private Watcher? selectedWatcher;

    [ObservableProperty]
    private string temper = "";

    [ObservableProperty]
    private string sps = "";

    [ObservableProperty]
    private string step = "";

    [ObservableProperty]
    private string distanceE = "";

    [ObservableProperty]
    private string orientation = "";

    [ObservableProperty]
    private string eardrumms = "";

    [ObservableProperty]
    private string throwWay = "";

    [ObservableProperty]
    private string resperatorsOrgan = "";

    [ObservableProperty]
    private string degreeOfDamage = "";

    [ObservableProperty]
    private VisualizationViewModel visualizationViewModel;

    [ObservableProperty]
    private string pressure = "";

    TableData[] data;

    NewForReflectionData newForReflectionDatas;

    public string fDel = "";

    List<string> riString = new List<string>();
    List<string> dpString = new List<string>();
    List<string> IString = new List<string>();
    List<string> tiString = new List<string>();



    public MainViewModel(User user)
    {

        Context ctx = new Context();
        Objectss = new ObservableCollection<Objectt>(ctx.Objectts.Include(a => a.Fabric).ToList());
        Substances = new ObservableCollection<Substance>(ctx.Substances.ToList());
        Airr = new ObservableCollection<Air>(ctx.Airs.ToList());
        Pri = new ObservableCollection<Priming>(ctx.Primings.ToList());
        Es = new ObservableCollection<EmergencySituation>(ctx.EmergencySituations.ToList());
        Watchers = new ObservableCollection<Watcher>(ctx.Watchers.ToList());


        userInput = user;
        nameUser = userInput.Name;
        if (ctx.EmergencySituationAtTheFacilities.ToList().Count != 0)
        {
            EmergensySituationInAnObject = new ObservableCollection<EmergencySituationAtTheFacility>(ctx.EmergencySituationAtTheFacilities.Include(a => a.SubstanceInAnObject).Include(s => s.EmergencySituation).Include(d => d.Air).Include(k => k.Priming).Include(u => u.Watcher).Include(j => j.User).Where(g => g.IdUser == userInput.Id).ToList());
            SubstanceInAnObject = new ObservableCollection<SubstanceInAnObject>(ctx.SubstanceInAnObjects.Include(a => a.Substance).Include(f => f.Objectt).ToList());
        }

        WeakReferenceMessenger.Default.Register<UserMessage>(this);

    }

    public void Receive(UserMessage message)
    {
        userInput = message.Value;
        NameUser = userInput.Name;
    }

    public void ESIAOSelected()
    {
        if (SelectedES is null)
            return;
        using var ctx = new Context();
        SubstanceInAnObject = new ObservableCollection<SubstanceInAnObject>(ctx.SubstanceInAnObjects.Include(a => a.Substance).Include(f => f.Objectt).Where(s => s.Id == SelectedES.IdSubstanceIAO).ToList());


    }

    private bool CanSolve() =>
        CheckNull(Kv, true);
    [RelayCommand(CanExecute = nameof(CanSolve))]


    private void Solve(Window window)
    {
        if (SelectedObject == null || SelectedAir == null || SelectedPri == null || SelectedEs == null || SelectedWatcher == null || Temper == "" || Sps == "" || Step == "" || XWatcher == "" || YWatcher == "" || XSituation == "" || YSituation == "")
        {
            MessageBox.Show("Ошибка. Не введены все значения для добавления нештатной ситуации в объекте!");


        }
        else if (SelectedSubstance == null || SelectedObject == null || Massa == "" || Scv == "" || Kv == "")
        {

            MessageBox.Show("Ошибка. Не введены все значения для добавления вещества в объекте!");
        }

        else
        {

            using Context ctx = new Context();
            Substance substance = new Substance();
            substance = ctx.Substances.FirstOrDefault(x => x.Id == SelectedSubstance.Id);
            Objectt oblt = new Objectt();
            oblt = ctx.Objectts.FirstOrDefault(d => d.Id == SelectedObject.Id);
            Air ai = new Air();
            ai = ctx.Airs.FirstOrDefault(z => z.Id == SelectedAir.Id);
            Priming pr = new Priming();
            pr = ctx.Primings.FirstOrDefault(k => k.Id == SelectedPri.Id);
            EmergencySituation ess = new EmergencySituation();
            ess = ctx.EmergencySituations.FirstOrDefault(s => s.Id == SelectedEs.Id);


            if (substance == null || oblt == null || ai == null || pr == null || ess == null)
            {
                MessageBox.Show("База данных не содержит в себе подходящих данных. Попробуйте снова!");

            }
            else
            {
                SubstanceInAnObject addSIAO = new SubstanceInAnObject() { IdSubstance = substance.Id, Substance = substance, IdObject = oblt.Id, Objectt = oblt, Mass = Massa, SCOASIA = Scv, COASIAO = Kv };
                ctx.SubstanceInAnObjects.Add(addSIAO);
                EmergencySituationAtTheFacility emergencySituationAtTheFacilityy = new EmergencySituationAtTheFacility() { IdSubstanceIAO = addSIAO.Id, IdAir = ai.Id, IdPriming = pr.Id, IdSituation = ess.Id, SubstanceInAnObject = SelectedSIO, Air = ai, Priming = pr, EmergencySituation = ess, StepMeasurement = Step };
                ctx.EmergencySituationAtTheFacilities.Add(emergencySituationAtTheFacilityy);
                ctx.SaveChanges();
            }


        }
    }
    private void Ok(Window window)
    {

        //using Context ctx = new Context();
        //Substance substance = new Substance();
        //Objectt oblt = new Objectt();
        //substance = ctx.Substances.FirstOrDefault(x => x.Id == SelectedSubstance.Id);
        //oblt = ctx.Objectts.FirstOrDefault(d => d.Id == SelectedObject.Id);
        //SubstanceInAnObject sbstiao = new SubstanceInAnObject();
        //ctx.Add(sbstiao);

        //using Context ctx = new Context();
        //    Substance substance = new Substance();
        //    substance = ctx.Substances.FirstOrDefault(x => x.Id == SelectedSubstance.Id);
        //    Objectt oblt = new Objectt();
        //    oblt = ctx.Objectts.FirstOrDefault(d => d.Id == SelectedObject.Id);
        //    Air ai = new Air();
        //    ai = ctx.Airs.FirstOrDefault(z => z.Id == SelectedAir.Id);
        //    Priming pr = new Priming();
        //    pr = ctx.Primings.FirstOrDefault(k => k.Id == SelectedPri.Id);
        //    EmergencySituation ess = new EmergencySituation();
        //    ess = ctx.EmergencySituations.FirstOrDefault(s => s.Id == SelectedEs.Id);




        //Fabric fabricc = new Fabric();
        //    fabricc = ctx.Materials.FirstOrDefault(x => x.Id == SelectedMaterial.Id);
        //    WeakReferenceMessenger.Default.Send(new NewObjecttMessage(new NewObjectt() { Name = Name, IdMaterial = fabricc.Id, Temperature = Temperature, Radius = Radius, Height = Height, DistanceFromSurface = DistanceFromSurface, WallThickness = WallThickness, Clutter = Clutter, fabric = fabricc }));
        //    //window.Close();


    }


    [RelayCommand]
    private void OpenMain(Window window)
    {
        window.Close();
        AuthWindow edit = new AuthWindow();
        edit.ShowDialog();

    }



    public bool CheckNull(string parameter, bool yes)
    {

        if (yes == true)
        {
            if (Double.TryParse(parameter, out double par))
            {
                if (par != 0)
                {
                    yes = true;
                }
                else
                {
                    yes = false;
                }
            }
            else
            {
                yes = false;
            }
        }
        else
        {
            yes = false;
        }

        return yes;
    }
    public bool CheckNullObject(object parameter, bool yes)
    {


        if (yes == true)
        {
            if (parameter != null)
            {

            }
            else
            {
                yes = false;
            }
        }
        else
        {
            yes = false;
        }


        return yes;
    }

    public bool CheckEmptyString(string parameter, bool yes)
    {
        if (yes == true)
        {
            if (Double.TryParse(parameter, out double par) || parameter != "")
            {

            }
            else
            {
                yes = false;
            }
        }
        else
        {
            yes = false;
        }


        return yes;

    }

    private void Ret(List<double> Z)
    {
        double h = Double.Parse(SelectedObject.Height);
        double w = 2 * Double.Parse(SelectedObject.Radius);
        double st = Double.Parse(Step);
        double x0 = Double.Parse(XSituation);
        double y0 = Double.Parse(YSituation);

        this.VisualizationViewModel = new VisualizationViewModel(h, w, Double.Parse(Step), x0, y0, Z);
        //window.Show();
    }


    [RelayCommand]
    private void Result(Window window)
    {

        bool yes = true;

        riString.Clear();
        dpString.Clear();
        IString.Clear();
        tiString.Clear();


        yes = CheckNull(Step, yes);
        yes = CheckNull(Kv, yes);
        yes = CheckEmptyString(Temper, yes);
        yes = CheckEmptyString(XSituation, yes);
        yes = CheckEmptyString(YSituation, yes);
        yes = CheckEmptyString(XWatcher, yes);
        yes = CheckEmptyString(YWatcher, yes);
        yes = CheckNullObject(SelectedObject, yes);
        yes = CheckNullObject(SelectedSubstance, yes);
        yes = CheckNullObject(SelectedAir, yes);
        yes = CheckNullObject(SelectedPri, yes);
        yes = CheckNullObject(SelectedEs, yes);
        yes = CheckNullObject(SelectedWatcher, yes);

        using Context ctx = new Context();
        Substance substance = new Substance();
        substance = ctx.Substances.FirstOrDefault(x => x.Id == SelectedSubstance.Id);
        Glossary gloss = new Glossary();

        gloss = ctx.Glossaries.Include(x => x.Substance).Where(x => x.IdSubstance == substance.Id).FirstOrDefault(x => x.IdSubstance == SelectedSubstance.Id);


        if (yes)
        {




            if (Double.Parse(SelectedObject.Height) < Double.Parse(XWatcher))
            {
                MessageBox.Show("Длина объекта меньше, чем заданное положение наблюдателя!");
            }
            else if (Double.Parse(SelectedObject.Radius) < Double.Parse(YWatcher))
            {
                MessageBox.Show("Радиус объекта меньше, чем заданное положение наблюдателя!");
            }
            else if (Double.Parse(SelectedObject.Height) < Double.Parse(XSituation))
            {
                MessageBox.Show("Длина объекта меньше, чем заданное положение нештатной ситуации!");
            }
            else if (Double.Parse(SelectedObject.Radius) < Double.Parse(YSituation))
            {
                MessageBox.Show("Радиус объекта меньше, чем заданное положение нештатной ситуации!");
            }
            else if (Double.Parse(SelectedObject.Radius) == 0.0 || Double.Parse(SelectedObject.Height) == 0.0)
            {
                MessageBox.Show("Параметры объекта заданы неверно.");
            }
            else
            {


                double fu = Math.Pow((Double.Parse(XSituation) - Double.Parse(XWatcher)), 2.0);//a
                double fi = Math.Pow((Double.Parse(YSituation) - Double.Parse(YWatcher)), 2.0);//b
                double fuplusfi = Math.Pow(fu + fi, (1.0 / 2.0));//c
                DistanceE = Math.Round(fuplusfi, 2).ToString();

                double bet = Double.Parse(gloss.beta);
                double Vw;
                double b;
                double c;
                double d;
                double e;
                double Tf;
                double ri = 0.0;//радиус
                int counter = 0;
                double ti = 0.0;//время
                double I = 0.0;//импульс
                double deltaP = 0.0;//избыточное давление
                double P0 = 101325.0; //Атмосферное давление
                double step = Double.Parse(Step);
                double Robj = Double.Parse(SelectedObject.Radius);//радиус (ширина) в метрах
                double H = Double.Parse(SelectedObject.Height);//высота (длина) в метрах
                double ro = Double.Parse(SelectedSubstance.Density);//плотность
                double T = Double.Parse(Temper) + 273.0;//температура окружающей среды в кельвинах

                double V = Math.PI * Math.Pow(Robj, 2.0) * H;//объём объекта
                double Ng = (((1.0 - (Double.Parse(Kv) / 100.0)) * 2.0) / 9.52) + (Double.Parse(Kv) / 100.0);//количество горючего газа (метан + кислород) в помещении
                double Vg = V * Ng;//объём горючего в помещении(метан+кислород)
                double Vm = V * (Double.Parse(Kv) / 100.0);
                double m = Vm * ro;//масса метана
                double Cg = m / Vg;//концентрация горючего в смеси(кислород+метан)
                double СstPr = 100.0 / (1.0 + (4.84 * bet));//стехиомметрическая концентрация метана в воздухе в процентах
                double mPs = V * (СstPr / 100.0) * ro;//масса полного сгорания метана
                double Сst = mPs / V;//Стехиометрическая концентрация метана в воздухе

                List<double> Z = new List<double>();

                Massa = m.ToString();
                fDel = Сst.ToString();

                double qg = Double.Parse(SelectedSubstance.SpecificHeatOfCombustion) * Math.Pow(10.0, 6.0);//удельная теплота сгорания
                double E;//эфективный энергозапас
                double Rx;//безразмерное расстояние
                double Px;//безразмерное давление
                double Ix;//безразмерный импульс

                double Co = 343.0;//скорость звука
                double a = 1.0 / 6.0;




                if (Cg > Сst)
                {
                    E = m * qg * (Сst / Cg);
                }
                else
                {
                    E = m * qg * 2;
                }

                int tableData = Int32.Parse(SelectedSubstance.HazardClass) + Int32.Parse(SelectedObject.Clutter);

                if (tableData < 4)
                {
                    Vw = 500.0;

                }
                else if (tableData == 4)
                {
                    Vw = 400.0;
                }
                else if (tableData == 5)
                {
                    Vw = 250.0;
                }
                else if (tableData == 6)
                {
                    Vw = 170.0;
                }
                else if (tableData == 7)
                {
                    Vw = 43 * Math.Pow(m, a);
                }
                else
                {
                    Vw = 26 * Math.Pow(m, a);
                }






                //ri = 0.5;
                //Rx = ri / (Math.Pow((E / P0), 0.33));
                double st = 1.0 / 3.0;
                //a = (6 * Vw) / (7 * Co);
                //b = 0.4 * a;
                //c = 0.06 / Rx;
                //d = Math.Pow(Rx, 2.0);
                //e = Math.Pow(Rx, 3.0);
                // Px = Math.Pow((Vw / Co), 2.0) * (6.0 / 7.0) * ((0.83 / Rx) - (0.14 / (Math.Pow(Rx, 2.0))));
                //Rx = ri / (Math.Pow((E / P0), st));
                //Px = Math.Exp(-0.9278 - (1.5415 * Math.Log(Rx)) + (0.1953 * (Math.Pow(Math.Log(Rx), 2.0))) - (0.0285 * (Math.Pow(Math.Log(Rx), 3.0))));
                //deltaP = Px * P0;
                //Tf = Math.Round(((((P0 + deltaP) * T) / P0) - 273.0), 0);

                Tf = Math.Round((m * 0.5), 2);


                var f = new Calculate(step, Robj, H, 3.46, Double.Parse(SelectedObject.Fabric.Density), 0.005, Double.Parse(SelectedObject.DistanceFromSurface), Double.Parse(SelectedPri.Density), Double.Parse(XSituation), Double.Parse(YSituation), Double.Parse(XWatcher), Double.Parse(YWatcher), 1.232);
                //Z = f.GetPresure();
                DistanceE = Math.Round(f.GetDistance(), 2).ToString();
                (double or, double ear, double thr, double pr) = f.GetOrientation(E, st, Vw, Double.Parse(SelectedWatcher.Mass));
                Orientation = Math.Round(or, 2).ToString();
                Eardrumms = Math.Round(ear, 2).ToString();
                ThrowWay = Math.Round(thr, 2).ToString();
                DegreeOfDamage = f.GetDamage();
                ResperatorsOrgan = f.GetResperatorsOrgan();

                Pressure = Math.Round(pr / 1000, 2).ToString();


                ri = 0;

                do
                {
                    ri += step;
                    Rx = ri / (Math.Pow((E / P0), st));

                    a = (6.0 * Vw) / (7.0 * Co);
                    b = 0.4 * a;
                    c = 0.06 / Rx;
                    d = Math.Pow(Rx, 2);
                    e = Math.Pow(Rx, 3);

                    //Ix = (Vw / Co) * (6.0 / 7.0) * (1.0 - b) * (c - (0.01 / d) - (0.0025 / e));
                    //Px = Math.Pow((Vw / Co), 2) * (6.0 / 7.0) * ((0.83 / Rx) - (0.14 / (Math.Pow(Rx, 2.0))));

                    if (Rx < 0.8 && Rx > 0.2)
                    {
                        Ix = Math.Exp(-3.3228 - (1.3689 * Math.Log(Rx)) - (0.9057 * (Math.Pow(Math.Log(Rx), 2.0))) - (0.4818 * (Math.Pow(Math.Log(Rx), 3.0))));
                        Px = Math.Exp(-0.9278 - (1.5415 * Math.Log(Rx)) + (0.1953 * (Math.Pow(Math.Log(Rx), 2.0))) - (0.0285 * (Math.Pow(Math.Log(Rx), 3.0))));
                    }
                    else if (Rx >= 0.8 && Rx < 50)
                    {
                        Ix = Math.Exp(-3.2656 - (0.9641 * Math.Log(Rx)) - (0.0180 * (Math.Pow(Math.Log(Rx), 2.0))));
                        Px = Math.Exp(-0.9278 - (1.5415 * Math.Log(Rx)) + (0.1953 * (Math.Pow(Math.Log(Rx), 2.0))) - (0.0285 * (Math.Pow(Math.Log(Rx), 3.0))));
                    }
                    else
                    {
                        Ix = 0.16;
                        Px = 18.0;
                        //Ix = (Vw / Co) * (6.0 / 7.0) * (1.0 - b) * (c - (0.01 / d) - (0.0025 / e));
                        //Px = Math.Pow((Vw / Co), 2) * (6.0 / 7.0) * ((0.83 / Rx) - (0.14 / (Math.Pow(Rx, 2.0))));
                    }



                    //if ((Rx * 2.15) <= 14.0 && (Rx * 2.15) >= 1.3)
                    //{ 

                    //}

                    deltaP = (Px * P0) / 1000; //Избыточное давление в кПа
                    ti = ri / Vw;
                    I = (Ix * (Math.Pow(P0, 2.0 / 3.0)) * (Math.Pow(E, st) / Co)) / 1000;
                    riString.Add(Math.Round(ri, 2).ToString());
                    dpString.Add(Math.Round(deltaP, 0).ToString());
                    // Z.Add(deltaP);
                    IString.Add(Math.Round(I, 0).ToString());
                    tiString.Add(Math.Round(ti, 2).ToString());
                    counter++;
                    Z.Add(deltaP);

                } while (Math.Round(deltaP, 0) > 4);



                Ret(Z);
                if (data != null)
                { data = null; }

                 data = new TableData[counter];
                for (int i = 0; i < counter; i++)
                {
                    data[i] = new TableData() { SituationColumn = SelectedEs.Name, RadiusColumn = riString[i], MaxPresureColumn = dpString[i], WavePresureColumn = IString[i], Time = tiString[i] };
                }
                var res = new ResultWindow();


                (ChartData chartDatas, ChartData chartDatas1) p = (new ChartData(), new ChartData());

                for (int i = 0; i < counter; i++)
                {
                    p.chartDatas.X.Add(Double.Parse(riString[i]));
                    p.chartDatas.Y.Add(Double.Parse(dpString[i]));
                    p.chartDatas1.X.Add(Double.Parse(riString[i]));
                    p.chartDatas1.Y.Add(Double.Parse(IString[i]));

                }


                WeakReferenceMessenger.Default.Send(new DataMessage(data));//
                WeakReferenceMessenger.Default.Send(new ChartMessage(p));//
                WeakReferenceMessenger.Default.Send(new TemperatureMessage(Tf.ToString()));

                res.ShowDialog();
            }
        }

        else
        {
            MessageBox.Show("Данные введены некорректно.");

            Step = "";
            Kv = "";
            Temper = "";
            XSituation = "";
            YSituation = "";
            XWatcher = "";
            YWatcher = "";
            SelectedObject = null;
            SelectedSubstance = null;
            SelectedAir = null;
            SelectedPri = null;
            SelectedEs = null;
            SelectedWatcher = null;

            var windoww = new MainWindow(userInput);
            windoww.Show();
            window.Close();
        }


    }
    [RelayCommand]
    private void SavePar()
    {
        double mSubstance = Math.Round(Double.Parse(Massa), 2);
        using Context ctx = new Context();
        Substance substance = new Substance();
        substance = ctx.Substances.FirstOrDefault(x => x.Id == SelectedSubstance.Id);
        Fabric fabr = new Fabric();
        fabr = ctx.Materials.FirstOrDefault(l => l.Id == SelectedObject.IdMaterial);
        Objectt oblt = new Objectt();
        oblt = ctx.Objectts.Include(x => x.Fabric).Where(x => x.IdMaterial == fabr.Id).FirstOrDefault(d => d.Id == SelectedObject.Id);
        SubstanceInAnObject SO = new SubstanceInAnObject() { IdSubstance = substance.Id, IdObject = oblt.Id, Mass = mSubstance.ToString(), SCOASIA = fDel, COASIAO = Kv, Substance = substance, Objectt = oblt };
        ctx.SubstanceInAnObjects.Add(SO);
        Air ai = new Air();
        ai = ctx.Airs.FirstOrDefault(z => z.Id == SelectedAir.Id);
        Priming pr = new Priming();
        pr = ctx.Primings.FirstOrDefault(k => k.Id == SelectedPri.Id);
        EmergencySituation ess = new EmergencySituation();
        ess = ctx.EmergencySituations.FirstOrDefault(s => s.Id == SelectedEs.Id);
        Watcher watch = new Watcher();
        watch = ctx.Watchers.FirstOrDefault(z => z.Id == SelectedWatcher.Id);
        User us = new User();
        us = ctx.Users.FirstOrDefault(d => d.Id == userInput.Id);
        EmergencySituationAtTheFacility eSF = new EmergencySituationAtTheFacility() { IdAir = ai.Id, IdPriming = pr.Id, IdSituation = ess.Id, IdWatcher = watch.Id, IdUser = us.Id, IdSubstanceIAO = SO.Id, StepMeasurement = Step, XEmergencyLocation = XSituation, YEmergencyLocation = YSituation, XWatcher = XWatcher, YWatcher = YWatcher, Priming = pr, EmergencySituation = ess, Air = ai, Watcher = watch, User = us, SubstanceInAnObject = SO };
        ctx.EmergencySituationAtTheFacilities.Add(eSF);
        ctx.SaveChanges();
        EmergensySituationInAnObject = new ObservableCollection<EmergencySituationAtTheFacility>(ctx.EmergencySituationAtTheFacilities.Include(a => a.SubstanceInAnObject).Include(s => s.EmergencySituation).Include(d => d.Air).Include(k => k.Priming).Include(u => u.Watcher).Include(j => j.User).Where(g => g.IdUser == userInput.Id).ToList());
        SubstanceInAnObject = new ObservableCollection<SubstanceInAnObject>(ctx.SubstanceInAnObjects.Include(a => a.Substance).Include(f => f.Objectt).ToList());//!!!!!!!!!!!!!!!!!!!!
       

    }

    [RelayCommand]
    private void Choose()
    {
        if (SelectedES != null && SelectedSIO != null)
        {
            Step = SelectedES.StepMeasurement;
            Temper = SelectedSIO.Objectt.Temperature;
            SelectedObject = SelectedSIO.Objectt;
            Kv = SelectedSIO.COASIAO;

            XSituation = SelectedES.XEmergencyLocation;
            YSituation = SelectedES.YEmergencyLocation;
            XWatcher = SelectedES.XWatcher;
            YWatcher = SelectedES.YWatcher;

            SelectedSubstance = SelectedSIO.Substance;
            SelectedAir = SelectedES.Air;
            SelectedPri = SelectedES.Priming;
            SelectedEs = SelectedES.EmergencySituation;
            SelectedWatcher = SelectedES.Watcher;
        }
        else
        {
            MessageBox.Show("Данные введены некорректно.");

        }
    }

    [RelayCommand]
    private void SaveRes()
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";

        if (saveFileDialog.ShowDialog() == true)
        {
            StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
            streamWriter.WriteLine("Расстояние от эпицентра");
            streamWriter.WriteLine(DistanceE);
            streamWriter.WriteLine("Вероятность потери ориентации");
            streamWriter.WriteLine(Orientation);
            streamWriter.WriteLine("Вероятность разрыва барабанных перепонок");
            streamWriter.WriteLine(Eardrumms);
            streamWriter.WriteLine("Вероятность отброса ударной волной");
            streamWriter.WriteLine(ThrowWay);
            streamWriter.WriteLine("Влияние на организм");
            streamWriter.WriteLine(ResperatorsOrgan);
            streamWriter.WriteLine("Расстояние от эпицентра");
            streamWriter.WriteLine(Pressure);


            streamWriter.Close();
        }
        MessageBox.Show("Сохранение прошло успешно", "Успешное сохранение", MessageBoxButton.OK);

    }

    protected string SaveFile()
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Exel files (*.xlsx)|*.xlsx";
        if (saveFileDialog.ShowDialog() == true)
        {
            return saveFileDialog.FileName;
        }
        return "";

    }

    [RelayCommand]
    private void HandWave()
    {
        var handWin = new HandWaveWindow();
        WeakReferenceMessenger.Default.Send(new ReflectionDataMessage(data));
         newForReflectionDatas= new NewForReflectionData() { XEpicenter=xSituation, YEpicenter=ySituation, RObject=SelectedObject.Radius.ToString()};
        WeakReferenceMessenger.Default.Send(new NewForReflectionDataMessage(newForReflectionDatas));
        handWin.ShowDialog();

    }

}
