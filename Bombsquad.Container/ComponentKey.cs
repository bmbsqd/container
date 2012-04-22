using System;

namespace Bombsquad.Container
{
	internal class ComponentKey : Tuple<Type, string>
	{
		public ComponentKey( Type type, string name ) : base( type, name )
		{
		}
	}
}