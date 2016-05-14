using Autofac;
using EntertainingExplosion.ViewModels;
using EntertainingExplosion.Views;

namespace EntertainingExplosion
{
    internal class EntertainingExplosionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterWindows(builder);
            RegisterViewModels(builder);
        }

        private void RegisterWindows(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<BaseWindow>()
                .AsSelf();
        }

        private void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<BaseViewModel>()
                .AsSelf();
        }
    }
}
