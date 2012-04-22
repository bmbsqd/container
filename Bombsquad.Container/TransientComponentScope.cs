using System;

namespace Bombsquad.Container
{
	internal class TransientComponentScope<TComponent> : ComponentScope<TComponent>
	{
		public override void Dispose()
		{
		}

		public override TComponent GetOrCreateInstance( Func<TComponent> factory )
		{
			return factory();
		}
	}
}