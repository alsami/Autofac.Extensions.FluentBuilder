using System;
using Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces;

namespace Autofac.Extensions.FluentBuilder.Samples.Shared
{
    public class Customer : IGeneric
    {
        public Guid Id { get; }

        public Customer(Guid id)
        {
            this.Id = id;
        }
    }
}