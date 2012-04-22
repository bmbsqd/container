using System;

namespace Bombsquad.Container
{
	internal class FactoryMethodComponentRegistration<TComponent> : ComponentRegistration<TComponent>
	{
		private readonly Func<IContainer, TComponent> m_factoryMethod;

		public FactoryMethodComponentRegistration( Func<IContainer, TComponent> factoryMethod, string name ) : base( name )
		{
			m_factoryMethod = factoryMethod;
		}

		protected override ComponentFactory<TComponent> CreateComponentFactory( BuildContext context )
		{
			var containerFacility = (ComponentFacility<IContainer>)context.ResolveConstructorParameter( typeof(IContainer), null ).GetFacility( context );
			using( context.Log.BeginScope( "FactoryMethod: {0}", m_factoryMethod.Method.Name ) ) {
				return new FactoryMethodComponentFactory<TComponent>( m_factoryMethod, containerFacility );
			}
		}
	}
}