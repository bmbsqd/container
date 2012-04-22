using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Bombsquad.Container.Tests
{
	[TestFixture]
	public class ReflectionFactoryTests
	{
		public class A
		{
			public string Name
			{
				get { return "Hello"; }
			}
		}

		public class B
		{
			public string Name
			{
				get { return "World"; }
			}
			
		}

		public class Hello
		{
			private A m_a;
			private B m_b;

			public Hello( A a, B b )
			{
				m_a = a;
				m_b = b;
			}

			public string Execute()
			{
				return m_a.Name + " " + m_b.Name;
			}
		}

		public class World
		{
			private Hello m_hello;

			public World( Hello hello )
			{
				m_hello = hello;
			}
		}

		internal class AFactory : ComponentFactory<A>
		{
			public override A CreateInstance()
			{
				return new A();
			}

			public override void Dispose()
			{
			}
		}

		public class BFactory : IUntypedComponentFacilityOrFactory
		{
			public object GetUntypedInstance()
			{
				return new B();
			}
		}

		public class CFactory : IUntypedComponentFacilityOrFactory
		{
			public object GetUntypedInstance()
			{
				return 10;
			}
		}

		[Test]
		public void TestReferenceTypes()
		{
			var f = new ReflectionComponentFactoryFactory<Hello>( typeof(Hello).GetConstructors()[0], new IUntypedComponentFacilityOrFactory[] {new AFactory(), new BFactory()} );
			var helloFactory = f.CreateFactory();
			var hello = helloFactory.CreateInstance();
			var result = hello.Execute();
			Assert.That( result, Is.EqualTo( "Hello World" ) );
		}

		public class TakesIntConstructor
		{
			private readonly int m_value;
			public TakesIntConstructor( int value )
			{
				m_value = value;
			}

			public int Value
			{
				get { return m_value; }
			}
		}

		[Test]
		public void TestValueType()
		{
			var f = new ReflectionComponentFactoryFactory<TakesIntConstructor>( typeof(TakesIntConstructor).GetConstructors()[0], new IUntypedComponentFacilityOrFactory[] {new CFactory()} );
			var factory = f.CreateFactory();
			var instance = factory.CreateInstance();
			Assert.That( instance.Value, Is.EqualTo( 10 ) );
		}
	}
}
