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
            //if (!b)
            //{
            //    for (double r = 0; r <= RadiationGrid.R; r += RadiationGrid.stepR)
            //    {
            //        for (double fi = -1; fi < 1; fi += RadiationGrid.stepFi)
            //            System.Diagnostics.Debug.WriteLine(RadiationGrid.GetIntensity(r, fi));
            //    }
            //}
            //b = true;
            return oldGrid;
        }
    }
}
