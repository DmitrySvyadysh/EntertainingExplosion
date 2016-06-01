using System;
using EntertainingExplosion.Core.Models;

namespace EntertainingExplosion.Core.Engine
{
    public static class RadiationRecounter
    {
        public static bool b = false;
        public static double timeCoef;
        public static double ellipseCoef;
        public static double absorbCoef;
        public static double[] array;
        public static double Square(int count, double size)
        {
            double sq = Math.PI * size * size * (2 * count + 1);
            return sq;
        }

        public static double Burger(double prevI, double dist, double coef)
        {
            return prevI * Math.Exp(-(dist / ellipseCoef) * coef);
        }

        public static double InitializeBounds(int param, Grid oldGrid, double prevI, double r)
        {
            double I = Burger(prevI, RadiationGrid.stepR, absorbCoef) +
                    timeCoef * AnglePart(-RadiationGrid.stepFi / 2, RadiationGrid.stepFi/2) *
                    Square((int)Math.Round(r/RadiationGrid.stepR), RadiationGrid.stepR)*
                    Math.Pow(oldGrid.Cells[(int)Math.Round(r / RadiationGrid.stepR)].Temperature, 4) -
                    Burger(timeCoef * AnglePart(-RadiationGrid.stepFi / 2, RadiationGrid.stepFi/2) *
                    Square((int)Math.Round(r / RadiationGrid.stepR), RadiationGrid.stepR) *
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

        public static double InnerEnergy(Cell c, int i)
        {
            double en = 5 / 2 * c.MolarMass * c.Temperature * 8.31 * Square(i, c.Size);
            return en;
        }

        public static double NewTemperature(double EnergyChange, Cell c, int i)
        {
            double temp = (InnerEnergy(c, i) + EnergyChange) / (Square(i, c.Size) * c.MolarMass * 8.31 * 5 / 2);
            if (temp < 0)
                temp = 0;
            return temp;
        }

        public static double AnglePart(double cos1, double cos2)
        {
            double d;
            d = Math.Abs(Math.Acos(cos2) - Math.Acos(cos1));
            return d / (Math.PI*2);
        }

        public static Grid CreateNewGrid(Grid oldGrid, double deltaTime)
        {
            timeCoef = deltaTime * Math.Pow(10,-12);
            absorbCoef = 0.1;
            ellipseCoef = 0.4;

            for (double cosFi = -1; cosFi <= 0; cosFi += RadiationGrid.stepFi)
            {
                RadiationGrid.SetIntensity(RadiationGrid.R, cosFi, 0);
            }

            for (double r = RadiationGrid.R - RadiationGrid.stepR; r >= 0; r -= RadiationGrid.stepR)
            {
                double prevI = RadiationGrid.GetIntensity(r + RadiationGrid.stepR, -1);
                double I = InitializeBounds(-1, oldGrid, prevI, r);
                RadiationGrid.SetIntensity(r, -1, I);
                oldGrid.Cells[(int)Math.Round(r / RadiationGrid.stepR)].Izluchenie += I;
            }

            RadiationGrid.SetIntensity(0, 1, RadiationGrid.GetIntensity(0, -1));

            for (double r = RadiationGrid.stepR; r <= RadiationGrid.R; r += RadiationGrid.stepR)
            {
                double prevI = RadiationGrid.GetIntensity(r - RadiationGrid.stepR, 1);
                double I = InitializeBounds(1, oldGrid, prevI, r - RadiationGrid.stepR);
                RadiationGrid.SetIntensity(r, 1, I);
                oldGrid.Cells[(int)Math.Round((r - RadiationGrid.stepR) / RadiationGrid.stepR)].Izluchenie += I;
            }

            for (double cosFi = -1 + RadiationGrid.stepFi; cosFi <= 0; cosFi += RadiationGrid.stepFi)
            {
                for (double r = RadiationGrid.R - RadiationGrid.stepR; r >= RadiationGrid.stepR; r -= RadiationGrid.stepR)
                {
                    double prevI;
                    double suppressionFi;
                    double suppressionR;
                    double p;
                    double sinFi = Math.Pow(1 - cosFi * cosFi, 0.5);
                    p = r * sinFi;
                    suppressionFi = p / (r + RadiationGrid.stepR);
                    double cos = -Math.Pow(1 - suppressionFi * suppressionFi, 0.5);
                    if (cos > cosFi - RadiationGrid.stepFi)//пересекает справа
                    {
                        suppressionR = r + RadiationGrid.stepR;
                        prevI = RadiationGrid.GetIntensity(r + RadiationGrid.stepR, cosFi) *
                            ((cosFi - cos) / RadiationGrid.stepFi) +
                            RadiationGrid.GetIntensity(r + RadiationGrid.stepR, cosFi - RadiationGrid.stepFi) *
                            ((1 - cosFi + cos) / RadiationGrid.stepFi);
                    }
                    else//пересекает снизу
                    {
                        suppressionFi = Math.Pow(1 - (cosFi- RadiationGrid.stepFi) * (cosFi - RadiationGrid.stepFi), 0.5);
                        suppressionR = p / suppressionFi;
                        prevI = RadiationGrid.GetIntensity(r, cosFi - RadiationGrid.stepFi) *
                            ((suppressionR - r) / RadiationGrid.stepR) +
                            RadiationGrid.GetIntensity(r + RadiationGrid.stepR, cosFi - RadiationGrid.stepFi) *
                            ((1 + r - suppressionR) / RadiationGrid.stepR);
                    }

                    double dist = Distance(r, cosFi, suppressionR, -Math.Sqrt(1 - Math.Pow(suppressionFi, 2)));
                    double I = Burger(prevI, dist, absorbCoef) +
                    timeCoef * AnglePart(cosFi + RadiationGrid.stepFi / 2, cosFi - RadiationGrid.stepFi/2) *
                    Square((int)Math.Round(r / RadiationGrid.stepR), RadiationGrid.stepR) *
                    Math.Pow(oldGrid.Cells[(int)Math.Round(r / RadiationGrid.stepR)].Temperature, 4) -
                    Burger(timeCoef * AnglePart(cosFi + RadiationGrid.stepFi / 2, cosFi - RadiationGrid.stepFi/2) *
                    Square((int)Math.Round(r / RadiationGrid.stepR), RadiationGrid.stepR) *
                    Math.Pow(oldGrid.Cells[(int)Math.Round(r / RadiationGrid.stepR)].Temperature, 4),
                    dist, absorbCoef);
                    RadiationGrid.SetIntensity(r, cosFi, I);
                    oldGrid.Cells[(int)Math.Round(r / RadiationGrid.stepR)].Izluchenie += I;
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
                        if (prevI < 0)
                        {
                            System.Diagnostics.Debug.WriteLine("problem is here!");
                        }
                    }

                    double dist = Distance(r, cosFi, suppressionR, Math.Sqrt(1 - Math.Pow(suppressionFi, 2)));
                    double I = Burger(prevI, dist, absorbCoef) +
                    timeCoef * AnglePart(cosFi + RadiationGrid.stepFi / 2, cosFi - RadiationGrid.stepFi/2) *
                    Square((int)Math.Round(r / RadiationGrid.stepR), RadiationGrid.stepR) *
                    Math.Pow(oldGrid.Cells[(int)Math.Round((r - RadiationGrid.stepR) / RadiationGrid.stepR)].Temperature, 4) -
                    Burger(timeCoef * AnglePart(cosFi + RadiationGrid.stepFi / 2, cosFi - RadiationGrid.stepFi/2) *
                    Square((int)Math.Round(r / RadiationGrid.stepR), RadiationGrid.stepR) *
                    Math.Pow(oldGrid.Cells[(int)Math.Round((r - RadiationGrid.stepR) / RadiationGrid.stepR)].Temperature, 4),
                    dist, absorbCoef);
                    RadiationGrid.SetIntensity(r, cosFi, I);
                    oldGrid.Cells[(int)Math.Round((r - RadiationGrid.stepR) / RadiationGrid.stepR)].Izluchenie += I;
                }
            }

            for(int i = 0; i < oldGrid.Cells.Count; i++)
            {
                

                double balance = 0;
                for(double cosFi = -1 + RadiationGrid.stepFi; cosFi < 0; cosFi += RadiationGrid.stepFi)
                {
                    balance += RadiationGrid.GetIntensity((i + 1) * RadiationGrid.stepR, cosFi);
                    balance -= RadiationGrid.GetIntensity(i * RadiationGrid.stepR, cosFi);
                }

                for (double cosFi = RadiationGrid.stepFi; cosFi <= 1 - RadiationGrid.stepFi; cosFi += RadiationGrid.stepFi)
                {
                    balance -= RadiationGrid.GetIntensity((i + 1) * RadiationGrid.stepR, cosFi);
                    balance += RadiationGrid.GetIntensity(i * RadiationGrid.stepR, cosFi);
                }
                oldGrid.Cells[i].Temperature = NewTemperature(balance, oldGrid.Cells[i], i);
            }
            
            b = true;
            return oldGrid;
        }
    }
}