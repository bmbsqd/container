using System;

namespace Bombsquad.Container
{
	public class InvalidComponentImplementationException : ContainerBuilderException
	{
		public InvalidComponentImplementationException( Type componentType, Type implementationType, string message ) : base( componentType, message )
		{
			ImplementationType = implementationType;
		}

		public Type ImplementationType { get; private set; }
	}
}
