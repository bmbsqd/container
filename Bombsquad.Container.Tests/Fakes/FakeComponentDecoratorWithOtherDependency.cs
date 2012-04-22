namespace Bombsquad.Container.Tests.Fakes
{
	public class FakeComponentDecoratorWithOtherDependency : FakeComponentDecorator
	{
		public FakeComponentDecoratorWithOtherDependency( IFakeComponent decoratedInstance, AberFakeComponent otherDependency ) : base( decoratedInstance )
		{
		}
	}
}