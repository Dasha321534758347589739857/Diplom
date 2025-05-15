using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Data.DB
{
    public class CalculationModel
    {
        public int Id { get; set; }

        public int IdEmergencySituation { get; set; }//id нештатной ситуации

        public string? Distance { get; set; }//расстояние от эпицентра (радиус)

        public string? ExcessPressure { get; set; } //избыточное давление

        public string? PressureWaveImpulse { get; set; }// импульс волны давления

        public string? Time { get; set; }// время

        public string? Temperature { get; set; }// температура

        public EmergencySituation? EmergencySituation { get; set; }
    }
}
