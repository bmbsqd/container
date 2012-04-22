using Bombsquad.Container.PerformanceTests.Classes;

namespace Bombsquad.Container.PerformanceTests
{
	public sealed class StructureMap : ITestContainer
	{
		private readonly global::StructureMap.IContainer m_container;

		public StructureMap()
		{
			m_container = new global::StructureMap.Container( builder => {
				builder.For<ISimpleTransientClass>().Use<SimpleTransientClass>();

				builder.For<IDependantTransientClass>().Use<DependantTransientClass>();
				builder.For<Foo>().Use<Foo>();
				builder.For<Bar>().Use<Bar>();

				// Not really cool as the inner decorator is not resolved
				builder.For<IDecoratedService>().Use<DecoratedService>().EnrichWith( ( ioc, inner ) => new DecoratedServiceDecorator( inner ) );
			} );
		}

		public T Resolve<T>()
		{
			return m_container.GetInstance<T>();
		}
	}
}