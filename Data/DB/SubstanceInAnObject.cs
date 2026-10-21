using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Data.DB
{
    public class SubstanceInAnObject
    {
        public int Id { get; set; }

        public int? IdSubstance { get; set; }

        public int? IdObject { get; set; }
       
        public string? Mass { get; set; } // масса

        //public string? Time { get; set; }

        //public string? Date { get; set; }

        public string? SCOASIA { get; set; } // стехиометрическая концентрация вещества в воздухе

        public string? COASIAO { get; set; } // концентрация вещества в объекте

        public Substance? Substance { get; set; }

        public Objectt? Objectt { get; set; }

        
    }
}
