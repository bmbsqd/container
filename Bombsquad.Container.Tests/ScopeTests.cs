using System;
using Bombsquad.Container.Tests.Fakes;
using NUnit.Framework;

namespace Bombsquad.Container.Tests
{
	[TestFixture]
	public class ScopeTests
	{
		[Test]
		public void TransientScope()
		{
			var builder = new ContainerBuilder();
			builder.Register<FakeComponent>().TransientScoped();

			var container = builder.Build();

			var fakeComponent1 = container.Resolve<FakeComponent>();
			var fakeComponent2 = container.Resolve<FakeComponent>();

			Assert.AreNotSame( fakeComponent1, fakeComponent2 );
		}

		[Test]
		public void FactoryTransientScope()
		{
			var builder = new ContainerBuilder();
			builder.Register( c => new FakeComponent() ).TransientScoped();

			var container = builder.Build();

			var fakeComponent1 = container.Resolve<FakeComponent>();
			var fakeComponent2 = container.Resolve<FakeComponent>();

			Assert.AreNotSame( fakeComponent1, fakeComponent2 );
		}

		[Test]
		public void SingletonScope()
		{
			var builder = new ContainerBuilder();
			builder.Register<FakeComponent>().SingletonScoped();

			var container = builder.Build();

			var fakeComponent1 = container.Resolve<FakeComponent>();
			var fakeComponent2 = container.Resolve<FakeComponent>();

			Assert.AreSame( fakeComponent1, fakeComponent2 );
		}

		[Test]
		public void FactorySingletonScope()
		{
			var builder = new ContainerBuilder();
			builder.Register( c => new FakeComponent() ).SingletonScoped();

			var container = builder.Build();

			var fakeComponent1 = container.Resolve<FakeComponent>();
			var fakeComponent2 = container.Resolve<FakeComponent>();

			Assert.AreSame( fakeComponent1, fakeComponent2 );
		}

		[Test]
		public void SingletonComponentIsDisposed()
		{
			DisposableComponent.DisposeWasCalled = false;
			var builder = new ContainerBuilder();
			builder.Register<DisposableComponent>().SingletonScoped();

			IContainer container = builder.Build();
			container.Resolve<DisposableComponent>();
			container.Dispose();

			Assert.IsTrue( DisposableComponent.DisposeWasCalled );
		}

		[Test]
		public void SingletonComponentIsNotDisposedIfItsNotDisposable()
		{
			DisposableComponent.DisposeWasCalled = false;
			var builder = new ContainerBuilder();
			builder.Register<FakeComponent>().SingletonScoped();

			IContainer container = builder.Build();
			container.Resolve<FakeComponent>();
			container.Dispose();
		}

		[Test]
		public void UnitializedSingletonComponentIsNotDisposed()
		{
			DisposableComponent.DisposeWasCalled = false;
			var builder = new ContainerBuilder();
			builder.Register<DisposableComponent>().SingletonScoped();

			IContainer container = builder.Build();
			container.Dispose();

			Assert.IsFalse( DisposableComponent.DisposeWasCalled );
		}

		[Test]
		public void CanSetOwnScope()
		{
			var builder = new ContainerBuilder();
			builder.Register<FakeComponent>().SetComponentScope( new FakeComponentScope<FakeComponent>() );

			IContainer container = builder.Build();

			container.Resolve<FakeComponent>();
			container.Resolve<FakeComponent>();
			container.Resolve<FakeComponent>();

			Assert.AreEqual( 3, FakeComponentScope<FakeComponent>.NumberOfInstancesCreated );
		}

		[Test]
		public void SettingNullScopeEqualsSettingTransient()
		{
			var builder = new ContainerBuilder();
			builder.Register<FakeComponent>().SetComponentScope( null );
			
			var container = builder.Build();

			var a = container.Resolve<FakeComponent>();
			var b = container.Resolve<FakeComponent>();

			Assert.AreNotSame( a, b );
		}

		[Test]
		public void ValueWithoutScopeIsDisposed()
		{
			DisposableComponent.DisposeWasCalled = false;
			var builder = new ContainerBuilder();
			builder.Register( new DisposableComponent() );

			var container = builder.Build();
			container.Dispose();

			Assert.IsTrue( DisposableComponent.DisposeWasCalled );
		}
	}
}
