using Autofac;
using EntertainingExplosion.InitialData;

namespace EntertainingExplosion
{
    internal class EntertainingExplosionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PrimitiveInitialProcessCreator>()
                .AsImplementedInterfaces();
        }
    }
}
