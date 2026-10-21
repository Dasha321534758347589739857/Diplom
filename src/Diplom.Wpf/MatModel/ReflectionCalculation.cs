using Diplom.ViewModels.ViewModelsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diplom.MatModel
{
    public class ReflectionCalculation
    {
        private double angleReflection;//угол отражения

        private double angleIncidence;//угол падения

        private readonly double distanceToTriplePoint;//расстояние до тройной точки

        private List<double> ImpulsReflection;
        
        private List<double> PresureReflection;

        private List<double> TimeReflection;

        private List<double> DistanceReflection;

        private double TriplePointImpuls;

        private double TriplePointPresure;

        private double TriplePointDistance;

        public void Calculate(NewForReflectionData dataIndence, TableData dataWaveIndense, double angleOfIncidence)
        {

            double xPointReflection;
            double yPointReflection;



            angleIncidence = angleOfIncidence;
            angleReflection = angleIncidence;


            xPointReflection = (Double.Parse(dataIndence.YEpicenter) / Math.Tan(angleIncidence) + Double.Parse(dataIndence.XEpicenter));
            yPointReflection = 0.0;
            
            


        }
        public double GetDouble(string parameter) // перевод из стрингов в даблы
        {
            double parameterDouble;
            bool message = Double.TryParse(parameter, out parameterDouble);

            if (message == false)
            {
                MessageBox.Show("Проверьте корректность введённых параметров.");
                parameterDouble = 0.0;
            }
            else 
            {
                parameterDouble = Math.Round(parameterDouble, 2);
            }

                return parameterDouble;

        }


        public (double,double) GetHandWaveEpicenter(string reflectionXEpicenter,string reflectionYEpicenter, string angleReflection) // получение эпицентра головной ударной волны
        {
            double x = 0.0;
            double y = 0.0;
            double angle = GetDouble(angleReflection); // угол отражения
            double catheterY = GetDouble(reflectionYEpicenter); // катет по оси х
            double catheterX = GetDouble(reflectionXEpicenter); // катет по оси у
            double degreeInRadians = Math.PI / 180.0; // перевод градусов в радианы

            x = catheterY * Math.Tan(angle * degreeInRadians) * 2 + catheterX;
            y = catheterY;

            return (x,y);
        }



       

    }
}
