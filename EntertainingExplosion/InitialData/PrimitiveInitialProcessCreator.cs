using System.Collections.Generic;
using EntertainingExplosion.Core.Models;

namespace EntertainingExplosion.InitialData
{
    public class PrimitiveInitialProcessCreator : IInitialProcessCreator
    {
        private const double Density = 1.2;
        private const double MolarMass = 0.029;
        private const double HeatCapacityRatio = 1.4;
        private const double Size = 0.01;

        private const int ExplosionCellsCount = 100;
        private const int UsualCellsCount = 900;
        private const double ExplosionCellTemperature = 1000;
        private const double UsualCellTemperature = 20;

        private const double DeltaTime = 0.01;
        private const int TicksCount = 1000;

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
                explosionInitialCells.Add(CreateInitialCell(ExplosionCellTemperature));
            }

            return explosionInitialCells;
        }

        private List<InitialCell> CreateUsualInitialCells()
        {
            var usualInitialCells = new List<InitialCell>(UsualCellsCount);
            for (int i = 0; i < UsualCellsCount; i++)
            {
                usualInitialCells.Add(CreateInitialCell(UsualCellTemperature));
            }

            return usualInitialCells;
        }

        private InitialCell CreateExternalCell()
        {
            return CreateInitialCell(UsualCellTemperature);
        }

        private InitialCell CreateInitialCell(double temperature)
        {
            return new InitialCell
            {
                Density = Density,
                MolarMass = MolarMass,
                HeatCapacityRatio = HeatCapacityRatio,
                Size = Size,
                Temperature = temperature
            };
        }
    }
}
