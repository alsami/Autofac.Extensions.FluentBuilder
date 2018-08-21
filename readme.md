# Autofac.Extensions.FluentBuilder

[![NuGet](https://img.shields.io/nuget/dt/Autofac.Extensions.FluentBuilder.svg)](https://www.nuget.org/packages/Autofac.Extensions.FluentBuilder) 
[![NuGet](https://img.shields.io/nuget/vpre/Autofac.Extensions.FluentBuilder.svg)](https://www.nuget.org/packages/Autofac.Extensions.FluentBuilder)

This is a cross platform library, written in .netstandard 2.0, containing a fluent-builder that covers most common usecases to configure your [autofac-ioc-container](https://autofac.org/).

## Installation

This package is available via nuget. You can install it using visual-studio-nuget-browser or by using the dotnet-cli.

```
dotnet add package Autofac.Extensions.FluentBuilder
```

If you want to add a specific version of this package

```
dotnet add package Autofac.Extensions.FluentBuilder --version 1.0.0
```

For more information please visit the official [dotnet-cli documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package).

## Usage

After you have successfully installed the package go ahead to your class and use the fluent-builder.

```c#
var container = new AutofacFluentBuilder()
    .RegisterTypeAsSingleton<MySingleton, IMySingleton>()
    .RegisterTypeAsScoped<MyScoped, IMyScoped>()
    .AddClosedTypeAsScoped(typeof(IClosingType<,>), new [] { typeof(MyProgram).Assembly })
    .AddGeneric(typeof(MyGeneric<,>), typeof(IMyGeneric<,>))
    .ApplyModule<MyModule>()
    .Build();
    
// now you can resolve registered dependencies

```

For more information about the usage please check out the [samples](https://github.com/cleancodelabs/Autofac.Extensions.FluentBuilder/tree/master/samples) I have provided.
