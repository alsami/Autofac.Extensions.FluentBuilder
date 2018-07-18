using System;
using System.Linq;

namespace Autofac.Extensions.FluentBuilder
{
    internal static class TypeExtensions
    {
        public static bool ClosesType(this Type type, Type genericInterfaceType)
        {
            var anyBaseTypeImplementsGenericInterface = false;
            
            if (type.BaseType != null)
            {
                anyBaseTypeImplementsGenericInterface = type.BaseType.ClosesType(genericInterfaceType);
            }
            
            return type
                .GetInterfaces()
                .Where(@interface => @interface.IsGenericType)
                .Any(@interface => @interface.GetGenericTypeDefinition() == genericInterfaceType) || anyBaseTypeImplementsGenericInterface;
        }
    }
}