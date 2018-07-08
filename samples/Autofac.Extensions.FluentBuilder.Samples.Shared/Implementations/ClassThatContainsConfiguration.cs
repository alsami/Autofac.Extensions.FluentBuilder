using Microsoft.Extensions.Options;

namespace Autofac.Extensions.FluentBuilder.Samples.Shared.Implementations
{
    public class ClassThatContainsConfiguration
    {
        private readonly IOptions<SomeConfigurationClass> options;

        public ClassThatContainsConfiguration(IOptions<SomeConfigurationClass> options)
        {
            this.options = options;
        }
    }
}