using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using System.IO;
using static MaterialDesignThemes.Wpf.Theme;


namespace Diplom.ViewModels.UserViewModel
{
    public partial class HandWaveViewModel: ObservableObject, IRecipient<ReflectionDataMessage>, IRecipient<NewForReflectionDataMessage>
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
        private ObservableCollection<TableData>? reflectionData;//данные для таблицы

        [ObservableProperty]
        private NewForReflectionData? forReflectionData;


        [ObservableProperty]
        private ObservableCollection<TableData>? handData;//данные для таблицы

        public HandWaveViewModel()
        {
            WeakReferenceMessenger.Default.Register<NewForReflectionDataMessage>(this);
            WeakReferenceMessenger.Default.Register<ReflectionDataMessage>(this);
        }


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

            ForReflectionData = new NewForReflectionData() { XEpicenter= message1.Value.XEpicenter, YEpicenter = message1.Value.XEpicenter, RObject = message1.Value.RObject };

        }


        [RelayCommand]
        private void CalcReflection()
        {
            XReflection = ((Double.Parse(ForReflectionData.YEpicenter) / Math.Tan(Double.Parse(AngleOfIncidence)))+ Double.Parse(ForReflectionData.XEpicenter)).ToString();
            YReflection = "0";
            AngleOfReflection = AngleOfIncidence;
        }



    }
}
