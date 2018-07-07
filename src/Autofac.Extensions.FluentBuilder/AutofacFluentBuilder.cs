using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Autofac.Extensions.FluentBuilder
{
    public class AutofacFluentBuilder
    {
        private readonly ContainerBuilder builder;

        public AutofacFluentBuilder(ContainerBuilder builder = null)
        {
            this.builder = builder ?? new ContainerBuilder();
        }

        public AutofacFluentBuilder RegisterTypeAsSingleton<TImplementation, TInterface>()
            where TImplementation : class, TInterface
        {
            this.builder.RegisterType<TImplementation>()
                .As<TInterface>()
                .SingleInstance();

            return this;
        }

        public AutofacFluentBuilder RegisterTypeAsSingleton<TImplementation>() where TImplementation : class
        {
            this.builder.RegisterType<TImplementation>()
                .SingleInstance();

            return this;
        }
        
        public AutofacFluentBuilder RegisterTypeAsTransient<TImplementation, TInterface>()
            where TImplementation : class, TInterface
        {
            this.builder.RegisterType<TImplementation>()
                .As<TInterface>()
                .InstancePerDependency();

            return this;
        }

        public AutofacFluentBuilder RegisterTypeAsTransient<TImplementation>() where TImplementation : class
        {
            this.builder.RegisterType<TImplementation>()
                .InstancePerDependency();

            return this;
        }
        
        public AutofacFluentBuilder RegisterTypeAsScoped<TImplementation, TInterface>()
            where TImplementation : class, TInterface
        {
            this.builder.RegisterType<TImplementation>()
                .As<TInterface>()
                .InstancePerLifetimeScope();

            return this;
        }

        public AutofacFluentBuilder RegisterTypeAsScoped<TImplementation>() where TImplementation : class
        {
            this.builder.RegisterType<TImplementation>()
                .InstancePerLifetimeScope();

            return this;
        }

        public AutofacFluentBuilder RegisterInstance<TType>(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            
            if (!type.IsAssignableFrom(typeof(TType)))
            {
                throw new ArgumentException(nameof(type), $"The given type { type.Name } is not assignable to { (typeof(TType).Name) }");
            }

            this.builder.RegisterType(type)
                .As<TType>()
                .SingleInstance();

            return this;
        }

        public AutofacFluentBuilder ApplyModule<TModule>() where TModule : Module, new()
        {
            this.builder.RegisterModule<TModule>();

            return this;
        }

        public AutofacFluentBuilder ApplyModule(Module module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }
            
            this.builder.RegisterModule(module);

            return this;
        }

        public IContainer Build(IServiceCollection serviceCollection = null)
        {
            this.builder.Populate(serviceCollection ?? new ServiceCollection());

            return this.builder.Build();
        }
    }
}