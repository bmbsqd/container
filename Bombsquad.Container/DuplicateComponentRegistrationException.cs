using System;

namespace Bombsquad.Container
{
	public class DuplicateComponentRegistrationException : ContainerException
	{
		internal DuplicateComponentRegistrationException( Type componentType, string componentName ) : base( componentType )
		{
			ComponentName = componentName;
		}

		public string ComponentName { get; private set; }
	}
}
