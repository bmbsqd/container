using Autofac;
using Bombsquad.Container.PerformanceTests.Classes;
using Castle.MicroKernel.Registration;

namespace Bombsquad.Container.PerformanceTests
{
	public sealed class CastleWindsor : ITestContainer
	{
		private readonly Castle.Windsor.IWindsorContainer m_container;

		public CastleWindsor()
		{
			var builder = new Castle.Windsor.WindsorContainer();
			builder.Register( Component.For<ISimpleTransientClass>().ImplementedBy<SimpleTransientClass>().LifeStyle.Transient );

			builder.Register( Component.For<IDependantTransientClass>().ImplementedBy<DependantTransientClass>().LifeStyle.Transient );
			builder.Register( Component.For<Foo>().LifeStyle.Transient );
			builder.Register( Component.For<Bar>().LifeStyle.Transient );

			builder.Register(
				Component.For<IDecoratedService>().ImplementedBy<DecoratedServiceDecorator>().Named( "some-service.decorated" ).ServiceOverrides(
					ServiceOverride.ForKey( "wrapped_repository" ).Eq( "some-service.default" ) ).LifeStyle.Transient );
			builder.Register( Component.For<IDecoratedService>().ImplementedBy<DecoratedService>().Named( "some-service.default" ).LifeStyle.Transient );

			m_container = builder;
		}

		public T Resolve<T>()
		{
			return m_container.Resolve<T>();
		}
	}
}