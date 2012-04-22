bmbsqd-container
=========

* Simple
* Fast
* Typed all way thru
* C#



## Performance comparisons
|                           | CastleWindsor | Autofac |   Unity | StructureMap | Bmbsqd |
|                           | ------------: | ------: | ------: | -----------: | -----: |
| ISimpleTransientClass     |        734 ms |  502 ms |  335 ms |       294 ms |  32 ms |
| IDependantTransientClass  |       3318 ms | 1527 ms | 1022 ms |       575 ms |  58 ms |
| IDecoratedService         |       4761 ms | 2090 ms | 1387 ms |       547 ms |  59 ms |


### Getting started

```CSharp
  var builder = new ContainerBuilder();

  // Auto dependency injection
  builder.Register<ISomeComponent,SomeComponent>();

  // Singleton scoped
  builder.Register<ISomeComponent,SomeComponent>().SingletonScoped();

  // Auto dependency injection with ISomeComponent decorator
  builder.Register<ISomeComponent,SomeComponent>().With<SomeDecorator>();

  // Multiple decorators
  builder.Register<ISomeComponent,SomeComponent>().
    With<LocalCacheDecorator>().
    With<MemcacheDecorator>();

  // Register static value
  builder.Register( "Hello World" );

  // Register a named static value
  builder.Register( "smtp.gmail.com", "smtp-host" );


  // Register a factory for ISomeComponent
  builder.Register<ISomeComponent>( c => new SomeComponent() );


  var container = builder.Build();
  var component = container.Resolve<ISomeComponent>();

  // Resolve named component
  var smtpHost = container.Resolve<string>( "smtp-host" );
```