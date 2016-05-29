using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntertainingExplosion.Core.Models
{
    static class RadiationGrid
    {
        public static double R;
        public static double stepR;
        public static double stepFi;
        static double[,]intensityMatrix;

        static public void InitRadiationGrid(double R1, double stepR1, double stepFi1)
        {
            R = R1;
            stepFi = stepFi1;
            stepR = stepR1;
            int rCount = (int)Math.Round(R / stepR);
            int cosFiCount = (int)Math.Round(2.0/stepFi);
            intensityMatrix = new double[rCount + 1, cosFiCount + 1];
        }

        static public double GetIntensity(double R, double CosFi)
        {
            int r = (int)Math.Round(R / stepR);
            int cosFi = (int)Math.Round((CosFi + 1) / stepFi);
            return intensityMatrix[r,cosFi];
        }

        static public void SetIntensity(double R, double CosFi, double value)
        {
            int r = (int)Math.Round(R / stepR);
            int cosFi = (int)Math.Round((CosFi + 1) / stepFi);
            intensityMatrix[r, cosFi] = value;
        }
    }
}
