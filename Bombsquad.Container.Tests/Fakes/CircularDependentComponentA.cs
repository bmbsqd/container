namespace Bombsquad.Container.Tests.Fakes
{
	public class CircularDependentComponentA
	{
		public CircularDependentComponentA( CircularDependentComponentB b )
		{
		}
	}
}
