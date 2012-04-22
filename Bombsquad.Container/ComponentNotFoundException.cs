using System;

namespace Bombsquad.Container
{
	public class ComponentNotFoundException : ContainerException
	{
		internal ComponentNotFoundException( Type componentType, string componentName ) : base( componentType )
		{
			ComponentName = componentName;
		}

		public string ComponentName { get; private set; }
	}
}
