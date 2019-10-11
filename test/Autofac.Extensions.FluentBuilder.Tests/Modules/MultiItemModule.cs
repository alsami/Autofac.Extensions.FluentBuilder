using Autofac.Extensions.FluentBuilder.Tests.Generics;
using Autofac.Extensions.FluentBuilder.Tests.Scoped;
using Autofac.Extensions.FluentBuilder.Tests.Singletones;
using Autofac.Extensions.FluentBuilder.Tests.Transient;

namespace Autofac.Extensions.FluentBuilder.Tests.Modules
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