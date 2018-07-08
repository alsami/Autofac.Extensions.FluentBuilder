using Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces;

namespace Autofac.Extensions.FluentBuilder.Samples.Shared.ClosingTypes
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        void AdditionalFunction(Customer customer);
    }
}