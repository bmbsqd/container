namespace Bombsquad.Container.Tests.Fakes
{
	public class FakeComponentDecorator : IFakeComponent
	{
		public IFakeComponent DecoratedInstance { get; set; }

		public FakeComponentDecorator( IFakeComponent decoratedInstance )
		{
			DecoratedInstance = decoratedInstance;
		}
	}
}
