using System;

namespace Bombsquad.Container
{
	public class ContainerException : ApplicationException
	{
		internal ContainerException( Type componentType ) : this( componentType, "A container exception occured." )
		{
		}

		internal ContainerException( Type componentType, string message ) : base( message )
		{
			ComponentType = componentType;
		}

		public Type ComponentType { get; private set; }
	}
}
