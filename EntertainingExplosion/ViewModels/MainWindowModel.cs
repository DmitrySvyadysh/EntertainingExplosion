using System;
using System.Windows.Input;
using EntertainingExplosion.Core.Engine;
using EntertainingExplosion.Core.Models;
using EntertainingExplosion.Helpers;
using EntertainingExplosion.InitialData;
using EntertainingExplosion.Models;
using EntertainingExplosion.Resources;
using EntertainingExplosion.UserInterface;

namespace EntertainingExplosion.ViewModels
{
    public class MainWindowModel : BaseViewModel
    {
        private PlotModel temperaturePlotModel;
        private PlotModel densityPlotModel;
        private PlotModel pressurePlotModel;
        private PlotModel speedPlotModel;
        private PlotModel internalEnergyPlotModel;
        private PlotModel izlucheniePlotModel;
        
        private ICommand prevTickCommand;
        private ICommand nextTickCommand;
        private int currentTick;
        private Process process;

        public MainWindowModel()
        {
            CalculateProcess();
            UpdatePlotModels();
        }

        public PlotModel TemperaturePlotModel
        {
            get { return temperaturePlotModel; }
            set
            {
                temperaturePlotModel = value;
                OnPropertyChanged();
            }
        }

        public PlotModel DensityPlotModel
        {
            get { return densityPlotModel; }
            set
            {
                densityPlotModel = value;
                OnPropertyChanged();
            }
        }

        public PlotModel PressurePlotModel
        {
            get { return pressurePlotModel; }
            set
            {
                pressurePlotModel = value;
                OnPropertyChanged();
            }
        }

        public PlotModel SpeedPlotModel
        {
            get { return speedPlotModel; }
            set
            {
                speedPlotModel = value;
                OnPropertyChanged();
            }
        }

        public PlotModel InternalEnergyPlotModel
        {
            get { return internalEnergyPlotModel; }
            set
            {
                internalEnergyPlotModel = value;
                OnPropertyChanged();
            }
        }

        public PlotModel IzlucheniePlotModel
        {
            get { return izlucheniePlotModel; }
            set
            {
                izlucheniePlotModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand PrevTickCommand
        {
            get
            {
                return prevTickCommand ?? (prevTickCommand = new CommandHandler(GoPrevTick));
            }
        }

        public ICommand NextTickCommand
        {
            get
            {
                return nextTickCommand ?? (nextTickCommand = new CommandHandler(GoNextTick));
            }
        }

        public string MolarMassInfo
        {
            get { return String.Format(Strings.MolarMassInfoFormat, process.Grids[0].Cells[0].MolarMass); }
        }

        public string HeatCapacityRatioInfo
        {
            get { return String.Format(Strings.HeatCapacityRatioInfoFormat, process.Grids[0].Cells[0].HeatCapacityRatio); }
        }

        public string SizeInfo
        {
            get { return String.Format(Strings.SizeInfoFormat, process.Grids[0].Cells[0].Size); }
        }

        public string DeltaTimeInfo
        {
            get { return String.Format(Strings.DeltaTimeInfoFormat, process.DeltaTime); }
        }

        public string TicksCountInfo
        {
            get { return String.Format(Strings.TicksCountInfoFormat, TicksCount); }
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
                UpdatePlotModels();
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

        private void UpdatePlotModels()
        {
            if (process == null || process.Grids.Count - 1 < currentTick)
            {
                return;
            }

            var grid = process.Grids[currentTick];
            UpdatePlotModels(grid);
        }

        private void UpdatePlotModels(Grid grid)
        {
            TemperaturePlotModel = PlotHelper.CreateTemperaturePlot(grid);
            DensityPlotModel = PlotHelper.CreateDensityPlot(grid);
            PressurePlotModel = PlotHelper.CreatePressurePlot(grid);
            SpeedPlotModel = PlotHelper.CreateSpeedPlot(grid);
            InternalEnergyPlotModel = PlotHelper.CreateEnergyPlot(grid);
            IzlucheniePlotModel = PlotHelper.CreateIzlucheniePlot(grid);
        }

        private void GoPrevTick()
        {
            if (CurrentTick > 0)
            {
                CurrentTick--;
            }
        }

        private void GoNextTick()
        {
            if (CurrentTick < process.Grids.Count - 1)
            {
                CurrentTick++;
            }
        }
    }
}
