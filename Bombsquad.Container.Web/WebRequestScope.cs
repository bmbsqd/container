using System;

namespace Bombsquad.Container.Web
{
	public class WebRequestScope<TComponent> : IComponentScope<TComponent>
	{
		private readonly object m_requestItemId = new object();

		public TComponent GetOrCreateInstance( Func<TComponent> factory )
		{
			var httpContext = HttpContextProvider.GetHttpContext();

			if ( httpContext.Items.Contains( m_requestItemId ) )
			{
				return (TComponent) httpContext.Items[ m_requestItemId ];
			}

			return (TComponent) (httpContext.Items[ m_requestItemId ] = factory());
		}

		public void Dispose()
		{
		}
	}
}
