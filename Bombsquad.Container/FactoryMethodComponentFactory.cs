using System;

namespace Bombsquad.Container
{
	internal class FactoryMethodComponentFactory<TComponent> : ComponentFactory<TComponent>
	{
		private readonly Func<IContainer, TComponent> m_factoryMethod;
		private readonly ComponentFacility<IContainer> m_containerFacility;

		public FactoryMethodComponentFactory( Func<IContainer, TComponent> factoryMethod, ComponentFacility<IContainer> containerFacility )
		{
			m_factoryMethod = factoryMethod;
			m_containerFacility = containerFacility;
		}

		public override TComponent CreateInstance()
		{
			var container = m_containerFacility.GetInstance();
			return m_factoryMethod( container );
		}

		public override void Dispose()
		{
		}
	}
}