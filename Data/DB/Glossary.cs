using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Data.DB
{
    public class Glossary
    {
        public int Id { get; set; }

        public int? IdSubstance { get; set; }

        public string NameSubstance { get; set; }

        public string beta { get; set; }

        public string minConcentration { get; set; }

        public string maxConcentration { get;set; }

        public Substance? Substance { get; set; }

    }
}
