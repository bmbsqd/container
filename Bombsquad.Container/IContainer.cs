using System;

namespace Bombsquad.Container
{
	public interface IContainer : IDisposable
	{
		object Resolve( Type type, string name );
		TComponent Resolve<TComponent>( string name );
	}
}
