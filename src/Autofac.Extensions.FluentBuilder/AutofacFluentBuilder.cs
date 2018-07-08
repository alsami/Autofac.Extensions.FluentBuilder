using System;
using System.Linq;
using System.Reflection;
using Autofac.Core;
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

        public AutofacFluentBuilder RegisterInstance<TType>(object @object)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }
            
            if (!(@object is TType))
            {
                throw new ArgumentException(nameof(@object), $"The given type { @object.GetType().Name } is not assignable to { (typeof(TType).Name) }");
            }

            this.builder.RegisterInstance(@object)
                .As<TType>()
                .SingleInstance();

            return this;
        }

        public AutofacFluentBuilder AddClosedTypeAsSingleton(Type closedType, Assembly[] assemblies, params ResolvedParameter[] resolvedParameters)
        {
            if (closedType == null)
            {
                throw new ArgumentNullException(nameof(closedType));
            }

            if (assemblies == null || !assemblies.Any())
            {
                throw new ArgumentNullException(nameof(assemblies), $"You must provide assemblies in order to register closing types for type { closedType.Name }");
            }

            var registrationBuilder = this.builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(closedType)
                .AsImplementedInterfaces()
                .SingleInstance();

            foreach (var resolvedParamter in resolvedParameters)
            {
                registrationBuilder.WithParameter(resolvedParamter)
                    .SingleInstance();
            }

            return this;
        }
        
        public AutofacFluentBuilder AddClosedTypeAsTransient(Type closedType, Assembly[] assemblies, params ResolvedParameter[] resolvedParameters)
        {
            if (closedType == null)
            {
                throw new ArgumentNullException(nameof(closedType));
            }

            if (assemblies == null || !assemblies.Any())
            {
                throw new ArgumentNullException(nameof(assemblies), $"You must provide assemblies in order to register closing types for type { closedType.Name }");
            }

            var registrationBuilder = this.builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(closedType)
                .AsImplementedInterfaces()
                .InstancePerDependency();
            
            foreach (var resolvedParamter in resolvedParameters)
            {
                registrationBuilder.WithParameter(resolvedParamter)
                    .InstancePerDependency();
            }

            return this;
        }
        
        public AutofacFluentBuilder AddClosedTypeAsScoped(Type closedType, Assembly[] assemblies, params ResolvedParameter[] resolvedParameters)
        {
            if (closedType == null)
            {
                throw new ArgumentNullException(nameof(closedType));
            }

            if (assemblies == null || !assemblies.Any())
            {
                throw new ArgumentNullException(nameof(assemblies), $"You must provide assemblies in order to register closing types for type { closedType.Name }");
            }

            var registrationBuilder = this.builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(closedType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            foreach (var resolvedParamter in resolvedParameters)
            {
                registrationBuilder.WithParameter(resolvedParamter)
                    .InstancePerLifetimeScope();
            }

            return this;
        }

        public AutofacFluentBuilder AddGenericAsSingleton(Type genericImplementationType, Type genericInterfaceType)
        {
            if (genericImplementationType == null || !genericImplementationType.IsGenericType)
            {
                throw new ArgumentException(nameof(genericImplementationType));
            }
            
            if (genericInterfaceType == null || !genericInterfaceType.IsGenericType)
            {
                throw new ArgumentNullException(nameof(genericInterfaceType));
            }
            
            if (!genericImplementationType.ClosesType(genericInterfaceType))
            {
                throw new ArgumentException(nameof(genericImplementationType), $"The given type { genericImplementationType.Name } is not assignable to { genericInterfaceType }");
            }

            this.builder.RegisterGeneric(genericImplementationType)
                .As(genericInterfaceType)
                .SingleInstance();

            return this;
        }
        
        public AutofacFluentBuilder AddGenericAsTransient(Type genericImplementationType, Type genericInterfaceType)
        {
            if (genericImplementationType == null || !genericImplementationType.IsGenericType)
            {
                throw new ArgumentException(nameof(genericImplementationType));
            }
            
            if (genericInterfaceType == null || !genericInterfaceType.IsGenericType)
            {
                throw new ArgumentNullException(nameof(genericInterfaceType));
            }

            if (!genericImplementationType.ClosesType(genericInterfaceType))
            {
                throw new ArgumentException(nameof(genericImplementationType), $"The given type { genericImplementationType.Name } is not assignable to { genericInterfaceType }");
            }

            this.builder.RegisterGeneric(genericImplementationType)
                .As(genericInterfaceType)
                .InstancePerDependency();

            return this;
        }
        
        public AutofacFluentBuilder AddGenericAsScoped(Type genericImplementationType, Type genericInterfaceType)
        {
            if (genericImplementationType == null || !genericImplementationType.IsGenericType)
            {
                throw new ArgumentException(nameof(genericImplementationType));
            }
            
            if (genericInterfaceType == null || !genericInterfaceType.IsGenericType)
            {
                throw new ArgumentNullException(nameof(genericInterfaceType));
            }

            if (!genericImplementationType.ClosesType(genericInterfaceType))
            {
                throw new ArgumentException(nameof(genericImplementationType), $"The given type { genericImplementationType.Name } is not assignable to { genericInterfaceType }");
            }

            this.builder.RegisterGeneric(genericImplementationType)
                .As(genericInterfaceType)
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