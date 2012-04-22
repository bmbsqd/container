using System;

namespace Bombsquad.Container
{
	internal abstract class ComponentScope<TComponent> : IComponentScope<TComponent>
	{
		public abstract void Dispose();

		public abstract TComponent GetOrCreateInstance( Func<TComponent> factory );
	}
}