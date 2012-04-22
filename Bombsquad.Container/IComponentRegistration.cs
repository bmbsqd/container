namespace Bombsquad.Container
{
	public interface IComponentRegistration<TComponent>
	{
		IScopableComponentRegistration<TComponent> With<TDecorator>() where TDecorator : TComponent;
	}
}