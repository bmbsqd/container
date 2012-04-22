using Bombsquad.Container.Tests.Fakes;
using NUnit.Framework;

namespace Bombsquad.Container.Tests
{
	[TestFixture]
	public class DecoratorTests
	{
		[Test]
		[ExpectedException( typeof( InvalidComponentDecoratorException ) )]
		public void DecoratorConstructorMustHaveCorrectParameters()
		{
			var builder = new ContainerBuilder();
			builder.Register<IFakeComponent, FakeComponent>().With<AberFakeComponent>();
			builder.Build();
		}

		[Test]
		[ExpectedException( typeof( InvalidComponentDecoratorException ) )]
		public void DecoratorMustExactlyOneConstructor()
		{
			var builder = new ContainerBuilder();
			builder.Register<IFakeComponent, FakeComponent>().With<TwoConstructorsFakeComponent>();
			builder.Build();
		}

		[Test]
		public void CanDecorate()
		{
			var builder = new ContainerBuilder();
			builder.Register<IFakeComponent, FakeComponent>().With<FakeComponentDecorator>();
			var container = builder.Build();
			var fakeComponent = container.Resolve<IFakeComponent>();
			var fakeComponentDecorator = fakeComponent as FakeComponentDecorator;
			Assert.IsNotNull( fakeComponentDecorator );
			Assert.IsAssignableFrom( typeof( FakeComponent ), fakeComponentDecorator.DecoratedInstance );
		}

		[Test]
		public void CanDoubleDecorate()
		{
			var builder = new ContainerBuilder();
			builder.Register<IFakeComponent, FakeComponent>().With<FakeComponentDecorator>().With<FakeComponentDecorator>();
			var container = builder.Build();
			var fakeComponent = container.Resolve<IFakeComponent>();
			var fakeComponentDecorator = fakeComponent as FakeComponentDecorator;
			Assert.IsNotNull( fakeComponentDecorator );
			var fakeComponentDecoratorDecorator = fakeComponentDecorator.DecoratedInstance as FakeComponentDecorator;
			Assert.IsNotNull( fakeComponentDecoratorDecorator );
			Assert.IsAssignableFrom( typeof( FakeComponent ), fakeComponentDecoratorDecorator.DecoratedInstance );
		}

		[Test]
		public void CanDecorateAndResolveNormalDependencies()
		{
			var builder = new ContainerBuilder();
			builder.Register<AberFakeComponent>().SingletonScoped();
			builder.Register<IFakeComponent, FakeComponent>().With<FakeComponentDecoratorWithOtherDependency>();
			var container = builder.Build();
			var fakeComponent = container.Resolve<IFakeComponent>();
			var fakeComponentDecorator = fakeComponent as FakeComponentDecoratorWithOtherDependency;
			Assert.IsNotNull( fakeComponentDecorator );
			Assert.IsAssignableFrom( typeof( FakeComponent ), fakeComponentDecorator.DecoratedInstance );
		}
	}
}
