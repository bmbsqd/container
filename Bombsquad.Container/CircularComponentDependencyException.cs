using System;

namespace Bombsquad.Container
{
	public class CircularComponentDependencyException : ContainerBuilderException
	{
		internal CircularComponentDependencyException( Type componentType )
			: base( componentType, "The component \"" + componentType.FullName + "\" has a circular dependency." )
		{
		}
	}
}
