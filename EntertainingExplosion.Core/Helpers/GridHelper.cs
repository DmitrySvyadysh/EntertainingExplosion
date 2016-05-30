using System.Collections.Generic;
using EntertainingExplosion.Core.Models;
using EntertainingExplosion.Core.Utils;

namespace EntertainingExplosion.Core.Helpers
{
    public static class GridHelper
    {
        public static Grid GetAverageGrid(Grid firstGrid, Grid secondGrid)
        {
            var newGrid = new Grid
            {
                Cells = GetAverageCells(firstGrid.Cells, secondGrid.Cells),
                ExternalCell = GetAverageCell(firstGrid.ExternalCell, secondGrid.ExternalCell)
            };

            return newGrid;
        }

        private static List<Cell> GetAverageCells(List<Cell> firstCells, List<Cell> secondCells)
        {
            var newCells = new List<Cell>(firstCells.Count);
            for(var i = 0; i < firstCells.Count; i++)
            {
                newCells.Add(GetAverageCell(firstCells[i], secondCells[i]));
            }

            return newCells;
        }

        private static Cell GetAverageCell(Cell firstCell, Cell secondCell)
        {
            return new Cell
            {
                Density = MathUtils.GetAverage(firstCell.Density, secondCell.Density),
                MolarMass = MathUtils.GetAverage(firstCell.MolarMass, secondCell.MolarMass),
                HeatCapacityRatio = MathUtils.GetAverage(firstCell.HeatCapacityRatio, secondCell.HeatCapacityRatio),
                Pressure = MathUtils.GetAverage(firstCell.Pressure, secondCell.Pressure),
                Energy = MathUtils.GetAverage(firstCell.Energy, secondCell.Energy),
                Temperature = MathUtils.GetAverage(firstCell.Temperature, secondCell.Temperature),
                Size = MathUtils.GetAverage(firstCell.Size, secondCell.Size),
                Speed = MathUtils.GetAverage(firstCell.Speed, secondCell.Speed)
            };
        }
    }
}
