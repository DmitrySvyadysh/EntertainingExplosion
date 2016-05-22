using System.Collections.Generic;
using OxyPlot;

namespace EntertainingExplosion.Models
{
    public class PlotModel
    {
        public string PlotName { get; set; }

        public string AbscissaName { get; set; }

        public string OrdinateName { get; set; }

        public IList<DataPoint> Points { get; set; }
    }
}
