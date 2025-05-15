using Diplom.Data.DB;
using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Diplom.MatModel
{
    public class Calculate
    {
        private readonly double step;
        private readonly double radius;//радиус
        private readonly double height;//высота
        private readonly double roCTX;//стехиометрическая плотность метана в воздухе
        private readonly double a;//эмпирический коэффициент
        private readonly double h;//толщина грунта
        private readonly double DCT;//толщина стенок резервуара !! добавить в базу сущность материал, а в объект толщина стенок
        private readonly double roCT;//плотность материала резервуара
        private readonly double roGr;//плотность грунта
        private readonly double x0;//точка эпицентра взрыва по х
        private readonly double y0;//точка эпицентра взрыва по y
        private readonly double xWatcher;
        private readonly double yWatcher;
        private double distance;
        private double deltaPP;
        private double pppp;


        public Calculate(double step,double radius, double height, double a, double roCT, double DCT, double h, double roGr, double x0, double y0, double xWatcher, double yWatcher, double roCTX)
        { 
            this.step = step;
            this.radius = radius;
            this.height = height;
            this.h = h;
            this.x0 = x0;
            this.y0 = y0;
            this.xWatcher = xWatcher;
            this.yWatcher = yWatcher;
            this.a = a;
            this.DCT = DCT;
            this.roCT = roCT;
            this.roGr = roGr;
            this.roCTX = roCTX;
                
        }


        public List<double> GetPresure()
        {
            List<double> presure= new List<double>();
            double B;
            double deltaP;
            double ri=0;
            double V= Math.PI * Math.Pow(radius, 2.0) * height;
            double V3 = Math.Pow(V, (1.0 / 3.0));
            double V3R;

            B = (Math.Pow(V,(1.0/3.0)))/(h+(DCT*(roCT/roGr)));


            do
            {
                if (ri == 0)
                { ri += 1; }
                else 
                {
                    ri += step;
                }

                V3R = V3 / ri;
                deltaP = 37.5 * a * roCTX * (Math.Pow(B, (1.0 / 3.0))) * (Math.Pow(V3R, (207.0 / 100.0)));
                presure.Add(deltaP);

            } while (Math.Round(deltaP, 0) > 0);

                return presure;
        }


        public double GetDistance()
        {
            
            double fu = Math.Pow((x0 - xWatcher), 2.0);//a
            double fi = Math.Pow((y0 - yWatcher), 2.0);//b
            double fuplusfi = Math.Pow(fu + fi, (1.0 / 2.0));//c
            distance = Math.Round(fuplusfi, 2);
            return distance;
        }

        public double GetProcent(double pr)
        {
            double procent;

            if (pr <= 3.72)
            {
                procent = 0.0;
            }
            else if (3.72 <= pr && pr < 4.16)
            {
                procent = 10.0;
            }
            else if (4.16 <= pr && pr < 4.48)
            {
                procent = 20.0;
            }
            else if (4.48 <= pr && pr < 4.75)
            {
                procent = 30.0;
            }
            else if (4.75 <= pr && pr < 5.0)
            {
                procent = 40.0;
            }
            else if (5.0 <= pr && pr < 5.25)
            {
                procent = 50.0;
            }
            else if (5.25 <= pr && pr < 5.52)
            {
                procent = 60.0;
            }
            else if (5.52 <= pr && pr < 5.84)
            {
                procent = 70.0;
            }
            else if (5.84 <= pr && pr < 6.28)
            {
                procent = 80.0;
            }
            else if (6.28 <= pr && pr < 7.33)
            {
                procent = 90.0;

            }
            else 
            {
                procent = 100.0;
            }
            return procent;

        }

        public (double,double,double,double) GetOrientation(double E,double st, double Vw, double m)
        {
            double P0=101325.0;//атмосферное давление
            double Co = 343.0;//скорость звука
            double Rx;
            double Ix;
            double Px;
            double V3;
            double V5;
            double I;
            double p;
            double i;
            double a;
            double b;
            double c;
            double d;
            double e;
            double eardrumms;
            double throwWay;
            double orientation;
            double eardrummsPr;
            double throwWayPr;
            double orientationPr;


            Rx = distance / (Math.Pow((E / P0), st));
            a = (6.0 * Vw) / (7.0 * Co);
            b = 0.4 * a;
            c = 0.06 / Rx;
            d = Math.Pow(Rx, 2);
            e = Math.Pow(Rx, 3);

            if (Rx < 0.8 && Rx > 0.2)
            {
                Ix = Math.Exp(-3.3228 - (1.3689 * Math.Log(Rx)) - (0.9057 * (Math.Pow(Math.Log(Rx), 2.0))) - (0.4818 * (Math.Pow(Math.Log(Rx), 3.0))));
                Px = Math.Exp(-0.9278 - (1.5415 * Math.Log(Rx)) + (0.1953 * (Math.Pow(Math.Log(Rx), 2.0))) - (0.0285 * (Math.Pow(Math.Log(Rx), 3.0))));
            }
            else if (Rx >= 0.8 && Rx < 50)
            {
                Ix = Math.Exp(-3.2656 - (0.9641 * Math.Log(Rx)) - (0.0180 * (Math.Pow(Math.Log(Rx), 2.0))));
                Px = Math.Exp(-0.9278 - (1.5415 * Math.Log(Rx)) + (0.1953 * (Math.Pow(Math.Log(Rx), 2.0))) - (0.0285 * (Math.Pow(Math.Log(Rx), 3.0))));
            }
            else
            {
                Ix = 0.16;
                Px = 18.0;
            }

            deltaPP = Px * P0; //Избыточное давление в кПа
            I = (Ix * (Math.Pow(P0, 2.0 / 3.0)) * (Math.Pow(E, st) / Co));//
            i = (I/(Math.Pow(P0,1.0/2.0)* Math.Pow(m, 1.0 / 3.0)));
            p = 1.0 + (deltaPP / P0);
            V3 = (4.2 / p) + (1.3 / i);
            V5 = (0.00738 / deltaPP) + ((1.3 * Math.Pow(10.0, 9.0)) / (deltaPP * I));
            orientationPr = 5 - (5.74 * Math.Log(V3));
            eardrummsPr = -12.6 + (1.524 * (Math.Log(deltaPP)));
            throwWayPr = 5 - (2.44 * Math.Log(V5));
            orientation = GetProcent(orientationPr);
            eardrumms = GetProcent(eardrummsPr);
            throwWay = GetProcent(throwWayPr);


            return (orientation, eardrumms, throwWay, deltaPP);
        }

        

        public string GetDamage()
        {
            string degreeOfDamage="";

            if (deltaPP <= 75)
            {
                degreeOfDamage = "слабое";
            }
            else if (deltaPP <= 150 && deltaPP > 75)
            {
                degreeOfDamage = "среднее";
            }
            else if (deltaPP <= 200 && deltaPP > 150)
            {
                degreeOfDamage = "сильное";
            }
            else 
            {
                degreeOfDamage = "полное";
            }
            return degreeOfDamage;
        }
        public string GetResperatorsOrgan()
        {
            string respiratorsOrgans = "";

            if (deltaPP <= 65900)
            {
                respiratorsOrgans = "маловероятны";
            }
            else if (deltaPP <= 243000 && deltaPP > 65900)
            {
                respiratorsOrgans = "порог выживания";
            }
            else 
            {
                respiratorsOrgans = "смертельно";
            }
            
            return respiratorsOrgans;
        }

    }
}
