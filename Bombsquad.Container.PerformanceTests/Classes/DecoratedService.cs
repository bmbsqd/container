namespace Bombsquad.Container.PerformanceTests.Classes
{
	public class DecoratedService : IDecoratedService
	{
		private readonly Foo m_foo;
		private readonly Bar m_bar;

		public DecoratedService( Foo foo, Bar bar )
		{
			m_foo = foo;
			m_bar = bar;
		}

		public Foo Foo
		{
			get { return m_foo; }
		}

		public Bar Bar
		{
			get { return m_bar; }
		}
	}
}