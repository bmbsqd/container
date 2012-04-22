using System;
using System.Web;

namespace Bombsquad.Container.Web
{
	public static class HttpContextProvider
	{
		public static Func<HttpContextBase> GetHttpContext = () => new HttpContextWrapper( HttpContext.Current );
	}
}