## [1.0.1](https://www.nuget.org/packages/Autofac.Extensions.FluentBuilder/1.0.1) (2018-07-08)

### Bugfixes

* When registering closed-types with resolved-parameters, parameters are now scoped according to the function (singleton, transient or scoped)

## [1.0.0](https://www.nuget.org/packages/Autofac.Extensions.FluentBuilder/1.0.0) (2018-07-08)

### Features

* Registering types with and without interfaces as scoped-, transient- and single-instances
* Registering registring instances with a given type
* Registering generics as scoped-, transient- and single-instances
* Registering closed-types using assembly-scanning with possible resolved-parameters as scoped-, transient- and single-instances
* Registering modules with and without parameters
* Type safety for ever function-call 