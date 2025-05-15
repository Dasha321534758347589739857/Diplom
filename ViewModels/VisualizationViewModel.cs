using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Diplom.Windows;
using OpenTK.Graphics.OpenGL;

namespace Diplom.ViewModels
{
    public partial class VisualizationViewModel : ObservableObject
    {
        private readonly double height;
        private readonly double weight;
        private readonly List<double> Z;
        private readonly double step;
        private readonly double x0;
        private readonly double y0;

        [ObservableProperty]
        private ChartWindow chart2DWindow;

        public VisualizationViewModel(double height, double weight, double step, double x0,double y0, List<double> Z)
        {
            Chart2DWindow = new ChartWindow(this);
            this.height = height;
            this.weight = weight;
            this.Z = Z;
            this.step = step;
            this.x0 = x0;
            this.y0 = y0;
        }

        public (List<double> DataX, List<double> DataY, List<double> DataZ) ParseData()
        {
            List<double> DataX = new();
            List<double> DataY = new();
            List<double> DataZ = new();

            double f = 0.0;
            double f1 = 0.0;
            double i = 0.0;
            double j = 0.0;
            double e = 0.0;
            int es = 0;
            string ess = "";
            

            do
            {
                do
                {
                    if (j <= weight && j>=0)
                    {
                        DataX.Add(i);
                        DataY.Add(j);
                        f = i - x0;
                        f1 = j - y0;
                        if (f < 0)
                        { f = f * -1; }
                        if (f1 < 0)
                        { f1 = f1 * -1; }

                        if (((f + f1) / step )> Z.Count || ((f + f1) / step)==Z.Count)
                        {
                            DataZ.Add(4.0);
                        }
                        else
                        {
                            ess = Math.Round(((f + f1) / step), 0).ToString();
                            es = Int32.Parse(ess);
                            DataZ.Add(Z[es]);
                        }
                    }
                    else 
                    {
                       
                        DataY.Add(j);
                       

                    }
                    
                    i = i + step;
                } while (i<= height);
                j = j + step;
                i = 0;
            } while (j<=height);

            return (DataX, DataY, DataZ);
        }

    }
}
