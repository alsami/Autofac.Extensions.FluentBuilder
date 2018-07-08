using System;
using Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces;

namespace Autofac.Extensions.FluentBuilder.Samples.Shared.Implementations
{
    public class WriteToConsole : IWriterToConsole
    {
        public void Write(string message, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(message);
        }
    }
}