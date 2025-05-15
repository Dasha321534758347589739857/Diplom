using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Data.DB
{
    public class Substance
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? StateOfAggregation { get; set; } // агрегатное состояние

        public string? Density { get; set; } // плотность

        //public string? KlassSubstance { get; set; } // степень чувствительности

        // public string? IgnitionSpeed { get; set; } // скорость воспламенения 


        //public int? M { get; set; } // количество молекул входных
        //public int? M { get; set; } // количество молекул входных
        public string? SpecificHeatOfCombustion { get; set; } // удельная теплота сгорания

        public string? HazardClass { get; set; } // класс опасности

    }
}
