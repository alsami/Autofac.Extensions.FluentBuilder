using System;
using System.Collections.Generic;
using Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces;

namespace Autofac.Extensions.FluentBuilder.Samples.Shared.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IGeneric
    {
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly Dictionary<Guid, T> genericsById = new Dictionary<Guid, T>();
        
        public void Add(T generic)
        {
            this.genericsById.Add(generic.Id, generic);
        }
    }
}