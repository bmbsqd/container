using System;
using Autofac;
using Autofac.Core;
using Bombsquad.Container.PerformanceTests.Classes;

namespace Bombsquad.Container.PerformanceTests
{
	public static class AutofacExtensions
	{
		public static void RegisterDecorator<TInterface, TImplementation, TDecorator>( this global::Autofac.ContainerBuilder builder )
		{
			var innerName = Guid.NewGuid().ToString();
			builder.RegisterType<TImplementation>().Named<TInterface>( innerName );
			builder.RegisterType<TDecorator>().As<TInterface>().WithParameter( new ResolvedParameter( ( pi, cc ) => pi.ParameterType == typeof(TInterface),
				( pi, cc ) => cc.ResolveNamed<TInterface>( innerName ) ) );
		}
	}

	public sealed class Autofac : ITestContainer
	{
		private readonly global::Autofac.IContainer m_container;

		public Autofac()
		{
			var builder = new global::Autofac.ContainerBuilder();
			builder.RegisterType<SimpleTransientClass>().As<ISimpleTransientClass>().InstancePerDependency();

			builder.RegisterType<DependantTransientClass>().As<IDependantTransientClass>().InstancePerDependency();
			builder.RegisterType<Foo>().InstancePerDependency();
			builder.RegisterType<Bar>().InstancePerDependency();

			builder.RegisterDecorator<IDecoratedService, DecoratedService, DecoratedServiceDecorator>();

			m_container = builder.Build();
		}

		public T Resolve<T>()
		{
			return m_container.Resolve<T>();
		}
	}
}