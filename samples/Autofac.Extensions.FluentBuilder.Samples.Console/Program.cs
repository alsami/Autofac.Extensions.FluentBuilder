using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extensions.FluentBuilder.Samples.Shared;
using Autofac.Extensions.FluentBuilder.Samples.Shared.ClosingTypes;
using Autofac.Extensions.FluentBuilder.Samples.Shared.Implementations;
using Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Autofac.Extensions.FluentBuilder.Samples.Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHost(args);

            await host.StartAsync();

            var container = host.Services.GetAutofacRoot();

            var genericCustomerRepository = container.Resolve<IGenericRepository<Customer>>();

            System.Console.WriteLine(genericCustomerRepository.GetType().Name);

            var customerRepository = container.Resolve<ICustomerRepository>();

            System.Console.WriteLine(customerRepository.GetType().Name);

            var writer = container.Resolve<IConsoleWriter>();

            System.Console.WriteLine(writer.GetType().Name);

            System.Console.ReadKey();

            await host.StopAsync();
        }

        private static IHost CreateHost(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(ConfigureContainer))
                .Build();

        private static void ConfigureContainer(ContainerBuilder builder)
        {
            new AutofacFluentBuilder(builder)
                .RegisterTypeAsSingleton<ConsoleWriter, IConsoleWriter>()
                .AddGenericAsScoped(typeof(GenericRepository<>), typeof(IGenericRepository<>))
                .AddClosedTypeAsScoped(typeof(IGenericRepository<>), new[] {typeof(IGenericRepository<>).Assembly});
        }
    }
}