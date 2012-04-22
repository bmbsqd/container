using System;

namespace Bombsquad.Container
{
	internal abstract class ComponentFactory : IDisposable, IUntypedComponentFacilityOrFactory
	{
		public abstract void Dispose();

		public abstract object GetUntypedInstance();
	}

	internal abstract class ComponentFactory<TComponent> : ComponentFactory
	{
		public abstract TComponent CreateInstance();

		public override object GetUntypedInstance()
		{
			return CreateInstance();
		}
	}
}