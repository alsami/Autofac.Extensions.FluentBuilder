using System;

namespace Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces
{
    public interface IConsoleWriter
    {
        void Write(string message, ConsoleColor consoleColor);
    }
}