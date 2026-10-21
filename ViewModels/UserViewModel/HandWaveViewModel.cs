
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data;
using Diplom.Data.DB;
using Diplom.MatModel;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using LiveChartsCore;
using System.Drawing;


namespace Diplom.ViewModels.UserViewModel
{
    public partial class HandWaveViewModel : ObservableObject, IRecipient<ReflectionDataMessage>, IRecipient<NewForReflectionDataMessage>, IRecipient<SelectedObjectMessage>
    {
        [ObservableProperty]
        //[NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        private string angleOfIncidence = "";

        [ObservableProperty]

        private string angleOfReflection = "";

        [ObservableProperty]

        private string xReflection = "";

        [ObservableProperty]

        private string yReflection = "";

        [ObservableProperty]

        private string xIntersection = "";

        [ObservableProperty]

        private string yIntersection = "";

        [ObservableProperty]

        private string xHandPoint = "";
        [ObservableProperty]

        private string yHandPoint = "";

        [ObservableProperty]
        private ObservableCollection<string>? methods = new ObservableCollection<string>();

        [ObservableProperty]
        private string? selectedMethod;

        [ObservableProperty]
        private ObservableCollection<TableData>? reflectionData;//данные для таблицы

        [ObservableProperty]
        private ObservableCollection<TableData>? a;

        [ObservableProperty]
        private NewForReflectionData? forReflectionData;

        [ObservableProperty]
        private NewObjectt? objecttSelect;

        [ObservableProperty]
        private ObservableCollection<TableData>? handData;//данные для таблицы

        private readonly ObservableCollection<ObservablePoint> ReflectionWavePresurePoints; //точки для линии отражённого давления

        private readonly ObservableCollection<ObservablePoint> HandWavePresurePoints;//точки для линии главного давления


        public HandWaveViewModel()
        {
            WeakReferenceMessenger.Default.Register<NewForReflectionDataMessage>(this);
            WeakReferenceMessenger.Default.Register<ReflectionDataMessage>(this);
            WeakReferenceMessenger.Default.Register<SelectedObjectMessage>(this);

            Methods.Add("Метод по Короткову");
            Methods.Add("Метод по РД 03-409-01");

            ReflectionWavePresurePoints = new ObservableCollection<ObservablePoint>();

            HandWavePresurePoints = new ObservableCollection<ObservablePoint>();


            ReflectionWavePresureSeries = new ObservableCollection<ISeries>();
            HandWavePresureSeries = new ObservableCollection<ISeries>();
        }


        public Axis[] RadiusX { get; set; } =
        {
        new()
        {
            Name = "Радиус, м",
            NameTextSize = 11
        }
        };

      
        public Axis[] PresureY { get; set; } =
        {
        new()
        {
            Name = "Избыточное давление, Па",
            NameTextSize = 11
        }
        };

        public ObservableCollection<ISeries> ReflectionWavePresureSeries { get; set; }
        public ObservableCollection<ISeries> HandWavePresureSeries { get; set; }


        public static System.Data.DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new System.Data.DataTable(typeof(T).Name);

            //Get all the properties
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                var att = prop.GetCustomAttribute(typeof(ColumnNameAttribute)) as ColumnNameAttribute;
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType &&
                            prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? Nullable.GetUnderlyingType(prop.PropertyType)
                    : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(att.Name, type);
            }

            foreach (var item in items)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    //inserting property values to data table rows
                    values[i] = properties[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            //put a breakpoint here and check data table
            return dataTable;
        }
        public void Receive(ReflectionDataMessage message)
        {

            ReflectionData = new ObservableCollection<TableData>(message.Value.Select(x => new TableData()
            {
                
                RadiusColumn = x.RadiusColumn,
                MaxPresureColumn = x.MaxPresureColumn,
                WavePresureColumn = x.WavePresureColumn,
                Time = x.Time,

              }));
        
        }

        public void Receive(NewForReflectionDataMessage message1)
        {

            ForReflectionData = new NewForReflectionData() { XEpicenter= message1.Value.XEpicenter, YEpicenter = message1.Value.YEpicenter, RObject = message1.Value.RObject, Step = message1.Value.Step, EnergyStorage = message1.Value.EnergyStorage, Speed = message1.Value.Speed, C0 = message1.Value.C0};

        }

        public void Receive(SelectedObjectMessage message1)
        {
            objecttSelect = new NewObjectt() { Name = message1.Value.Name, IdMaterial  = message1.Value.IdMaterial, Temperature = message1.Value.Temperature, Radius = message1.Value.Radius, Height = message1.Value.Height, DistanceFromSurface = message1.Value.DistanceFromSurface, WallThickness = message1.Value.WallThickness, Clutter = message1.Value.Clutter, fabric = message1.Value.fabric };
            
        }

        [RelayCommand]
        private void CalcReflection()
        {



            double x;
            double y;
            double distanceReflection = 0.0;
            double energyStorage;

           
            
            int n = 0;


           
            if (SelectedMethod == "Метод по Короткову")
            {
                x = (Double.Parse(ForReflectionData.YEpicenter) * Math.Tan(Double.Parse(AngleOfIncidence) * (Math.PI / 180.0))) + Double.Parse(ForReflectionData.XEpicenter);
                y = 0;
                XReflection = x.ToString();
                YReflection = "0";
                AngleOfReflection = AngleOfIncidence;
                distanceReflection = Math.Sqrt(Math.Pow((Double.Parse(ForReflectionData.YEpicenter) * Math.Tan(Double.Parse(AngleOfIncidence) * (Math.PI / 180.0))), 2) + Math.Pow(Double.Parse(ForReflectionData.YEpicenter), 2));

                if (Double.Parse(AngleOfReflection) == 0)
                {

                    x = Double.Parse(objecttSelect.Height) + Double.Parse(ForReflectionData.XEpicenter);
                    y = Double.Parse(ForReflectionData.YEpicenter);
                    XReflection = x.ToString();
                    YReflection =ForReflectionData.YEpicenter;
                   
                    distanceReflection = Double.Parse(objecttSelect.Height);
                }

                for (int i = 0; i < ReflectionData.Count(); i++)
                {
                    if ((Math.Round(distanceReflection, 0)).ToString() == ReflectionData[i].RadiusColumn)

                    {
                        A = new ObservableCollection<TableData>()
                    { new TableData() {
                        RadiusColumn = ReflectionData[i].RadiusColumn,
                        MaxPresureColumn = (Math.Round(((2 * (Double.Parse(ReflectionData[i].MaxPresureColumn))+((1.4+1.0)/2.0)*1.2*Math.Pow(Double.Parse(ReflectionData[i].RadiusColumn)/ Double.Parse(ReflectionData[i].Time), 2))/1000),2)).ToString(),
                        WavePresureColumn = ReflectionData[i].WavePresureColumn,
                        Time = "0",
                         } };

                        n = i;

                    }


                }
                double ri = 0.0;
                double Rx;
                double Px;
                double Ix;
                double deltaP;
                double ti;
                double I;
                double q;
                double st = 1.0 / 3.0;

                double D;

                double p0;
                double i0;
                double l;

                TableData[] Data;
                p0 = Double.Parse(A[0].MaxPresureColumn) * 1000;
                i0 = Double.Parse(ReflectionData[n].WavePresureColumn);
                l = Double.Parse(ReflectionData[n].RadiusColumn) + 1;




                do
                {
                    ri += Double.Parse(ForReflectionData.Step);
                    Rx = ri / Math.Pow(Double.Parse(ForReflectionData.EnergyStorage) / 101325, (1.0 / 3.0));

                    if (Rx < 0.8 && Rx > 0.2)
                    {
                        Ix = Math.Exp(-3.3228 - (1.3689 * Math.Log(Rx)) - (0.9057 * (Math.Pow(Math.Log(Rx), 2.0))) -
                                      (0.4818 * (Math.Pow(Math.Log(Rx), 3.0))));
                        Px = Math.Exp(-0.9278 - (1.5415 * Math.Log(Rx)) + (0.1953 * (Math.Pow(Math.Log(Rx), 2.0))) -
                                      (0.0285 * (Math.Pow(Math.Log(Rx), 3.0))));
                    }
                    else if (Rx >= 0.8 && Rx < 50)
                    {
                        Ix = Math.Exp(-3.2656 - (0.9641 * Math.Log(Rx)) - (0.0180 * (Math.Pow(Math.Log(Rx), 2.0))));
                        Px = Math.Exp(-0.9278 - (1.5415 * Math.Log(Rx)) + (0.1953 * (Math.Pow(Math.Log(Rx), 2.0))) -
                                      (0.0285 * (Math.Pow(Math.Log(Rx), 3.0))));
                    }
                    else
                    {
                        Ix = 0.16;
                        Px = 18.0;
                        //Ix = (Vw / Co) * (6.0 / 7.0) * (1.0 - b) * (c - (0.01 / d) - (0.0025 / e));
                        //Px = Math.Pow((Vw / Co), 2) * (6.0 / 7.0) * ((0.83 / Rx) - (0.14 / (Math.Pow(Rx, 2.0))));
                    }

                    deltaP = ((p0 * l) / (l + Double.Parse(ForReflectionData.Step))); //Избыточное давление в кПа
                    ti = ri / Double.Parse(ForReflectionData.Speed);
                    I = (Ix * (Math.Pow(p0, 2.0 / 3.0)) * (Math.Pow(Double.Parse(ForReflectionData.EnergyStorage), st) / Double.Parse(ForReflectionData.C0))) / 1000;
                    //I = i0 * Math.Cos(Double.Parse(AngleOfIncidence));





                    A.Add(new TableData()
                    {
                        RadiusColumn = ri.ToString(),
                        MaxPresureColumn = (Math.Round(deltaP / 1000,2)).ToString(),
                        WavePresureColumn = Math.Round(I,2).ToString(),
                        Time = Math.Round(ti,2).ToString(),
                    });

                   

                    p0 = deltaP;
                    i0 = I;
                    l += Double.Parse(ForReflectionData.Step);

                } while (ri < Double.Parse(ReflectionData[n].RadiusColumn));
                //} while (Math.Round(deltaP, 0) > 4);

                double I0H = 0;
                if (Double.Parse(angleOfIncidence) > 0 && Double.Parse(angleOfIncidence) < 45)
                {
                    I0H = Double.Parse(ReflectionData[0].WavePresureColumn) * 1000 * (1 + Math.Cos(Double.Parse(angleOfReflection)));
                }
                else if (Double.Parse(angleOfIncidence) > 45 && Double.Parse(angleOfIncidence) < 90)
                {
                    I0H = Double.Parse(ReflectionData[0].WavePresureColumn) * 1000 * (1 + Math.Pow(Math.Cos(Double.Parse(angleOfReflection)), 2));

                }

                HandData = new ObservableCollection<TableData>(){
                new TableData()
                 {
                RadiusColumn = (l+ Double.Parse(ReflectionData[n].RadiusColumn)).ToString(),
                MaxPresureColumn = (Math.Round(Double.Parse(ReflectionData[0].MaxPresureColumn)*(1+Math.Cos(Double.Parse(AngleOfIncidence))),2)).ToString() ,
                WavePresureColumn = (Math.Round(I0H/1000,2)).ToString(),
                Time = Math.Round(ti,2).ToString(),
                    }


                };

                p0 = Double.Parse(HandData[0].MaxPresureColumn)*1000;
                i0 = Double.Parse(HandData[0].WavePresureColumn);
                l = Double.Parse(HandData[0].RadiusColumn) + 1;

                Rx = ri / (Math.Pow((Double.Parse(ForReflectionData.EnergyStorage) / 101325), st));



                ri = 0;



                do
                {
                    ri += Double.Parse(ForReflectionData.Step);
                    Rx = ri / Math.Pow(Double.Parse(ForReflectionData.EnergyStorage) / 101325, (1.0 / 3.0));
                    deltaP = ((p0 * l) / (l + Double.Parse(ForReflectionData.Step))); //Избыточное давление в кПа
                    ti = ri / Double.Parse(ForReflectionData.Speed);

                    if (Rx < 0.8 && Rx > 0.2)
                    {
                        Ix = Math.Exp(-3.3228 - (1.3689 * Math.Log(Rx)) - (0.9057 * (Math.Pow(Math.Log(Rx), 2.0))) -
                                      (0.4818 * (Math.Pow(Math.Log(Rx), 3.0))));
                        Px = Math.Exp(-0.9278 - (1.5415 * Math.Log(Rx)) + (0.1953 * (Math.Pow(Math.Log(Rx), 2.0))) -
                                      (0.0285 * (Math.Pow(Math.Log(Rx), 3.0))));
                    }
                    else if (Rx >= 0.8 && Rx < 50)
                    {
                        Ix = Math.Exp(-3.2656 - (0.9641 * Math.Log(Rx)) - (0.0180 * (Math.Pow(Math.Log(Rx), 2.0))));
                        Px = Math.Exp(-0.9278 - (1.5415 * Math.Log(Rx)) + (0.1953 * (Math.Pow(Math.Log(Rx), 2.0))) -
                                      (0.0285 * (Math.Pow(Math.Log(Rx), 3.0))));
                    }
                    else
                    {
                        Ix = 0.16;
                        Px = 18.0;
                        //Ix = (Vw / Co) * (6.0 / 7.0) * (1.0 - b) * (c - (0.01 / d) - (0.0025 / e));
                        //Px = Math.Pow((Vw / Co), 2) * (6.0 / 7.0) * ((0.83 / Rx) - (0.14 / (Math.Pow(Rx, 2.0))));
                    }

                    I = (Ix * (Math.Pow(p0 * 1000, 2.0 / 3.0)) * (Math.Pow(Double.Parse(ForReflectionData.EnergyStorage), st) / Double.Parse(ForReflectionData.C0))) / 1000;


                    HandData.Add(new TableData()
                    {
                        RadiusColumn = ri.ToString(),
                        MaxPresureColumn = (Math.Round((deltaP/1000),2)).ToString(),
                        WavePresureColumn = Math.Round(I,2).ToString(),
                        Time = Math.Round(ti,2).ToString(),
                    });

                    p0 = deltaP;
                    //i0 = I;
                    l += Double.Parse(ForReflectionData.Step);

                } while (Math.Round(p0, 0) > (4*1000));
                //} while (Math.Round(deltaP, 0) > 4);

                double xHP;
                double yHP;

                ReflectionCalculation? f = new ReflectionCalculation();

                (xHP, yHP) = f.GetHandWaveEpicenter(ForReflectionData.XEpicenter, ForReflectionData.YEpicenter, AngleOfIncidence);

                XHandPoint = xHP.ToString();
                YHandPoint = yHP.ToString();

                DrowData();

            }
            else if (SelectedMethod == "Метод по РД 03-409-01")
            {
                if (HandData != null)
                {   HandData.Clear();
                    XHandPoint = null;
                    YHandPoint = null;

                }

                x = Double.Parse(objecttSelect.Height) + Double.Parse(ForReflectionData.XEpicenter);
                y = Double.Parse(ForReflectionData.YEpicenter);
                XReflection = x.ToString();
                YReflection = "0";
                AngleOfIncidence = "0";
                AngleOfReflection = AngleOfIncidence;
                distanceReflection = Double.Parse(objecttSelect.Height);






                for (int i = 0; i < ReflectionData.Count(); i++)
                {
                    if ((Math.Round(distanceReflection, 0)).ToString() == ReflectionData[i].RadiusColumn)

                    {
                        A = new ObservableCollection<TableData>()
                    { new TableData() {
                        RadiusColumn = ReflectionData[i].RadiusColumn,
                        MaxPresureColumn = ReflectionData[i].MaxPresureColumn,
                        WavePresureColumn = ReflectionData[i].WavePresureColumn,
                        Time = "0",
                         } };

                        n = i;

                    }


                }
                double ri = 0.0;
                double Rx;
                double Pplus;
                double Pminus;
                double durationReflectedWavePressure;
                double durationReflectedWaveDischarge;
                double Iplus;
                double Iminus;
                double K;

                double Ix;
                double deltaP;

                double ti;
                double I;
                double q;
                double st = 1.0 / 3.0;
                double alpha;

                double D;

                double p0;
                double i0;
                double l;

                TableData[] Data;
               
                l = Double.Parse(ReflectionData[n].RadiusColumn) + 1;





                do
                {
                    ri += Double.Parse(ForReflectionData.Step);
                    Rx = l / Math.Pow((Double.Parse(ForReflectionData.EnergyStorage) / 101325), (1.0 / 3.0));
                    alpha = 100*l/ Math.Pow(Double.Parse(ForReflectionData.EnergyStorage) , (1.0 / 3.0));


                    Pplus = Math.Exp(1.264 - 2.056 * Math.Log(alpha) + 0.211 * Math.Pow(Math.Log(alpha), 2))*101325;
                    Pminus = Math.Exp(-0.673 - 1.043* Math.Log(alpha) + 0.252* Math.Pow(Math.Log(alpha), 2))*101325;
                    durationReflectedWavePressure = Math.Exp(-0.109 + 0.983*Math.Log(alpha) - 0.23* Math.Pow(Math.Log(alpha), 2))*Math.Pow(Double.Parse(ForReflectionData.EnergyStorage), (1.0 / 3.0))/Math.Pow(10,5);
                    durationReflectedWaveDischarge = Math.Exp(1.265 + 0.857* Math.Log(alpha) - 0.192*Math.Pow(Math.Log(alpha), 2)) * Math.Pow(Double.Parse(ForReflectionData.EnergyStorage), (1.0 / 3.0)) / Math.Pow(10, 5);
                    Iplus = Math.Exp(-0.07 - 1.033 * Math.Log(alpha) - 0.045 * Math.Pow(Math.Log(alpha), 2)) * Math.Pow(Double.Parse(ForReflectionData.EnergyStorage), (1.0 / 3.0)) / Math.Pow(10, 5);
                    Iminus = Math.Exp(-0.052 - 0.462 * Math.Log(alpha) - 0.27 * Math.Pow(Math.Log(alpha), 2)) * Math.Pow(Double.Parse(ForReflectionData.EnergyStorage), (1.0 / 3.0));
                    ti = durationReflectedWavePressure + durationReflectedWaveDischarge;
                    K= 0.889 - 0.356* Math.Log(alpha) + 0.105* Math.Pow(Math.Log(alpha), 2);
                    deltaP = Pplus * (Math.Sin(3.14*(ti- durationReflectedWavePressure)/ durationReflectedWaveDischarge)/Math.Sin(-3.14* durationReflectedWavePressure)/ durationReflectedWaveDischarge)*Math.Exp((-K*ti)/ durationReflectedWavePressure);


                    if (ti > durationReflectedWaveDischarge)
                    {
                        A.Add(new TableData()
                        {
                            RadiusColumn = Math.Round(ri,2).ToString(),
                            MaxPresureColumn = Math.Round((Pplus / 1000),2).ToString(),
                            WavePresureColumn = Math.Round(Iplus,2).ToString(),
                            Time = Math.Round(ti,2).ToString(),
                        });
                    }
                    else 
                    {

                        A.Add(new TableData()
                        {
                            RadiusColumn = Math.Round(ri,2).ToString(),
                            MaxPresureColumn = Math.Round((Pminus / 1000),2).ToString(),
                            WavePresureColumn = Math.Round(Iplus,2).ToString(),
                            Time = Math.Round(ti,2).ToString(),
                        });
                    }


                        l += Double.Parse(ForReflectionData.Step);

                } while (ri <= Double.Parse(ReflectionData[n].RadiusColumn));
                //} while (Math.Round(deltaP, 0) > 4);

               
               
                DrowData();
            }

            else 
            {
            
            }
        
        }



        public void DrowData()
        {
            ReflectionWavePresurePoints.Clear();
            HandWavePresurePoints.Clear();

            if (A == null || !A.Any()) return;

            // Добавляем точки
            foreach (var item in A)
            {
                if (item == A[0])
                {
                    
                    if (double.TryParse(item.RadiusColumn, out double x) &&
                            double.TryParse(item.MaxPresureColumn, out double y))
                    {
                        x = 0.0;
                        ReflectionWavePresurePoints.Add(new ObservablePoint(x, y));
                    }
                }
                else
                {
                    if (double.TryParse(item.RadiusColumn, out double x) &&
                        double.TryParse(item.MaxPresureColumn, out double y))
                    {
                        ReflectionWavePresurePoints.Add(new ObservablePoint(x, y));
                    }
                }
            }

            if (HandData == null || !HandData.Any()) return;

            // Добавляем точки
            foreach (var item in HandData)
            {
                if (item == HandData[0])
                {

                    if (double.TryParse(item.RadiusColumn, out double x) &&
                            double.TryParse(item.MaxPresureColumn, out double y))
                    {
                        x = 0.0;
                        HandWavePresurePoints.Add(new ObservablePoint(x, y));
                    }
                }
                else
                {
                    if (double.TryParse(item.RadiusColumn, out double x) &&
                        double.TryParse(item.MaxPresureColumn, out double y))
                    {
                        HandWavePresurePoints.Add(new ObservablePoint(x, y));
                    }
                }
            }

            // ОБНОВЛЯЕМ существующую коллекцию, а не создаем новую
            ReflectionWavePresureSeries.Clear();
            HandWavePresureSeries.Clear();

            if (ReflectionWavePresurePoints.Any())
            {
                ReflectionWavePresureSeries.Add(new LineSeries<ObservablePoint>
                {
                    Values = ReflectionWavePresurePoints,
                    Stroke = new SolidColorPaint(SKColors.GreenYellow, 2),
                    Fill = null,
                    GeometrySize = 0,
                    LineSmoothness = 0.4 // Уменьшите для более угловатых линий
                });
            }

            if (HandWavePresurePoints.Any())
            {
                HandWavePresureSeries.Add(new LineSeries<ObservablePoint>
                {
                    Values = HandWavePresurePoints,
                    Stroke = new SolidColorPaint(SKColors.GreenYellow, 2),
                    Fill = null,
                    GeometrySize = 0,
                    LineSmoothness = 0.4 // Уменьшите для более угловатых линий
                });
            }

            // Уведомляем об изменениях
            OnPropertyChanged(nameof(ReflectionWavePresureSeries));

            //HandWavePresureSeries = new ObservableCollection<ISeries>
            //{
            //new LineSeries<ObservablePoint>
            //{
            //    Values = HandWavePresurePoints,
            //    Stroke = new SolidColorPaint(new SKColor(0, 250, 154), 2),
            //    Fill = null,
            //    GeometrySize = 0,
            //    LineSmoothness = 4
            //}

            //};

        }



    }
}
