namespace Bombsquad.Container.Tests.Fakes
{
	public interface IFakeComponentWithValue<out T>
	{
		T Value{ get; }
	}
}