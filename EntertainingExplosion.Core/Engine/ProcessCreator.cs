using System.Linq;
using EntertainingExplosion.Core.Models;

namespace EntertainingExplosion.Core.Engine
{
    public class ProcessCreator
    {
        private readonly Process process;
        private readonly int ticksCount;

        public ProcessCreator(InitialProcess initialProcess)
        {
            process = Initializer.InitializeProcess(initialProcess);
            ticksCount = initialProcess.TicksCount;
        }

        public Process CreateProcess()
        {
            for (int i = 0; i < ticksCount - 1; i++)
            {
                var lastGrid = process.Grids.Last();
                process.Grids.Add(GetNewGrid(lastGrid, process.DeltaTime));
            }

            return process;
        }

        private Grid GetNewGrid(Grid prevGrid, double deltaTime)
        {
            var nextGasDynGrid = GasDynRecounter.CreateNewGrid(prevGrid, deltaTime);
            return nextGasDynGrid;

            // TODO:
            //var nextRadiationGrid = RadiationRecounter.CreateNewGrid(prevGrid, deltaTime);
            //return GridHelper.GetAverageGrid(nextGasDynGrid, nextRadiationGrid);
        }
    }
}
