namespace Bombsquad.Container.PerformanceTests.Classes
{
	public interface IDependantTransientClass
	{
		Foo Foo { get; }
		Bar Bar { get; }
	}
}