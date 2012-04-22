using System;

namespace Bombsquad.Container
{
	[AttributeUsage( AttributeTargets.Parameter )]
	public class NamedComponentAttribute : Attribute
	{
		public NamedComponentAttribute( string name )
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}
