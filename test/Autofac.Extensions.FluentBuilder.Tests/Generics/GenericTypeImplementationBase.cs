namespace Autofac.Extensions.FluentBuilder.Tests.Generics
{
    public class GenericTypeImplementationBase<TGenericType> : IGenericTypeInterface<TGenericType> where TGenericType : class, IGenericType
    {
        
    }
}