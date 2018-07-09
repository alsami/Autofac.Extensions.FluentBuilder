using System;
using System.Runtime.CompilerServices;
using Autofac.Extensions.FluentBuilder.Samples.Shared;
using Autofac.Extensions.FluentBuilder.Samples.Shared.ClosingTypes;
using Autofac.Extensions.FluentBuilder.Samples.Shared.Implementations;
using Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces;

namespace Autofac.Extensions.FluentBuilder.Samples.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new AutofacFluentBuilder()
                .RegisterTypeAsSingleton<ConsoleWriter, IConsoleWriter>()
                .AddGenericAsScoped(typeof(GenericRepository<>), typeof(IGenericRepository<>))
                .AddClosedTypeAsScoped(typeof(IGenericRepository<>), new[] {typeof(IGenericRepository<>).Assembly})
                .Build();

            var genericCustomerRepository = container.Resolve<IGenericRepository<Customer>>();
            
            System.Console.WriteLine(genericCustomerRepository.GetType().Name);

            var customerRepository = container.Resolve<ICustomerRepository>();
            
            System.Console.WriteLine(customerRepository.GetType().Name);

            var writer = container.Resolve<IConsoleWriter>();
            
            System.Console.WriteLine(writer.GetType().Name);

            System.Console.ReadKey();
        }
    }
}