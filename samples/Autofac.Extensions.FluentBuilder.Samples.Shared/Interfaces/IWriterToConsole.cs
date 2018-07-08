using System;

namespace Autofac.Extensions.FluentBuilder.Samples.Shared.Interfaces
{
    public interface IWriterToConsole
    {
        void Write(string message, ConsoleColor consoleColor);
    }
}