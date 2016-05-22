using System;
using System.Collections.Generic;
using OxyPlot;
using PlotModel = EntertainingExplosion.Models.PlotModel;

namespace EntertainingExplosion.ViewModels
{
    public class PlotViewModel : BaseViewModel
    {
        private PlotModel model;

        public PlotModel Model
        {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged(String.Empty);
            }
        }

        public string PlotTitle
        {
            get { return Model != null ? Model.PlotName : null; }
        }

        public string AbscissaName
        {
            get { return Model != null ? Model.AbscissaName : null; }
        }

        public string OrdinateName
        {
            get { return Model != null ? Model.OrdinateName : null; }
        }

        public IList<DataPoint> Points
        {
            get { return Model != null ? Model.Points : null; }
        }
    }
}
