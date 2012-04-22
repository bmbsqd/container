using System;

namespace Bombsquad.Container
{
	internal class ValueComponentFactory<TComponent> : ComponentFactory<TComponent>
	{
		private readonly TComponent m_value;

		public ValueComponentFactory( TComponent value )
		{
			m_value = value;
		}

		public override TComponent CreateInstance()
		{
			return m_value;
		}

		public override void Dispose()
		{
			if( m_value is IContainer ) {
				return;
			}
			var disposable = m_value as IDisposable;
			if( disposable != null ) {
				disposable.Dispose();
			}
		}
	}
}