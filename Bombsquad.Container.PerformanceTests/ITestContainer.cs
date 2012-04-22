namespace Bombsquad.Container.PerformanceTests
{
	public interface ITestContainer
	{
		T Resolve<T>();
	}
}