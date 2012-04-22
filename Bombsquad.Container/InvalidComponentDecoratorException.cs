using System;

namespace Bombsquad.Container
{
	public class InvalidComponentDecoratorException : ContainerBuilderException
	{
		public InvalidComponentDecoratorException( Type componentType, Type decoratorType, string message ) : base( componentType, message )
		{
			DecoratorType = decoratorType;
		}

		public Type DecoratorType { get; private set; }
	}
}
