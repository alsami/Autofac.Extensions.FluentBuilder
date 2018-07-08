using Autofac.Extensions.FluentBuilder.Samples.Shared.Implementations;

namespace Autofac.Extensions.FluentBuilder.Samples.Shared.ClosingTypes
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public void AdditionalFunction(Customer customer)
        {
            
        }
    }
}