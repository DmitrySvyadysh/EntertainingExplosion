using System.Collections.Generic;
using System.Linq;
using EntertainingExplosion.Core.Models;
using EntertainingExplosion.Core.Utils;

namespace EntertainingExplosion.Core.Engine
{
    public static class GasDynRecounter
    {
        public static Grid CreateNewGrid(Grid oldGrid, double deltaTime)
        {
            return new Grid
            {
                Cells = CreateNewCells(oldGrid.Cells, oldGrid.ExternalCell, deltaTime),
                ExternalCell = CreateNewExternalCell(oldGrid.ExternalCell, oldGrid.Cells.Last(), deltaTime)
            };
        }

        public static List<Cell> CreateNewCells(List<Cell> oldCells, Cell oldExternalCell, double deltaTime)
        {
            var newCells = new List<Cell>(oldCells.Count);
            for (int i = 0; i < oldCells.Count; i++)
            {
                var oldCell = oldCells[i];
                var oldCellPrev = i > 0 ? oldCells[i - 1] : oldCell;
                var oldCellNext = i < oldCells.Count - 1 ? oldCells[i + 1] : oldExternalCell;

                var newCell = CreateNewCell(oldCell, oldCellPrev, oldCellNext, deltaTime);
                newCells.Add(newCell);
            }

            return newCells;
        } 

        public static Cell CreateNewCell(Cell oldCell, Cell oldCellPrev, Cell oldCellNext, double deltaTime)
        {
            var newCell = new Cell
            {
                Density = oldCell.Density,
                HeatCapacityRatio = oldCell.HeatCapacityRatio,
                MolarMass = oldCell.MolarMass,
                Size = oldCell.Size
            };

            newCell.Speed = GasDynUtils.GetNewSpeed(oldCell.Speed, oldCell.Pressure, oldCellNext.Pressure,
                oldCellPrev.Pressure, oldCell.Density, oldCell.Size, deltaTime);

            newCell.InternalEnergy = GasDynUtils.GetNewInternalEnergy(oldCell.InternalEnergy, oldCell.Pressure,
                oldCellNext.Pressure,oldCellPrev.Pressure, oldCell.Speed, oldCellNext.Speed, 
                oldCellPrev.Speed, oldCell.Density, oldCell.Size, deltaTime);

            newCell.Pressure = GasDynUtils.GetPressure(newCell.Density, newCell.InternalEnergy, newCell.Speed,
                newCell.HeatCapacityRatio);

            newCell.Temperature = GasDynUtils.GetTemperature(newCell.MolarMass, newCell.Pressure, newCell.Density);

            return newCell;
        }

        public static Cell CreateNewExternalCell(Cell oldExternalCell, Cell oldCellPrev, double deltaTime)
        {
            var newExternalCell = new Cell
            {
                Density = oldExternalCell.Density,
                HeatCapacityRatio = oldExternalCell.HeatCapacityRatio,
                MolarMass = oldExternalCell.MolarMass,
                Size = oldExternalCell.Size,
                Pressure = oldExternalCell.Pressure,
                Temperature = oldExternalCell.Temperature,
                Speed = oldExternalCell.Speed
            };

            newExternalCell.InternalEnergy = GasDynUtils.GetNewInternalEnergy(oldExternalCell.InternalEnergy,
                oldExternalCell.Pressure, 0, oldCellPrev.Pressure, oldExternalCell.Speed, 0, 
                oldCellPrev.Speed, oldExternalCell.Density, oldExternalCell.Size, deltaTime);

            return newExternalCell;
        }
    }
}
