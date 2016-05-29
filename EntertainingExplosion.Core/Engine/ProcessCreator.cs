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

        public void InitializeRadiation(double stepFi)
        {
            int count = process.Grids.Last().Cells.Count;
            double size = process.Grids.Last().Cells.First().Size;
            RadiationRecounter.array = new double[count];

            for (int i = 0; i < count; i++)
            {
                RadiationRecounter.array[i] = RadiationRecounter.Square(i, size);
            }
            RadiationGrid.InitRadiationGrid(count * size, size, stepFi);
        }

        public Process CreateProcess()
        {
            InitializeRadiation(0.1);
            
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
            RadiationRecounter.CreateNewGrid(prevGrid, deltaTime);
            return nextGasDynGrid;

            // TODO:

            /*var nextRadiationGrid = */
            //return GridHelper.GetAverageGrid(nextGasDynGrid, nextRadiationGrid);
        }
    }
}
