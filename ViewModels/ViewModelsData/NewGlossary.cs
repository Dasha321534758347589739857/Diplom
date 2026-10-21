using Diplom.Data.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.ViewModels.ViewModelsData
{
    public class NewGlossary
    {
        public int? IdSubstance { get; set; }
        public string? NameSubstance { get; set; }

        public string? beta { get; set; }

        public string? minConcentration { get; set; }

        public string? maxConcentration { get; set; }

        public Substance? Substance { get; set; }
    }
}
