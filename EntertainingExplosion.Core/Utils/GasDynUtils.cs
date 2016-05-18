using System;
using EntertainingExplosion.Core.Constants;

namespace EntertainingExplosion.Core.Utils
{
    public static class GasDynUtils
    {
        public static double GetTemperature(
            double molarMass,
            double pressure,
            double density)
        {
            return (molarMass * pressure) / (density * PhysicalConstants.R);
        }

        public static double GetInternalEnergy(
            double pressure,
            double density,
            double heatCapacityRatio,
            double speed)
        {
            return (pressure/(density*(heatCapacityRatio - 1))) + Math.Pow(speed, 2)/2;
        }

        public static double GetPressure(
            double density,
            double molarMass,
            double temperature)
        {
            return (density / molarMass) * Constants.PhysicalConstants.R*temperature;
        }

        public static double GetPressure(
            double density,
            double internalEnergy,
            double speed,
            double heatCapacityRatio)
        {
            return density * (internalEnergy - Math.Pow(speed, 2)/2) * (heatCapacityRatio - 1);
        }

        public static double GetNewSpeed(
            double oldSpeed,
            double oldPressure,
            double oldPressureNext,
            double oldPressurePrev,
            double oldDensity,
            double size,
            double deltaTime)
        {
            double oldPressureHalfNext = MathUtils.GetAverage(oldPressure, oldPressureNext);
            double oldPressureHalfPrev = MathUtils.GetAverage(oldPressure, oldPressurePrev);

            return oldSpeed -
                ((oldPressureHalfNext - oldPressureHalfPrev) * deltaTime) / (size * oldDensity);
        }

        public static double GetNewInternalEnergy(
            double oldInternalEnergy,
            double oldPressure,
            double oldPressureNext,
            double oldPressurePrev,
            double oldSpeed,
            double oldSpeedNext,
            double oldSpeedPrev,
            double oldDensity,
            double size,
            double deltaTime)
        {
            double oldPressureHalfNext = MathUtils.GetAverage(oldPressure, oldPressureNext);
            double oldPressureHalfPrev = MathUtils.GetAverage(oldPressure, oldPressurePrev);

            double oldSpeedHalfNext = MathUtils.GetAverage(oldSpeed, oldSpeedNext);
            double oldSpeedHalfPrev = MathUtils.GetAverage(oldSpeed, oldSpeedPrev);

            return oldInternalEnergy -
                   ((oldPressureHalfNext * oldSpeedHalfNext - oldPressureHalfPrev * oldSpeedHalfPrev) * deltaTime) / (size * oldDensity);
        }
    }
}
