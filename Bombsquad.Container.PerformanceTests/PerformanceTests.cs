using System;
using System.Diagnostics;
using System.Threading;
using Bombsquad.Container.PerformanceTests.Classes;
using NUnit.Framework;

namespace Bombsquad.Container.PerformanceTests
{
	[TestFixture]
	public class PerformanceTests
	{
		private ITestContainer[] m_containers;

		[SetUp]
		public void Setup()
		{
			m_containers = new ITestContainer[] {
				new CastleWindsor(), 
				new Autofac(), 
				new Unity(),
				new StructureMap(), 
				new Bmbsqd()
			};
		}

		[Test]
		public void Test()
		{
			Thread.CurrentThread.Priority = ThreadPriority.Highest;
			RunTestLoop<ISimpleTransientClass>();
			RunTestLoop<IDependantTransientClass>();
			RunTestLoop<IDecoratedService>();
		}

		private void RunTestLoop<T>()
		{
			GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced );
			GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced );
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			stopwatch.Stop();
			stopwatch.Reset();

			Console.WriteLine( typeof(T).Name );
			foreach( var c in m_containers ) {
				stopwatch.Reset();
				stopwatch.Start();
				RunTest<T>( c );
				stopwatch.Stop();
				Console.WriteLine( "{0, 25}: {1,5}", c.GetType().Name, stopwatch.ElapsedMilliseconds );
			}
		}

		private void RunTest<T>( ITestContainer container )
		{
			for( var i = 0; i < 100000; i ++ ) {
				container.Resolve<T>();
			}
		}

		[Test]
		public void Verify()
		{
			foreach( var c in m_containers ) {
				try {
					VerifyTransient( c );
					VerifyDecorator( c );
				}
				catch( Exception e ) {
					Console.WriteLine( "{0} faild verify test", c.GetType().Name );
					throw;
				}
			}
		}

		private void VerifyDecorator( ITestContainer c )
		{
			var service = c.Resolve<IDecoratedService>();
			Assert.That( service, Is.TypeOf<DecoratedServiceDecorator>() );
			Assert.That( service.Foo, Is.TypeOf<Foo>() );
			Assert.That( service.Bar, Is.TypeOf<Bar>() );
		}

		private void VerifyTransient( ITestContainer c )
		{
			var a = c.Resolve<ISimpleTransientClass>();
			var b = c.Resolve<ISimpleTransientClass>();
			Assert.That( a, Is.Not.EqualTo( b ) );
		}
	}
}