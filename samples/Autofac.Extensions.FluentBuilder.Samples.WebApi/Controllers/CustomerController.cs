using Autofac.Extensions.FluentBuilder.Samples.Shared.ClosingTypes;
using Autofac.Extensions.FluentBuilder.Samples.Shared.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Autofac.Extensions.FluentBuilder.Samples.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ClassThatContainsConfiguration classThatContainsConfiguration;
        private readonly ICustomerRepository customerRepository;

        public CustomerController(ClassThatContainsConfiguration classThatContainsConfiguration, ICustomerRepository customerRepository)
        {
            this.classThatContainsConfiguration = classThatContainsConfiguration;
            this.customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult ReturnInstanceTypes()
        {
            return new ObjectResult(new
            {
                Repository = this.customerRepository.GetType().Name,
                Configuration = this.classThatContainsConfiguration.GetType().Name
            });
        }
    }
}