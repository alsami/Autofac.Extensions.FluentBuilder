using Autofac.Extensions.FluentBuilder.Samples.Shared.Implementations;
using Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Autofac.Extensions.FluentBuilder.Samples.WebApi
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    }
                );

            services.Configure<SomeConfigurationClass>(options =>
            {
                this.configuration.Bind(nameof(SomeConfigurationClass), options);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(builder => builder.MapControllers());
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            new AutofacFluentBuilder(builder)
                .RegisterTypeAsSingleton<ClassThatContainsConfiguration>()
                .AddClosedTypeAsScoped(typeof(IGenericRepository<>), new[] {typeof(IGenericRepository<>).Assembly});
        }
    }
}