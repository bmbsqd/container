using System.Linq;
using NUnit.Framework;

namespace Bombsquad.Container.Web.Tests
{
	[TestFixture]
	public class WebRequestScopeTests
	{
		[Test]
		public void CanUseWebRequestScope()
		{
			var fakeHttpContext = new FakeHttpContext();

			HttpContextProvider.GetHttpContext = () => fakeHttpContext;

			var builder = new ContainerBuilder();
			builder.Register<TestComponent>().WebRequestScoped();

			var container = builder.Build();

			var resolve1 = container.Resolve<TestComponent>();
			var resolve2 = container.Resolve<TestComponent>();

			Assert.AreSame( resolve1, resolve2 );
			Assert.AreEqual( 1, fakeHttpContext.m_items.Count );
		}

		public class TestComponent
		{
		}
	}
}
