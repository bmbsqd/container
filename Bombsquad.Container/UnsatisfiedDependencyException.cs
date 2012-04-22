using System;

namespace Bombsquad.Container
{
	public class UnsatisfiedDependencyException : ContainerBuilderException
	{
		internal UnsatisfiedDependencyException( Type componentType, Type parameterType )
			: base( componentType, "The component \"" + componentType.FullName + "\" has an unsatisfied dependency for \"" + parameterType.Name + "\"." )
		{
			ParameterType = parameterType;
		}

		public Type ParameterType { get; private set; }
	}
}
