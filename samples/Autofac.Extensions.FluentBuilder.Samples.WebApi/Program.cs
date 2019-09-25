using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Autofac.Extensions.FluentBuilder.Samples.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHost(args);

            await host.RunAsync();
        }

        public static IHost CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .Build();
    }
}