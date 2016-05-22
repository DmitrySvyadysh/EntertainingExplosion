using System.Windows;
using System.Windows.Controls;
using EntertainingExplosion.Models;
using EntertainingExplosion.ViewModels;

namespace EntertainingExplosion.Views
{
    public partial class PlotView : UserControl
    {
        public static readonly DependencyProperty PlotModelProperty = DependencyProperty.Register(
            "PlotModel", typeof(PlotModel), typeof(PlotView),
            new PropertyMetadata(default(PlotModel), OnModelChanged));

        public PlotView()
        {
            InitializeComponent();
        }

        public PlotModel PlotModel
        {
            get { return (PlotModel)GetValue(PlotModelProperty); }
            set { SetValue(PlotModelProperty, value); }
        }

        private static void OnModelChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var plotView = (PlotView)obj;
            var plotViewModel = (PlotViewModel)plotView.DataContext;
            plotViewModel.Model = (PlotModel)args.NewValue;
        }
    }
}
