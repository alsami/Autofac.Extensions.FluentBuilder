using System;
using System.Linq;
using System.Reflection;
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

        public AutofacFluentBuilder AddClosedTypeAsSingleton(Type closedType, params Assembly[] assemblies)
        {
            if (closedType == null)
            {
                throw new ArgumentNullException(nameof(closedType));
            }

            if (assemblies == null || !assemblies.Any())
            {
                throw new ArgumentNullException(nameof(assemblies), $"You must provide assemblies in order to register closing types for type { closedType.Name }");
            }

            this.builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(closedType)
                .AsImplementedInterfaces()
                .SingleInstance();

            return this;
        }
        
        public AutofacFluentBuilder AddClosedTypeAsTransient(Type closedType, params Assembly[] assemblies)
        {
            if (closedType == null)
            {
                throw new ArgumentNullException(nameof(closedType));
            }

            if (assemblies == null || !assemblies.Any())
            {
                throw new ArgumentNullException(nameof(assemblies), $"You must provide assemblies in order to register closing types for type { closedType.Name }");
            }

            this.builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(closedType)
                .AsImplementedInterfaces()
                .InstancePerDependency();

            return this;
        }
        
        public AutofacFluentBuilder AddClosedTypeAsScoped(Type closedType, params Assembly[] assemblies)
        {
            if (closedType == null)
            {
                throw new ArgumentNullException(nameof(closedType));
            }

            if (assemblies == null || !assemblies.Any())
            {
                throw new ArgumentNullException(nameof(assemblies), $"You must provide assemblies in order to register closing types for type { closedType.Name }");
            }

            this.builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(closedType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return this;
        }

        public AutofacFluentBuilder AddGenericAsSingleton(Type genericImplementation, Type genericInterface)
        {
            if (genericImplementation == null || !genericImplementation.IsGenericType)
            {
                throw new ArgumentException(nameof(genericImplementation));
            }
            
            if (genericInterface == null || !genericInterface.IsGenericType)
            {
                throw new ArgumentNullException(nameof(genericInterface));
            }

            if (!genericInterface.IsAssignableFrom(genericImplementation))
            {
                throw new ArgumentException(nameof(genericImplementation), $"The given type { genericImplementation.Name } is not assignable to { genericInterface }");
            }

            this.builder.RegisterGeneric(genericImplementation)
                .As(genericInterface)
                .SingleInstance();

            return this;
        }
        
        public AutofacFluentBuilder AddGenericAsTransient(Type genericImplementation, Type genericInterface)
        {
            if (genericImplementation == null || !genericImplementation.IsGenericType)
            {
                throw new ArgumentException(nameof(genericImplementation));
            }
            
            if (genericInterface == null || !genericInterface.IsGenericType)
            {
                throw new ArgumentNullException(nameof(genericInterface));
            }

            if (!genericInterface.IsAssignableFrom(genericImplementation))
            {
                throw new ArgumentException(nameof(genericImplementation), $"The given type { genericImplementation.Name } is not assignable to { genericInterface }");
            }

            this.builder.RegisterGeneric(genericImplementation)
                .As(genericInterface)
                .InstancePerDependency();

            return this;
        }
        
        public AutofacFluentBuilder AddGenericAsScoped(Type genericImplementation, Type genericInterface)
        {
            if (genericImplementation == null || !genericImplementation.IsGenericType)
            {
                throw new ArgumentException(nameof(genericImplementation));
            }
            
            if (genericInterface == null || !genericInterface.IsGenericType)
            {
                throw new ArgumentNullException(nameof(genericInterface));
            }

            if (!genericInterface.IsAssignableFrom(genericImplementation))
            {
                throw new ArgumentException(nameof(genericImplementation), $"The given type { genericImplementation.Name } is not assignable to { genericInterface }");
            }

            this.builder.RegisterGeneric(genericImplementation)
                .As(genericInterface)
                .InstancePerLifetimeScope();

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