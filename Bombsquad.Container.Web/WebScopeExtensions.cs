namespace Bombsquad.Container.Web
{
	public static class WebScopeExtensions
	{
		public static IScopableComponentRegistration<TComponent> WebRequestScoped<TComponent>( this IScopableComponentRegistration<TComponent> registration )
		{
			return registration.SetComponentScope( new WebRequestScope<TComponent>() );
		}
	}
}
