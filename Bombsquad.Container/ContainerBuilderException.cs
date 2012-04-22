using System;

namespace Bombsquad.Container
{
	public class ContainerBuilderException : ContainerException
	{
		internal ContainerBuilderException( Type componentType, string message ) : base( componentType, message )
		{
		}

		public string BuildLog { get; internal set; }
	}
}
