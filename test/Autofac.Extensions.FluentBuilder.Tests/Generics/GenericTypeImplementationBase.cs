namespace Autofac.Extensions.FluentBuilder.TestInfrastructure.Generics
{
    public class GenericTypeImplementationBase<TGenericType> : IGenericTypeInterface<TGenericType> where TGenericType : class, IGenericType
    {
        
    }
}