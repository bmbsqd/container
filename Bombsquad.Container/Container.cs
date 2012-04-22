using System;
using System.Collections.Generic;

namespace Bombsquad.Container
{
	internal class Container : IContainer
	{
		private IDictionary<ComponentKey, ComponentFacility> m_facilities;

		public void Initialize( IDictionary<ComponentKey, ComponentFacility> facilities )
		{
			m_facilities = facilities;
		}

		public object Resolve( Type type, string name )
		{
			var facility = GetFacility( type, name );
			return facility.GetUntypedInstance();
		}

		public TComponent Resolve<TComponent>( string name )
		{
			var facility = GetFacility( typeof(TComponent), name );
			return ((ComponentFacility<TComponent>)facility).GetInstance();
		}

		private ComponentFacility GetFacility( Type type, string name )
		{
			ComponentFacility facility;
			var key = new ComponentKey( type, name );
			if( !m_facilities.TryGetValue( key, out facility ) ) {
				throw new ComponentNotFoundException( key.Item1, key.Item2 );
			}
			return facility;
		}

		public void Dispose()
		{
			foreach( var componentFacility in m_facilities.Values ) {
				componentFacility.Dispose();
			}
		}
	}
}