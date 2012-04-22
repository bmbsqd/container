namespace Bombsquad.Container
{
	public interface IScopedComponentRegistration<TComponent> : IComponentRegistration
	{
		IComponentRegistration SetComponentScope( IComponentScope<TComponent> scope );

		IComponentRegistration TransientScoped();

		IComponentRegistration SingletonScoped();
	}
}