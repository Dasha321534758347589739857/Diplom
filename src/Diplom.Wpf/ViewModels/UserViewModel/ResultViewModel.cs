using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Reflection;
using Diplom.ViewModels.ViewModelsData;
using Diplom.Message;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Diplom.Windows;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using LiveChartsCore;
using System.Drawing;
using Diplom.Data.DB;


namespace Diplom.ViewModels.UserViewModel
{
    public partial class ResultViewModel : ObservableObject, IRecipient<DataMessage>, IRecipient<ChartMessage>, IRecipient<TemperatureMessage>

    {
        [ObservableProperty]
        private ObservableCollection<TableData>? data;//данные для таблицы

        [ObservableProperty]
        private string temperature = "";


        private readonly ObservableCollection<ObservablePoint> ImpulsPoints; //точки для линии импульса
        private readonly ObservableCollection<ObservablePoint> PresurePoints;//точки для линии избыточного давления


        public ResultViewModel()
        {
            WeakReferenceMessenger.Default.Register<DataMessage>(this);
            WeakReferenceMessenger.Default.Register<ChartMessage>(this);
            WeakReferenceMessenger.Default.Register<TemperatureMessage>(this);


            ImpulsPoints = new ObservableCollection<ObservablePoint>();
            PresurePoints = new ObservableCollection<ObservablePoint>();

        }


        public Axis[] XRadius { get; set; } =
        {
        new()
        {
            Name = "Радиус, м",
            NameTextSize = 11
        }
        };

        public Axis[] YImpuls { get; set; } =
        {
        new()
        {
            Name = "Импульс, Па * с",
            NameTextSize = 11
        }
        };

        public Axis[] YPresure { get; set; } =
       {
        new()
        {
            Name = "Избыточное давление, Па",
            NameTextSize = 11
        }
        };

        public ObservableCollection<ISeries> ImpulsSeries { get; set; }
        public ObservableCollection<ISeries> PresureSeries { get; set; }



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
        public void Receive(DataMessage message)
        {
            
            Data = new ObservableCollection<TableData>(message.Value.Select(x => new TableData()
            {
                SituationColumn = x.SituationColumn,
                RadiusColumn = x.RadiusColumn,
                MaxPresureColumn = x.MaxPresureColumn,
                WavePresureColumn = x.WavePresureColumn,
                Time = x.Time,

            })) ;
        }

        public void Receive( ChartMessage chartMessage)
        {
            var l = chartMessage.Value;

            for (var i = 0; i < l.chartDatasI.X.Count(); i++)
            {

                ImpulsPoints.Add(new ObservablePoint(l.chartDatasI.X[i], l.chartDatasI.Y[i]));
                PresurePoints.Add(new ObservablePoint(l.chartDatasP.X[i], l.chartDatasP.Y[i]));


            }


            ImpulsSeries = new ObservableCollection<ISeries>
            {
            new LineSeries<ObservablePoint>
            {
                Values = ImpulsPoints,
                Stroke = new SolidColorPaint(new SKColor(154, 205, 50), 2),
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 4
            }
            

            };

            PresureSeries = new ObservableCollection<ISeries>
            {
            new LineSeries<ObservablePoint>
            {
                Values = PresurePoints,
                Stroke = new SolidColorPaint(new SKColor(0, 250, 154), 2),
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 4
            }
            
            };

        }

        public void Receive(TemperatureMessage message)
        {
            Temperature=message.Value.ToString();
           
        }

    }
}
