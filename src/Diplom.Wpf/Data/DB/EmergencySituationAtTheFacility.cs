using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Data.DB
{
    public class EmergencySituationAtTheFacility
    {
        public int Id { get; set; }

      

        public int IdAir { get; set; }

        public int IdPriming { get; set; } // id грунта

        public int IdSituation { get; set; }

        public int IdWatcher { get; set; }

        public int IdUser { get; set; }

        public int IdSubstanceIAO { get; set; }

        //public string? Temperature { get; set; }

        //public string? ExtentOfDamageToBuildings { get; set; } //степень повреждения строений

        public string? StepMeasurement { get; set; } // шаг измерения

        public string? XEmergencyLocation { get; set; } // место нештатной ситуацииX

        public string? YEmergencyLocation { get; set; } // место нештатной ситуацииY
        public string? XWatcher { get; set; } // место нештатной ситуации

        public string? YWatcher { get; set; } // место нештатной ситуации

       

        public Priming? Priming { get; set; }

        public EmergencySituation? EmergencySituation { get; set; }

        public Air? Air { get; set; }

        public Watcher? Watcher { get; set; }

        public User? User { get; set; }

        public SubstanceInAnObject? SubstanceInAnObject { get; set; }

        //public List<CalculationModel>? CalculationModels { get; set; } = new();



    }
}
