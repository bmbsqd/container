using System;

namespace Bombsquad.Container
{
	public interface IComponentScope<TComponent> : IDisposable
	{
		TComponent GetOrCreateInstance( Func<TComponent> factory );
	}
}