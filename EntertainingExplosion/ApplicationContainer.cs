using Autofac;

namespace EntertainingExplosion
{
    public static class ApplicationContainer
    {
        public static IContainer Container;

        public static void BuildContainer(ContainerBuilder containerBuilder)
        {
            Container = containerBuilder.Build();
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
