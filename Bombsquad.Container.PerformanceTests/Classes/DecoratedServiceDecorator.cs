namespace Bombsquad.Container.PerformanceTests.Classes
{
	public class DecoratedServiceDecorator : IDecoratedService
	{
		private readonly IDecoratedService m_decorated;

		public DecoratedServiceDecorator( IDecoratedService decorated )
		{
			m_decorated = decorated;
		}

		public Foo Foo
		{
			get { return m_decorated.Foo; }
		}

		public Bar Bar
		{
			get { return m_decorated.Bar; }
		}
	}
}