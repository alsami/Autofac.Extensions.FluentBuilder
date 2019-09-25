using System;
using System.Collections.Generic;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.ClosedTypes;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.Generics;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.Modules;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.Scoped;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.Singletones;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.Strategies;
using Autofac.Extensions.FluentBuilder.TestInfrastructure.Transient;
using Xunit;

namespace Autofac.Extensions.FluentBuilder.IntegrationTests
{
    public class AutofacFluentBuilderTests : IDisposable
    {
        private readonly AutofacFluentBuilder fluentBuilder;
        private IContainer container;

        public AutofacFluentBuilderTests()
        {
            this.fluentBuilder = new AutofacFluentBuilder();
        }

        [Fact]
        public void AutofacFluent_Builder_RegisterResolver_ExpectInstancePerScope()
        {
            this.container = this.fluentBuilder
                .RegisterTypeAsTransient<GoogleAuthenticationStrategy, IGoogleAuthenticationStrategy>()
                .RegisterResolver<GoogleAuthenticationStrategy, IGoogleAuthenticationStrategy>(_ => new GoogleAuthenticationStrategy())
                .Build();

            var googleAuthenticationStrategy = this.container.Resolve<IGoogleAuthenticationStrategy>();
            
            Assert.NotNull(googleAuthenticationStrategy);
        }
        
        [Fact]
        public void AutofacFluent_Builder_RegisterTypedResolver_ExpectInstancePerScope()
        {
            this.container = this.fluentBuilder
                .RegisterTypeAsTransient<GoogleAuthenticationStrategy, IGoogleAuthenticationStrategy>()
                .RegisterTypeAsTransient<FacebookAuthenticationStrategy, IFacebookAuthenticationStrategy>()
                .RegisterResolver(Resolver)
                .Build();


            Func<AuthenticationProvider, IAuthenticationStrategy> func;
            Func<AuthenticationProvider, IAuthenticationStrategy> func2;
            IGoogleAuthenticationStrategy googleAuth;
            IGoogleAuthenticationStrategy googleAuth2;
            IFacebookAuthenticationStrategy facebookAuth;
            IFacebookAuthenticationStrategy facebookAuth2;
            

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                func = scope.Resolve<Func<AuthenticationProvider, IAuthenticationStrategy>>();
                googleAuth = (IGoogleAuthenticationStrategy) func(AuthenticationProvider.Google);
                facebookAuth = (IFacebookAuthenticationStrategy) func(AuthenticationProvider.Facebook);
                
                using (var scope2 = this.GetLifetimeScope().BeginLifetimeScope())
                {
                    func2 = scope2.Resolve<Func<AuthenticationProvider, IAuthenticationStrategy>>();
                    googleAuth2 = (IGoogleAuthenticationStrategy) func2(AuthenticationProvider.Google);
                    facebookAuth2 = (IFacebookAuthenticationStrategy) func2(AuthenticationProvider.Facebook);
                }
            }
            
            Assert.NotEqual(func, func2);
            Assert.NotEqual(googleAuth, googleAuth2);
            Assert.NotEqual(facebookAuth, facebookAuth2);
        }
        
        [Fact]
        public void AutofacFluentBuilder_RegisterSinglentonAndResolve_ExpectSameInstances()
        {
            this.container = this.fluentBuilder
                .RegisterTypeAsSingleton<Singleton, ISingleton>()
                .RegisterTypeAsSingleton<Singleton>()
                .Build();

            Assert.True(this.container.IsRegistered<ISingleton>(), "ISingleton not registered!");
            Assert.True(this.container.IsRegistered<Singleton>(), "Singleton not registered!");

            ISingleton s1;
            
            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                s1 = scope.Resolve<ISingleton>();
            }
            
            Assert.NotNull(s1);

            ISingleton s2;

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                s2 = scope.Resolve<ISingleton>();
            }
            
            Assert.NotNull(s2);
            Assert.Equal(s1, s2);
            
            Singleton s3;
            
            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                s3 = scope.Resolve<Singleton>();
            }
            
            Assert.NotNull(s3);

            Singleton s4;

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                s4 = scope.Resolve<Singleton>();
            }
            
            Assert.NotNull(s4);
            Assert.Equal(s3, s4);
        }
        
        [Fact]
        public void AutofacFluentBuilder_RegisterTransientAndResolve_ExpectDifferentInstances()
        {
            this.container = this.fluentBuilder
                .RegisterTypeAsTransient<Transient, ITransient>()
                .RegisterTypeAsTransient<Transient>()
                .Build();

            Assert.True(this.container.IsRegistered<ITransient>(), "ITransient not registered!");
            Assert.True(this.container.IsRegistered<Transient>(), "Transient not registered!");

            ITransient t1;
            ITransient t2;
            
            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                t1 = scope.Resolve<ITransient>();
                t2 = scope.Resolve<ITransient>();
            }
            
            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.NotEqual(t1, t2);

            Transient s3;
            Transient s4;
            
            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                s3 = scope.Resolve<Transient>();
                s4 = scope.Resolve<Transient>();
            }
            
            Assert.NotNull(s3);
            Assert.NotNull(s4);
            Assert.NotEqual(s3, s4);
        }
        
        [Fact]
        public void AutofacFluentBuilder_RegisterScopedAndResolve_ExpectSameInstances()
        {
            this.container = this.fluentBuilder
                .RegisterTypeAsScoped<Scoped, IScoped>()
                .RegisterTypeAsScoped<Scoped>()
                .Build();

            Assert.True(this.container.IsRegistered<IScoped>(), "IScoped not registered!");
            Assert.True(this.container.IsRegistered<Scoped>(), "Scoped not registered!");
            
            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                var s1 = scope.Resolve<IScoped>();
                
                Assert.NotNull(s1);
                
                var s2 = scope.Resolve<IScoped>();
                Assert.NotNull(s2);
                Assert.Equal(s1, s2);
            }
            
            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                var s3 = scope.Resolve<Scoped>();
                var s4 = scope.Resolve<Scoped>();
                Assert.NotNull(s3);
                Assert.NotNull(s4);
                Assert.Equal(s3, s4);
            }
        }
        
        [Fact]
        public void AutofacFluentBuilder_RegisterInstance_ExpectSameInstances()
        {
            this.container = this.fluentBuilder
                .RegisterInstance<ISingleton>(new Singleton())
                .Build();

            Assert.True(this.container.IsRegistered<ISingleton>(), "ISingleton not registered!");
            
            ISingleton s1;
            
            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                s1 = scope.Resolve<ISingleton>();
            }
            
            Assert.NotNull(s1);

            ISingleton s2;

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                s2 = scope.Resolve<ISingleton>();
            }
            
            Assert.NotNull(s2);
            Assert.Equal(s1, s2);
        }
        
        [Fact]
        public void AutofacFluentBuilder_RegisterInstanceWithNullValue_ExpectException()
        {
            Assert.Throws<ArgumentNullException>(() => this.fluentBuilder
                .RegisterInstance<ISingleton>(null));
        }
        
        [Fact]
        public void AutofacFluentBuilder_RegisterInstanceNoneMatchingType_ExpectException()
        {
            Assert.Throws<ArgumentException>(() => this.fluentBuilder
                .RegisterInstance<ISingleton>(new Scoped()));
        }
        
        [Fact]
        public void AutofacFluentBuilder_AddGenericAsSingletonAndResolve_ExpectSameInstances()
        {
            this.container = this.fluentBuilder
                .AddGenericAsSingleton(typeof(GenericTypeImplementationBase<>), typeof(IGenericTypeInterface<>))
                .Build();
            
            Assert.True(this.container.IsRegistered(typeof(IGenericTypeInterface<GenericClass>)));

            IGenericTypeInterface<GenericClass> g1;
            IGenericTypeInterface<GenericClass> g2;

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                g1 = (IGenericTypeInterface<GenericClass>) scope.Resolve(typeof(IGenericTypeInterface<GenericClass>));
            }
            
            Assert.NotNull(g1);
            
            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                g2 = (IGenericTypeInterface<GenericClass>) scope.Resolve(typeof(IGenericTypeInterface<GenericClass>));
            }
            
            Assert.NotNull(g2);
            Assert.Equal(g1, g2);
        }
        
        [Fact]
        public void AutofacFluentBuilder_AddGenericAsTransientAndResolve_ExpectDifferentInstances()
        {
            this.container = this.fluentBuilder
                .AddGenericAsTransient(typeof(GenericTypeImplementationBase<>), typeof(IGenericTypeInterface<>))
                .Build();
            
            Assert.True(this.container.IsRegistered(typeof(IGenericTypeInterface<GenericClass>)));

            IGenericTypeInterface<GenericClass> g1;
            IGenericTypeInterface<GenericClass> g2;

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                g1 = (IGenericTypeInterface<GenericClass>) scope.Resolve(typeof(IGenericTypeInterface<GenericClass>));
            }
            
            Assert.NotNull(g1);
            
            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                g2 = (IGenericTypeInterface<GenericClass>) scope.Resolve(typeof(IGenericTypeInterface<GenericClass>));
            }
            
            Assert.NotNull(g2);
            Assert.NotEqual(g1, g2);
        }
        
        [Fact]
        public void AutofacFluentBuilder_AddGenericAsScopedAndResolve_ExpectSameInstances()
        {
            this.container = this.fluentBuilder
                .AddGenericAsScoped(typeof(GenericTypeImplementationBase<>), typeof(IGenericTypeInterface<>))
                .Build();
            
            Assert.True(this.container.IsRegistered(typeof(IGenericTypeInterface<GenericClass>)));

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                var g1 = (IGenericTypeInterface<GenericClass>) scope.Resolve(typeof(IGenericTypeInterface<GenericClass>));
                var g2 = (IGenericTypeInterface<GenericClass>) scope.Resolve(typeof(IGenericTypeInterface<GenericClass>));
                Assert.NotNull(g1);
                Assert.NotNull(g2);
                Assert.Equal(g1, g2);
                
                using (var scope2 = this.GetLifetimeScope().BeginLifetimeScope())
                {
                    var g3 = (IGenericTypeInterface<GenericClass>) scope2.Resolve(typeof(IGenericTypeInterface<GenericClass>));
                    Assert.NotNull(g3);
                    Assert.NotEqual(g1, g3);
                    Assert.NotEqual(g2, g3);
                }
            }
        }

        [Fact]
        public void AutofacFluentBuilder_AddClosedTypeAsSingletonAndResolve_ExpectSameInstances()
        {
            this.container = this.fluentBuilder.AddClosedTypeAsSingleton(typeof(IGenericTypeInterface<>),
                    new[] {typeof(IGenericTypeInterface<>).Assembly})
                .Build();
            
            Assert.True(this.container.IsRegistered(typeof(IGenericTypeInterface<GenericClass>)));
            Assert.True(this.container.IsRegistered<IGenericClosingType>());
            Assert.True(this.container.IsRegistered<GenericClosingType>());

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                var ct1 = scope.Resolve<IGenericClosingType>();
                Assert.NotNull(ct1);
                
                using (var scope2 = this.GetLifetimeScope().BeginLifetimeScope())
                {
                    var ct2 = scope2.Resolve<IGenericClosingType>();
                    Assert.NotNull(ct2);
                    Assert.Equal(ct1, ct2);
                }
            }
        }
        
        [Fact]
        public void AutofacFluentBuilder_AddClosedTypeAsTransientAndResolve_ExpectDifferentInstances()
        {
            this.container = this.fluentBuilder.AddClosedTypeAsTransient(typeof(IGenericTypeInterface<>),
                    new[] {typeof(IGenericTypeInterface<>).Assembly})
                .Build();
            
            Assert.True(this.container.IsRegistered(typeof(IGenericTypeInterface<GenericClass>)));
            Assert.True(this.container.IsRegistered<IGenericClosingType>());
            Assert.True(this.container.IsRegistered<GenericClosingType>());

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                var ct1 = scope.Resolve<IGenericClosingType>();
                Assert.NotNull(ct1);
                
                using (var scope2 = this.GetLifetimeScope().BeginLifetimeScope())
                {
                    var ct2 = scope2.Resolve<IGenericClosingType>();
                    Assert.NotNull(ct2);
                    Assert.NotEqual(ct1, ct2);
                }
            }
        }
        
        [Fact]
        public void AutofacFluentBuilder_AddClosedTypeAsScopedAndResolve_ExpectSameInstances()
        {
            this.container = this.fluentBuilder.AddClosedTypeAsScoped(typeof(IGenericTypeInterface<>),
                    new[] {typeof(IGenericTypeInterface<>).Assembly})
                .Build();
            
            Assert.True(this.container.IsRegistered(typeof(IGenericTypeInterface<GenericClass>)));
            Assert.True(this.container.IsRegistered<IGenericClosingType>());
            Assert.True(this.container.IsRegistered<GenericClosingType>());

            using (var scope = this.GetLifetimeScope().BeginLifetimeScope())
            {
                var ct1 = scope.Resolve<IGenericClosingType>();
                Assert.NotNull(ct1);
                
                var ct2 = scope.Resolve<IGenericClosingType>();
                Assert.NotNull(ct2);
                Assert.Equal(ct1, ct2);
            }
        }

        [Fact]
        public void AutofacFluentBuilder_ApplyModule_ExpectTypesToBeRegistered()
        {
            this.container = this.fluentBuilder.ApplyModule<MultiItemModule>()
                .Build();
            
            Assert.True(this.container.IsRegistered<ISingleton>());
            Assert.True(this.container.IsRegistered<ITransient>());
            Assert.True(this.container.IsRegistered<IScoped>());
            Assert.True(this.container.IsRegistered(typeof(IGenericTypeInterface<GenericClass>)));
        }

        public void Dispose()
        {
            this.container?.Dispose();
        }

        private ILifetimeScope GetLifetimeScope()
        {
            return this.container.Resolve<ILifetimeScope>();
        }
        
        private static IAuthenticationStrategy Resolver(IComponentContext ctx, IEnumerable<Autofac.Core.Parameter> @params)
        {
            var providers = @params.TypedAs<AuthenticationProvider>();

            switch (providers)
            {
                case AuthenticationProvider.Google:
                {
                    return ctx.Resolve<IGoogleAuthenticationStrategy>();
                }
                case AuthenticationProvider.Facebook:
                {
                    return ctx.Resolve<IFacebookAuthenticationStrategy>();
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}