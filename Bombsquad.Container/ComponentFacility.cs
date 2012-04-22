using System;

namespace Bombsquad.Container
{
	internal abstract class ComponentFacility : IDisposable, IUntypedComponentFacilityOrFactory
	{
		public abstract object GetUntypedInstance();

		public abstract void Dispose();
	}

	internal class ComponentFacility<TComponent> : ComponentFacility
	{
		private readonly IComponentScope<TComponent> m_scope;
		private readonly ComponentFactory<TComponent> m_factory;

		public ComponentFacility( IComponentScope<TComponent> scope, ComponentFactory<TComponent> factory )
		{
			m_scope = scope;
			m_factory = factory;
		}

		public virtual TComponent GetInstance()
		{
			return m_scope.GetOrCreateInstance( m_factory.CreateInstance );
		}

		public override object GetUntypedInstance()
		{
			return GetInstance();
		}

		public override void Dispose()
		{
			m_scope.Dispose();
			m_factory.Dispose();
		}
	}
}
