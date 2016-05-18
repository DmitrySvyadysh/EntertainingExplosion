using System.Collections.Generic;
using System.Linq;
using EntertainingExplosion.Core.Models;
using EntertainingExplosion.Core.Utils;

namespace EntertainingExplosion.Core.Engine
{
    public static class Initializer
    {
        public static Process InitializeProcess(InitialProcess initialProcess)
        {
            return new Process
            {
                Grids = new List<Grid> { CreateFirstGrid(initialProcess.InitialGrid)},
                DeltaTime = initialProcess.DeltaTime
            };
        }

        private static Grid CreateFirstGrid(InitialGrid initialGrid)
        {
            return new Grid
            {
                Cells = initialGrid.Cells.Select(CreateCell).ToList(),
                ExternalCell = CreateCell(initialGrid.ExternalCell)
            };
        }

        private static Cell CreateCell(InitialCell initialCell)
        {
            var cell = new Cell
            {
                Density = initialCell.Density,
                MolarMass = initialCell.MolarMass,
                HeatCapacityRatio = initialCell.HeatCapacityRatio,
                Temperature = initialCell.Temperature,
                Size = initialCell.Size,
                Speed = 0
            };

            cell.Pressure = GasDynUtils.GetPressure(cell.Density, cell.MolarMass, cell.Temperature);
            cell.InternalEnergy = GasDynUtils.GetInternalEnergy(cell.Pressure, cell.Density, 
                cell.HeatCapacityRatio, cell.Speed);

            return cell;
        }
    }
}
