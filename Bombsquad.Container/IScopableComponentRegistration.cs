namespace Bombsquad.Container
{
	public interface IScopableComponentRegistration<TComponent> : IComponentRegistration<TComponent>
	{
		IScopableComponentRegistration<TComponent> SetComponentScope( IComponentScope<TComponent> scope );

		IScopableComponentRegistration<TComponent> TransientScoped();

		IScopableComponentRegistration<TComponent> SingletonScoped();
	}
}