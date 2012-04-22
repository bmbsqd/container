using System;
using System.Collections.Generic;
using System.Linq;

namespace Bombsquad.Container
{
	public class ContainerBuilder
	{
		private readonly Container m_container = new Container();
		private readonly IDictionary<ComponentKey, ComponentRegistration> m_registrations = new Dictionary<ComponentKey, ComponentRegistration>();
		private bool m_isDirty;
		private BuildLog m_buildLog;

		public ContainerBuilder()
		{
			Register<IContainer>( m_container, null );
		}

		public IScopableComponentRegistration<TComponent> Register<TComponent, TImplementation>( string name ) where TImplementation : TComponent
		{
			var registration = new ReflectionComponentRegistration<TComponent, TImplementation>( name );
			AddRegistration<TComponent>( registration );
			return registration;
		}

		public IScopableComponentRegistration<TComponent> Register<TComponent>( TComponent value, string name )
		{
			var registration = new ValueComponentRegistration<TComponent>( value, name );
			AddRegistration<TComponent>( registration );
			return registration;
		}

		public IScopableComponentRegistration<TComponent> Register<TComponent>( Func<IContainer, TComponent> factoryMethod, string name )
		{
			var registration = new FactoryMethodComponentRegistration<TComponent>( factoryMethod, name );
			AddRegistration<TComponent>( registration );
			return registration;
		}

		public IContainer Build()
		{
			if( m_isDirty ) {
				throw new InvalidOperationException( "Can only construct one container." );
			}
			m_buildLog = new BuildLog();
			var context = new BuildContext( m_registrations, m_buildLog );
			using( context.Log.BeginScope( "Container" ) ) {
				foreach( var registration in m_registrations ) {
					registration.Value.Validate( context );
				}
				var facilities = new Dictionary<ComponentKey, ComponentFacility>();
				foreach( var registration in m_registrations ) {
					using( context.Log.BeginScope( "Component: <{0}, \"{1}\">", registration.Key.Item1.FullName, registration.Key.Item2 ?? "(null)" ) ) {
						facilities.Add( registration.Key, registration.Value.GetFacility( context ) );
					}
				}
				m_container.Initialize( facilities );
			}
			m_isDirty = true;
			//BuildLog = context.Log.ToString();
			return m_container;
		}

		private void AddRegistration<TComponent>( ComponentRegistration registration )
		{
			var key = new ComponentKey( typeof(TComponent), registration.Name );
			if( m_registrations.ContainsKey( key ) ) {
				throw new DuplicateComponentRegistrationException( key.Item1, key.Item2 );
			}
			m_registrations.Add( key, registration );
		}

		public string BuildLog
		{
			get
			{
				return m_buildLog != null ? m_buildLog.ToString() : null;
			}
		}
	}
}