namespace Bombsquad.Container
{
	internal class ScopedComponentFacility<TComponent> : ComponentFacility<TComponent>
	{
		private readonly IComponentScope<TComponent> m_scope;

		public ScopedComponentFacility( IComponentScope<TComponent> scope, ComponentFactory<TComponent> factory ) : base( factory )
		{
			m_scope = scope;
		}

		public override TComponent GetInstance()
		{
			return m_scope.GetOrCreateInstance( base.GetInstance );
		}

		public override void Dispose()
		{
			m_scope.Dispose();
			base.Dispose();
		}
	}
}