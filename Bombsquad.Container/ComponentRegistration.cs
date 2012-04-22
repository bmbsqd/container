namespace Bombsquad.Container
{
	internal abstract class ComponentRegistration
	{
		private ComponentFacility m_facility;

		protected ComponentRegistration( string name )
		{
			Name = name;
		}

		public ComponentFacility GetFacility( BuildContext context )
		{
			return m_facility ?? (m_facility = CreateFacility( context ));
		}

		protected abstract ComponentFacility CreateFacility( BuildContext context );

		public string Name { get; private set; }

		public abstract void Validate( BuildContext context );
	}

	internal abstract class ComponentRegistration<TComponent> : ComponentRegistration, IScopableComponentRegistration<TComponent>
	{
		private ComponentDecorator<TComponent> m_decorator;
		private IComponentScope<TComponent> m_componentScope;

		protected ComponentRegistration( string name ) : base( name )
		{
		}

		public IScopableComponentRegistration<TComponent> With<TDecorator>() where TDecorator : TComponent
		{
			m_decorator = new ComponentDecorator<TComponent, TDecorator>( m_decorator );
			return this;
		}

		protected override ComponentFacility CreateFacility( BuildContext context )
		{
			using ( context.Visit<TComponent>() )
			{
				return new ComponentFacility<TComponent>( GetComponentScope(), Decorate( context, CreateComponentFactory( context ) ) );
			}
		}

		public override void Validate( BuildContext context )
		{
			if ( m_decorator != null )
			{
				m_decorator.Validate( context );
			}
		}

		private ComponentFactory<TComponent> Decorate( BuildContext context, ComponentFactory<TComponent> componentFactory )
		{
			return m_decorator != null ? m_decorator.CreateComponentFactory( context, componentFactory ) : componentFactory;
		}

		protected abstract ComponentFactory<TComponent> CreateComponentFactory( BuildContext context );

		private IComponentScope<TComponent> GetComponentScope()
		{
			return m_componentScope ?? new TransientComponentScope<TComponent>();
		}

		public IScopableComponentRegistration<TComponent> SetComponentScope( IComponentScope<TComponent> scope )
		{
			m_componentScope = scope;
			return this;
		}

		public IScopableComponentRegistration<TComponent> TransientScoped()
		{
			m_componentScope = new TransientComponentScope<TComponent>();
			return this;
		}

		public IScopableComponentRegistration<TComponent> SingletonScoped()
		{
			m_componentScope = new SingletonComponentScope<TComponent>();
			return this;
		}
	}
}
