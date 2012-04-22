namespace Bombsquad.Container.Tests.Fakes
{
	public class DependantOnNamedFakeComponent
	{
		public DependantOnNamedFakeComponent( [NamedComponent( "aber" )] IFakeComponent dependency )
		{
			Dependency = dependency;
		}

		public IFakeComponent Dependency { get; private set; }
	}
}