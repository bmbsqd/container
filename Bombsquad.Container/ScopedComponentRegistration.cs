using System;

namespace Bombsquad.Container
{
	internal abstract class ScopedComponentRegistration<TComponent> : ComponentRegistration, IScopedComponentRegistration<TComponent>
	{
		private IComponentScope<TComponent> m_componentScope;

		protected ScopedComponentRegistration( string name ) : base( name )
		{
		}

		protected IComponentScope<TComponent> GetComponentScope()
		{
			return m_componentScope ?? new TransientComponentScope<TComponent>();
		}

		public IComponentRegistration SetComponentScope( IComponentScope<TComponent> scope )
		{
			if (scope == null)
			{
				throw new ArgumentNullException( "scope" );
			}
			m_componentScope = scope;
			return this;
		}

		public IComponentRegistration TransientScoped()
		{
			m_componentScope = new TransientComponentScope<TComponent>();
			return this;
		}

		public IComponentRegistration SingletonScoped()
		{
			m_componentScope = new SingletonComponentScope<TComponent>();
			return this;
		}
	}
}
