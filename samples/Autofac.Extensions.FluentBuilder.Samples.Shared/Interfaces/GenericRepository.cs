namespace Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces
{
    public interface IGenericRepository<T> where T : class, IGeneric
    {
        void Add(T generic);
    }
}