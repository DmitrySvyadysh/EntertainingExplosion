using System.Collections.Generic;
using EntertainingExplosion.Core.Engine;
using EntertainingExplosion.Core.Models;
using EntertainingExplosion.Helpers;
using EntertainingExplosion.InitialData;
using EntertainingExplosion.Resources;
using OxyPlot;

namespace EntertainingExplosion.ViewModels
{
    public class MainWindowModel : BaseViewModel
    {
        private int currentTick;
        private Process process;

        public MainWindowModel()
        {
            CalculateProcess();
        }

        public string PlotTitle
        {
            get { return Strings.TemperaturePlotTitle; }
        }

        public string XAxisName
        {
            get { return Strings.Radius; }
        }

        public string YAxisName
        {
            get { return Strings.Temperature; }
        }

        public IList<DataPoint> Points
        {
            get; 
            private set;
        }

        public int CurrentTick
        {
            get { return currentTick; }
            set
            {
                if (value < 0 || value > TicksCount - 1)
                {
                    return;
                }

                currentTick = value;
                UpdatePoints();
                OnPropertyChanged();
            }
        }

        public int TicksCount
        {
            get { return process.Grids.Count; }
        }

        private void CalculateProcess()
        {
            var initialProcess = ApplicationContainer.Resolve<IInitialProcessCreator>().CreateInitialProcess();
            var processCreator = new ProcessCreator(initialProcess);
            process = processCreator.CreateProcess();
        }

        private void UpdatePoints()
        {
            if (process == null || process.Grids.Count - 1 < currentTick)
            {
                return;
            }

            var grid = process.Grids[currentTick];
            Points = OxyPlotHelper.CreatePoints(grid);

            OnPropertyChanged("Points");
        }
    }

    
}
