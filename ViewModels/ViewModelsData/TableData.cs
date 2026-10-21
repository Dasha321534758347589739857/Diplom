using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.ViewModels.ViewModelsData
{
    public class TableData
    {
        [ColumnName("Нештатная ситуация")]
        public string SituationColumn { get; set; }
        [ColumnName("Расстояние от эпицентра")]
        public string RadiusColumn { get; set; }

        [ColumnName("Избыточное давление")]
        public string MaxPresureColumn { get; set; }

        [ColumnName("Импульс волны давления")]
        public string WavePresureColumn { get; set; }

        [ColumnName("Время")]
        public string Time { get; set; }
        
    }
}
