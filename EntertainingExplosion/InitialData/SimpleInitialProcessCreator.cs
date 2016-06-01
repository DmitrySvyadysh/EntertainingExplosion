using System.Collections.Generic;
using EntertainingExplosion.Core.Models;

namespace EntertainingExplosion.InitialData
{
    public class SimpleInitialProcessCreator : IInitialProcessCreator
    {
        private const double MolarMass = 0.029;
        private const double HeatCapacityRatio = 1.4;
        private const double Size = 1;

        private const int ExplosionCellsCount = 10;
        private const double ExplosionCellTemperature = 10000;
        private const double ExplosionCellDensity = 1.29;

        private const int UsualCellsCount = 80;
        private const double UsualCellTemperature = 300;
        private const double UsualCellDensity = 1.29;

        private const double DeltaTime = 0.000001;
        private const int TicksCount = 200000;

        public InitialProcess CreateInitialProcess()
        {
            return new InitialProcess
            {
                InitialGrid = CreateInitialGrid(),
                DeltaTime = DeltaTime,
                TicksCount = TicksCount
            };
        }

        public InitialGrid CreateInitialGrid()
        {
            return new InitialGrid
            {
                Cells = CreateInitialCells(),
                ExternalCell = CreateExternalCell()
            };
        }

        private List<InitialCell> CreateInitialCells()
        {
            var initialCells = new List<InitialCell>(ExplosionCellsCount + UsualCellsCount);
            initialCells.AddRange(CreateExplosionInitialCells());
            initialCells.AddRange(CreateUsualInitialCells());

            return initialCells;
        }

        private List<InitialCell> CreateExplosionInitialCells()
        {
            var explosionInitialCells = new List<InitialCell>(ExplosionCellsCount);
            for (int i = 0; i < ExplosionCellsCount; i++)
            {
                explosionInitialCells.Add(CreateInitialCell(ExplosionCellTemperature, ExplosionCellDensity));
            }

            return explosionInitialCells;
        }

        private List<InitialCell> CreateUsualInitialCells()
        {
            var usualInitialCells = new List<InitialCell>(UsualCellsCount);
            for (int i = 0; i < UsualCellsCount; i++)
            {
                usualInitialCells.Add(CreateInitialCell(UsualCellTemperature, UsualCellDensity));
            }

            return usualInitialCells;
        }

        private InitialCell CreateExternalCell()
        {
            return CreateInitialCell(UsualCellTemperature, UsualCellDensity);
        }

        private InitialCell CreateInitialCell(double temperature, double density)
        {
            return new InitialCell
            {
                Density = density,
                MolarMass = MolarMass,
                HeatCapacityRatio = HeatCapacityRatio,
                Size = Size,
                Temperature = temperature
            };
        }
    }
}
