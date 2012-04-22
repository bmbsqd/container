using System;

namespace Bombsquad.Container
{
	public static class ContainerBuilderExtensions
	{
		public static IScopableComponentRegistration<TComponent> Register<TComponent>( this ContainerBuilder builder )
		{
			return builder.Register<TComponent, TComponent>( null );
		}

		public static IScopableComponentRegistration<TComponent> Register<TComponent>( this ContainerBuilder builder, string name )
		{
			return builder.Register<TComponent, TComponent>( name );
		}

		public static IScopableComponentRegistration<TComponent> Register<TComponent, TImplementation>( this ContainerBuilder builder ) where TImplementation : TComponent
		{
			return builder.Register<TComponent, TImplementation>( null );
		}

		public static IScopableComponentRegistration<TComponent> Register<TComponent>( this ContainerBuilder builder, TComponent value )
		{
			return builder.Register( value, null );
		}

		public static IScopableComponentRegistration<TComponent> Register<TComponent>( this ContainerBuilder builder, Func<IContainer, TComponent> factoryMethod )
		{
			return builder.Register( factoryMethod, null );
		}
	}
}
