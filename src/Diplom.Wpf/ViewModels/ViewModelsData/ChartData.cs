using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.ViewModels.ViewModelsData
{
    public class ChartData //класс строения графика
    {
        public List<double>? X { get; set; } = new List<double>(); 

        public List<double>? Y { get; set; } = new List<double>(); 
    }
}
