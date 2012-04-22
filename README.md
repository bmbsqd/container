bmbsqd-container
=========

* Simple
* Fast
* Typed all way thru
* C#



## Performance comparisons
<!--
|                           | CastleWindsor | Autofac |   Unity | StructureMap | Bmbsqd |
|                           | ------------: | ------: | ------: | -----------: | -----: |
| ISimpleTransientClass     |        734 ms |  502 ms |  335 ms |       294 ms |  32 ms |
| IDependantTransientClass  |       3318 ms | 1527 ms | 1022 ms |       575 ms |  58 ms |
| IDecoratedService         |       4761 ms | 2090 ms | 1387 ms |       547 ms |  59 ms |
-->

<table>
<thead>
<tr>
<th>                           </th>
<th align="right"> CastleWindsor </th>
<th align="right"> Autofac </th>
<th align="right">   Unity </th>
<th align="right"> StructureMap </th>
<th align="right"> Bmbsqd </th>
</tr>
</thead>
<tbody>
<tr>
<td> ISimpleTransientClass     </td>
<td align="right">        734 ms </td>
<td align="right">  502 ms </td>
<td align="right">  335 ms </td>
<td align="right">       294 ms </td>
<td align="right">  32 ms </td>
</tr>
<tr>
<td> IDependantTransientClass  </td>
<td align="right">       3318 ms </td>
<td align="right"> 1527 ms </td>
<td align="right"> 1022 ms </td>
<td align="right">       575 ms </td>
<td align="right">  58 ms </td>
</tr>
<tr>
<td> IDecoratedService         </td>
<td align="right">       4761 ms </td>
<td align="right"> 2090 ms </td>
<td align="right"> 1387 ms </td>
<td align="right">       547 ms </td>
<td align="right">  59 ms </td>
</tr>
</tbody>
</table>

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