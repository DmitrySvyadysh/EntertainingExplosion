using System;
using System.Collections.Generic;
using System.Linq;
using EntertainingExplosion.Core.Models;
using EntertainingExplosion.Resources;
using OxyPlot;
using PlotModel = EntertainingExplosion.Models.PlotModel;

namespace EntertainingExplosion.Helpers
{
    public static class PlotHelper
    {
        public static PlotModel CreateTemperaturePlot(Grid grid)
        {
            return new PlotModel
            {
                PlotName = Strings.TemperaturePlotTitle,
                AbscissaName = Strings.Radius,
                OrdinateName = Strings.Temperature,
                Points = CreateDataPoints(grid, c => c.Temperature)
            };
        }

        public static PlotModel CreateDensityPlot(Grid grid)
        {
            return new PlotModel
            {
                PlotName = Strings.DensityPlotTitle,
                AbscissaName = Strings.Radius,
                OrdinateName = Strings.Density,
                Points = CreateDataPoints(grid, c => c.Density)
            };
        }

        public static PlotModel CreatePressurePlot(Grid grid)
        {
            return new PlotModel
            {
                PlotName = Strings.PressurePlotTitle,
                AbscissaName = Strings.Radius,
                OrdinateName = Strings.Pressure,
                Points = CreateDataPoints(grid, c => c.Pressure)
            };
        }

        public static PlotModel CreateSpeedPlot(Grid grid)
        {
            return new PlotModel
            {
                PlotName = Strings.SpeedPlotTitle,
                AbscissaName = Strings.Radius,
                OrdinateName = Strings.Speed,
                Points = CreateDataPoints(grid, c => c.Speed)
            };
        }

        public static PlotModel CreateEnergyPlot(Grid grid)
        {
            return new PlotModel
            {
                PlotName = Strings.EnergyPlotTitle,
                AbscissaName = Strings.Radius,
                OrdinateName = Strings.Energy,
                Points = CreateDataPoints(grid, c => c.Energy)
            };
        }

        public static PlotModel CreateIzlucheniePlot(Grid grid)
        {
            return new PlotModel
            {
                PlotName = Strings.IzlucheniePlotTitle,
                AbscissaName = Strings.Radius,
                OrdinateName = Strings.Izluchenie,
                Points = CreateDataPoints(grid, c => c.Izluchenie)
            };
        }

        private static List<DataPoint> CreateDataPoints(Grid grid, Func<Cell, double> predicate)
        {
            var newPoints = new List<DataPoint>(grid.Cells.Count);
            newPoints.AddRange(grid.Cells.Select((cell, j) => 
                new DataPoint(cell.Size*j, predicate.Invoke(cell))));

            return newPoints;
        }
    }
}
