using System;

namespace Bombsquad.Container
{
	public static class ContainerExtensions
	{
		public static object Resolve( this IContainer container, Type type )
		{
			return container.Resolve( type, null );
		}

		public static TComponent Resolve<TComponent>( this IContainer container )
		{
			return container.Resolve<TComponent>( null );
		}
	}
}