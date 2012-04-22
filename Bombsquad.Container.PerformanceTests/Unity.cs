using Microsoft.Practices.Unity;
using Bombsquad.Container.PerformanceTests.Classes;

namespace Bombsquad.Container.PerformanceTests
{
	public sealed class Unity : ITestContainer
	{
		private readonly IUnityContainer m_container;

		public Unity()
		{
			var builder = new UnityContainer();
			builder.RegisterType<ISimpleTransientClass, SimpleTransientClass>();


			builder.RegisterType<IDependantTransientClass, DependantTransientClass>();
			builder.RegisterType<Foo>();
			builder.RegisterType<Bar>();

			builder.RegisterType<IDecoratedService,DecoratedServiceDecorator>(new InjectionConstructor(new ResolvedParameter<DecoratedService>()));

			m_container = builder;
		}

		public T Resolve<T>()
		{
			return m_container.Resolve<T>();
		}
	}
}