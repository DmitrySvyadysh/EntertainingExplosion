using System.Collections.Generic;
using EntertainingExplosion.Core.Models;
using OxyPlot;

namespace EntertainingExplosion.Helpers
{
    public static class OxyPlotHelper
    {
        public static List<DataPoint> CreatePoints(Grid grid)
        {
            var newPoints = new List<DataPoint>(grid.Cells.Count);
            for (int j = 0; j < grid.Cells.Count; j++)
            {
                var cell = grid.Cells[j];
                newPoints.Add(new DataPoint(cell.Size * j, cell.Temperature));
            }

            return newPoints;
        }
    }
}
