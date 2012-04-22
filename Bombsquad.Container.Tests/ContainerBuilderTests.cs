using System;
using Bombsquad.Container.Tests.Fakes;
using NUnit.Framework;

namespace Bombsquad.Container.Tests
{
	[TestFixture]
	public class ContainerBuilderTests
	{
		private ContainerBuilder m_builder;

		[SetUp]
		public void SetUp()
		{
			m_builder = new ContainerBuilder();
		}

		[TearDown]
		public void TearDown()
		{
			if ( m_builder != null )
			{
				Console.WriteLine( m_builder.BuildLog );
			}
		}

		[Test]
		public void CanConstructContainer()
		{
			IContainer container = m_builder.Build();
			Assert.IsNotNull( container );
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void CanOnlyConstructOneContainer()
		{
			m_builder.Build();
			m_builder.Build();
		}

		[Test]
		[ExpectedException( typeof( DuplicateComponentRegistrationException ) )]
		public void CannotRegisterIContainer()
		{
			m_builder.Register<IContainer, FakeContainer>();
		}

		[Test]
		[ExpectedException( typeof( DuplicateComponentRegistrationException ) )]
		public void CannotRegisterDuplicate()
		{
			m_builder.Register<IFakeComponent, FakeComponent>();
			m_builder.Register<IFakeComponent, FakeComponent>();
		}

		[Test]
		[ExpectedException( typeof( InvalidComponentImplementationException ) )]
		public void MustRegisterClassForImplementation()
		{
			m_builder.Register<IFakeComponent, IFakeComponent>();
			m_builder.Build();
		}

		[Test]
		[ExpectedException( typeof( InvalidComponentImplementationException ) )]
		public void CannotRegisterAbstractForImplementation()
		{
			m_builder.Register<IFakeComponent, AbstractFakeComponent>();
			m_builder.Build();
		}

		[Test]
		public void LargestConstructorFirst()
		{
			m_builder.Register<int>( 10 );
			m_builder.Register<IFakeComponentWithValue<int>, DualConstructorFakeComponent>();
			var container = m_builder.Build();
			var fake = container.Resolve<IFakeComponentWithValue<int>>();
			Assert.That( fake.Value, Is.EqualTo( 10 ) );
		}

		[Test]
		public void FirstSatisfiedConstructor()
		{
			m_builder.Register<int>( 10 );
			m_builder.Register<IFakeComponentWithValue<int>, ManyConstructorsFakeComponent>();
			var container = m_builder.Build();
			var fake = container.Resolve<IFakeComponentWithValue<int>>();
			Assert.That( fake.Value, Is.EqualTo( 10 ) );
		}

		[Test]
		public void CanResolveContainerItSelf()
		{
			IContainer container = m_builder.Build();
			var resolvedContainer = container.Resolve<IContainer>();
			Assert.AreSame( container, resolvedContainer );
		}

		[Test]
		public void CanRegisterValueComponent()
		{
			var component = new FakeComponent();
			m_builder.Register( component );
			IContainer container = m_builder.Build();
			var resolvedComponent = container.Resolve<FakeComponent>();
			Assert.AreSame( component, resolvedComponent );
		}

		[Test]
		public void CanRegisterComponent()
		{
			m_builder.Register<IFakeComponent, FakeComponent>();
			IContainer container = m_builder.Build();
			var component = container.Resolve<IFakeComponent>();
			Assert.IsNotNull( component );
			Assert.IsInstanceOf( typeof( IFakeComponent ), component );
		}

		[Test]
		public void CanResolveComponentsWithDependencies()
		{
			m_builder.Register<DependantOnFakeComponent, DependantOnFakeComponent>();
			m_builder.Register<IFakeComponent, FakeComponent>();
			IContainer container = m_builder.Build();
			var component = container.Resolve<DependantOnFakeComponent>();
			Assert.IsNotNull( component );
		}

		[Test]
		[ExpectedException( typeof( CircularComponentDependencyException ) )]
		public void CanDetectSimpleCircularDependencies()
		{
			m_builder.Register<SimpleCircularDependentComponent, SimpleCircularDependentComponent>();
			m_builder.Build();
		}

		[Test]
		[ExpectedException( typeof( CircularComponentDependencyException ) )]
		public void CanDetectCircularDependencies()
		{
			m_builder.Register<CircularDependentComponentA, CircularDependentComponentA>();
			m_builder.Register<CircularDependentComponentB, CircularDependentComponentB>();
			m_builder.Build();
		}

		[Test]
		[ExpectedException( typeof( UnsatisfiedDependencyException ) )]
		public void CanDetectUnsatisfiedDependencies()
		{
			m_builder.Register<UnsatisfiedComponent, UnsatisfiedComponent>();
			m_builder.Build();
		}

		[Test]
		public void CanRegisterWithName()
		{
			m_builder.Register<IFakeComponent, AberFakeComponent>( "aber" );
			m_builder.Register<IFakeComponent, FakeComponent>();

			var container = m_builder.Build();
			var aberFakeComponent = container.Resolve<IFakeComponent>( "aber" );
			var fakeComponent = container.Resolve<IFakeComponent>();

			Assert.AreNotSame( aberFakeComponent, fakeComponent );
			Assert.IsAssignableFrom( typeof( AberFakeComponent ), aberFakeComponent );
			Assert.IsAssignableFrom( typeof( FakeComponent ), fakeComponent );
		}

		[Test]
		public void CanRegisterValueWithName()
		{
			const string aber = "aber";
			m_builder.Register( aber, aber );
			const string lol = "lol";
			m_builder.Register( lol, lol );

			var container = m_builder.Build();
			var resolvedAber = container.Resolve<string>( aber );
			var resolvedLol = container.Resolve<string>( lol );

			Assert.AreEqual( aber, resolvedAber );
			Assert.AreEqual( lol, resolvedLol );
		}

		[Test]
		[ExpectedException( typeof( DuplicateComponentRegistrationException ) )]
		public void CannotRegisterWithSameNameAndType()
		{
			m_builder.Register<IFakeComponent, AberFakeComponent>( "aber" );
			m_builder.Register<IFakeComponent, FakeComponent>( "aber" );

			m_builder.Build();
		}

		[Test]
		public void CanRegisterDifferentTypesWithSameName()
		{
			m_builder.Register( "detta är en sträng", "aber" );
			m_builder.Register<FakeComponent, FakeComponent>( "aber" );

			var container = m_builder.Build();
			var resolvedAberString = container.Resolve<string>( "aber" );
			var resolvedAberComponent = container.Resolve<FakeComponent>( "aber" );

			Assert.AreEqual( "detta är en sträng", resolvedAberString );
			Assert.IsNotNull( resolvedAberComponent );
		}

		[Test]
		[ExpectedException( typeof( ComponentNotFoundException ) )]
		public void CannotResolveNamedComponentWithoutName()
		{
			m_builder.Register<FakeComponent>( "aber" );

			var container = m_builder.Build();
			container.Resolve<FakeComponent>();
		}

		[Test]
		[ExpectedException( typeof( ComponentNotFoundException ) )]
		public void CannotResolveUnamedComponentWithName()
		{
			m_builder.Register<FakeComponent, FakeComponent>();

			var container = m_builder.Build();
			container.Resolve<FakeComponent>( "aber" );
		}

		[Test]
		public void CanRegisterFactoryMethod()
		{
			m_builder.Register<IFakeComponent>( c => new FakeComponent() );

			var container = m_builder.Build();
			container.Resolve<IFakeComponent>();
		}

		[Test]
		[ExpectedException( typeof( CircularComponentDependencyException ) )]
		[Explicit( "This test causes an infinite loop. Fix the problem and then reactivate this test... :)" )]
		public void CanDetectCircularDependencyInFactoryMethod()
		{
			m_builder.Register<CircularDependentComponentA, CircularDependentComponentA>();
			m_builder.Register( c => new CircularDependentComponentB( c.Resolve<CircularDependentComponentA>() ) );

			var container = m_builder.Build();

			container.Resolve<CircularDependentComponentB>();
		}

		[Test]
		public void CanResolveFactoryMethodDependency()
		{
			m_builder.Register<DependantOnFakeComponent, DependantOnFakeComponent>();
			m_builder.Register<IFakeComponent>( c => new FakeComponent() );
			IContainer container = m_builder.Build();
			var component = container.Resolve<DependantOnFakeComponent>();
			Assert.IsNotNull( component );
		}

		[Test]
		public void CanResolveNamnedDependency()
		{
			m_builder.Register<DependantOnNamedFakeComponent, DependantOnNamedFakeComponent>();
			m_builder.Register<IFakeComponent, FakeComponent>();
			var fakeComponent = new FakeComponent();
			m_builder.Register<IFakeComponent>( fakeComponent, "aber" );
			IContainer container = m_builder.Build();
			var component = container.Resolve<DependantOnNamedFakeComponent>();
			Assert.IsNotNull( component );
			Assert.AreSame( fakeComponent, component.Dependency );
		}
	}
}
