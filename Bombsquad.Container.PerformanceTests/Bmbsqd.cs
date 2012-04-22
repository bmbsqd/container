using Bombsquad.Container.PerformanceTests.Classes;

namespace Bombsquad.Container.PerformanceTests
{
	public sealed class Bmbsqd : ITestContainer
	{
		private readonly IContainer m_container;

		public Bmbsqd()
		{
			var builder = new ContainerBuilder();

			builder.Register<ISimpleTransientClass, SimpleTransientClass>().TransientScoped();

			builder.Register<IDependantTransientClass, DependantTransientClass>().TransientScoped();
			builder.Register<Foo>().TransientScoped();
			builder.Register<Bar>().TransientScoped();

			builder.Register<IDecoratedService, DecoratedService>().With<DecoratedServiceDecorator>();

			m_container = builder.Build();
		}

		public T Resolve<T>()
		{
			return m_container.Resolve<T>();
		}
	}
}