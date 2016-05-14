using System.Windows;
using Autofac;

namespace EntertainingExplosion
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializeAppContainer();
        }

        private void InitializeAppContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EntertainingExplosionModule>();
            ApplicationContainer.BuildContainer(builder);
        }
    }
}
