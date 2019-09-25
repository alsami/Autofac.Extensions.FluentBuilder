using Autofac.Extensions.FluentBuilder.TestInfrastructure.Generics;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.Scoped;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.Singletones;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.Transient;

namespace Autofac.Extensions.FluentBuilder.TestInfrastructure.Modules
{
    public class MultiItemModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Singleton>()
                .As<ISingleton>()
                .SingleInstance();

            builder.RegisterType<Transient.Transient>()
                .As<ITransient>()
                .InstancePerDependency();

            builder.RegisterType<Scoped.Scoped>()
                .As<IScoped>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GenericTypeImplementationBase<>))
                .As(typeof(IGenericTypeInterface<>));
        }
    }
}