using System;
using EntertainingExplosion.Core.Models;

namespace EntertainingExplosion.Core.Engine
{
    public static class RadiationRecounter
    {
        public static bool b = false;
        public static double timeCoef;
        public static double absorbCoef;
        public static double[] array;
        public static double Square(int count, double size)
        {
            double sq = Math.PI * size * size * (2 * count + 1);
            return sq;
        }

        public static double Burger(double prevI, double dist, double coef)
        {
            return prevI * Math.Exp(-dist * coef);
        }

        public static double InitializeBounds(int param, Grid oldGrid, double prevI,double r)
        {
            double I = Burger(prevI, RadiationGrid.stepR, absorbCoef) +
                    timeCoef * absorbCoef *
                    Math.Pow(oldGrid.Cells[(int)Math.Round(r / RadiationGrid.stepR)].Temperature, 4) -
                    Burger(timeCoef * absorbCoef *
                    Math.Pow(oldGrid.Cells[(int)Math.Round(r / RadiationGrid.stepR)].Temperature, 4),
                    RadiationGrid.stepR, absorbCoef);
            return I;
        }

        public static double Distance(double r1, double cos1, double r2, double cos2)
        {
            double x1 = r1 * cos1;
            double y1 = r1 * Math.Pow(1 - cos1 * cos1, 0.5);

            double x2 = r2 * cos2;
            double y2 = r2 * Math.Pow(1 - cos2 * cos2, 0.5);

            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public static Grid CreateNewGrid(Grid oldGrid, double deltaTime)
        {
            timeCoef = 0.1;
            absorbCoef = 0.5;
            
            for(double cosFi = -1; cosFi <= 0; cosFi+= RadiationGrid.stepFi)
            {
                RadiationGrid.SetIntensity(RadiationGrid.R, cosFi, 0);
            }


            for (double r = RadiationGrid.R - RadiationGrid.stepR; r >= 0; r -= RadiationGrid.stepR)
            {
                double prevI = RadiationGrid.GetIntensity(r + RadiationGrid.stepR, -1);
                double I = InitializeBounds(-1, oldGrid, prevI, r);
                RadiationGrid.SetIntensity(r, -1, I);
            }

            RadiationGrid.SetIntensity(0, 1, RadiationGrid.GetIntensity(0,-1));

            for (double r = RadiationGrid.stepR; r <= RadiationGrid.R; r += RadiationGrid.stepR)
            {
                double prevI = RadiationGrid.GetIntensity(r - RadiationGrid.stepR, 1);
                double I = InitializeBounds(1, oldGrid, prevI, r - RadiationGrid.stepR);
                RadiationGrid.SetIntensity(r, 1, I);
            }

            for(double cosFi = -1 + RadiationGrid.stepFi; cosFi <= 0; cosFi += RadiationGrid.stepFi)
            {
                for(double r = RadiationGrid.R - RadiationGrid.stepR; r >= RadiationGrid.stepR; r -= RadiationGrid.stepR)
                {
                    double prevI;
                    double suppressionFi;
                    double suppressionR;
                    double p;
                    double sinFi = Math.Pow(1 - cosFi * cosFi, 0.5);
                    p = r * sinFi;
                    suppressionFi = p / (r + RadiationGrid.stepR);
                    double cos = - Math.Pow(1 - suppressionFi * suppressionFi, 0.5);
                    if (cos > cosFi - RadiationGrid.stepFi)//пересекает справа
                    {
                        suppressionR = r + RadiationGrid.stepR;
                        prevI = RadiationGrid.GetIntensity(r + RadiationGrid.stepR, cosFi) *
                            ((cosFi - cos) / RadiationGrid.stepFi) +
                            RadiationGrid.GetIntensity(r + RadiationGrid.stepR, cosFi + RadiationGrid.stepFi) *
                            ((1 - cosFi + cos) / RadiationGrid.stepFi);
                    }
                    else//пересекает снизу
                    {
                        suppressionR = p / suppressionFi;
                        prevI = RadiationGrid.GetIntensity(r, cosFi + RadiationGrid.stepFi) *
                            ((r - suppressionR) / RadiationGrid.stepR) +
                            RadiationGrid.GetIntensity(r + RadiationGrid.stepR, cosFi + RadiationGrid.stepFi) *
                            ((1 - r + suppressionR) / RadiationGrid.stepR);
                    }

                    double dist = Distance(r, cosFi, suppressionR, -Math.Sqrt(1 - Math.Pow(suppressionFi, 2)));
                    /*if (!b)
                    {
                        System.Diagnostics.Debug.WriteLine("{0} {1} {2} {3}", r, cosFi, suppressionR, -Math.Sqrt(1 - Math.Pow(suppressionFi, 2)));
                        System.Diagnostics.Debug.WriteLine(dist);
                    }*/
                    double I = Burger(prevI, dist, absorbCoef) +
                    timeCoef * absorbCoef *
                    Math.Pow(oldGrid.Cells[(int)Math.Round(r / RadiationGrid.stepR)].Temperature, 4) -
                    Burger(timeCoef * absorbCoef *
                    Math.Pow(oldGrid.Cells[(int)Math.Round(r / RadiationGrid.stepR)].Temperature, 4),
                    dist, absorbCoef);
                    RadiationGrid.SetIntensity(r, cosFi, I);
                }
            }

            for (double cosFi = RadiationGrid.stepFi; cosFi <= 1 - RadiationGrid.stepFi; cosFi += RadiationGrid.stepFi)
            {
                for (double r = RadiationGrid.stepR; r <= RadiationGrid.R; r += RadiationGrid.stepR)
                {
                    double prevI;
                    double suppressionFi;
                    double suppressionR;
                    double p;
                    double sinFi = Math.Pow(1 - cosFi * cosFi, 0.5);
                    double sinFi2 = Math.Pow(1 - (cosFi - RadiationGrid.stepFi) * (cosFi - RadiationGrid.stepFi), 0.5);
                    p = r * sinFi;
                    suppressionR = p / sinFi2;
                    double cos;
                    if (suppressionR > r - RadiationGrid.stepR)//снизу
                    {
                        cos = cosFi - RadiationGrid.stepFi;
                        suppressionFi = Math.Pow(1 - cos * cos, 0.5);
                        prevI = RadiationGrid.GetIntensity(r, cosFi - RadiationGrid.stepFi) *
                            ((r - suppressionR) / RadiationGrid.stepR) +
                            RadiationGrid.GetIntensity(r - RadiationGrid.stepR, cosFi - RadiationGrid.stepFi) *
                            ((1 - r + suppressionR) / RadiationGrid.stepFi);
                    }
                    else//слева
                    {
                        suppressionR = r - RadiationGrid.stepR;
                        suppressionFi = p / suppressionR;
                        cos = Math.Pow(1 - suppressionFi * suppressionFi, 0.5);
                        prevI = RadiationGrid.GetIntensity(r - RadiationGrid.stepR, cosFi) *
                            ((cosFi - cos) / RadiationGrid.stepFi) +
                            RadiationGrid.GetIntensity(r - RadiationGrid.stepR, cosFi - RadiationGrid.stepFi) *
                            ((1 - cosFi + cos) / RadiationGrid.stepFi);
                    }

                    double dist = Distance(r, cosFi, suppressionR, Math.Sqrt(1 - Math.Pow(suppressionFi, 2)));
                    /*if (!b)
                    {
                        //System.Diagnostics.Debug.WriteLine(RadiationGrid.GetIntensity(RadiationGrid.R, RadiationGrid.stepFi));
                        System.Diagnostics.Debug.WriteLine("{0} {1} {2} {3}", r, cosFi, suppressionR, Math.Sqrt(1 - Math.Pow(suppressionFi, 2)));
                        System.Diagnostics.Debug.WriteLine(dist);
                    }*/
                    double I = Burger(prevI, dist, absorbCoef) +
                    timeCoef * absorbCoef *
                    Math.Pow(oldGrid.Cells[(int)Math.Round((r-RadiationGrid.stepR) / RadiationGrid.stepR)].Temperature, 4) -
                    Burger(timeCoef * absorbCoef *
                    Math.Pow(oldGrid.Cells[(int)Math.Round((r - RadiationGrid.stepR) / RadiationGrid.stepR)].Temperature, 4),
                    dist, absorbCoef);
                    RadiationGrid.SetIntensity(r, cosFi, I);
                }
            }

            if (!b)
            {
                for (double r = 0; r <= RadiationGrid.R; r += RadiationGrid.stepR)
                {
                    for (double fi = -1; fi <= 1; fi += RadiationGrid.stepFi)
                        System.Diagnostics.Debug.Write(RadiationGrid.GetIntensity(r, fi)+"  ");
                    System.Diagnostics.Debug.WriteLine("\n");
                }
            }
            b = true;
            return oldGrid;
        }
    }
}
