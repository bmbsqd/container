using System;
using System.Threading;

namespace Bombsquad.Container
{
	internal class SingletonComponentScope<TComponent> : ComponentScope<TComponent>
	{
		private TComponent m_instance;
		private bool m_initialized;

		public override void Dispose()
		{
			if( !m_initialized ) {
				return;
			}
			var disposable = m_instance as IDisposable;
			if( disposable != null ) {
				disposable.Dispose();
			}
		}

		public override TComponent GetOrCreateInstance( Func<TComponent> factory )
		{
			if( !m_initialized ) {
				lock( this ) {
					if( !m_initialized ) {
						m_instance = factory();
						m_initialized = true;
					}
				}
			}
			return m_instance;
		}
	}
}