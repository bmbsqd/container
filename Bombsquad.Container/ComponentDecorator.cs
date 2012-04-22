using System;
using System.Linq;
using System.Reflection;

namespace Bombsquad.Container
{
	internal abstract class ComponentDecorator<TComponent>
	{
		public abstract ComponentFactory<TComponent> CreateComponentFactory( BuildContext context, ComponentFactory<TComponent> decorated );

		public abstract void Validate( BuildContext context );
	}

	internal class ComponentDecorator<TComponent, TDecorator> : ComponentDecorator<TComponent>
	{
		private readonly ComponentDecorator<TComponent> m_inner;

		public ComponentDecorator( ComponentDecorator<TComponent> inner )
		{
			m_inner = inner;
		}

		public override ComponentFactory<TComponent> CreateComponentFactory( BuildContext context, ComponentFactory<TComponent> decorated )
		{
			using( context.Log.BeginScope( "Decorator: {0}", typeof(TDecorator).FullName ) ) {
				if( m_inner != null ) {
					decorated = m_inner.CreateComponentFactory( context, decorated );
				}
				var constructorInfo = typeof(TDecorator).GetConstructors().First();
				var facilities = constructorInfo.GetParameters().Select( p => GetParameterFactory( context, decorated, p ) ).ToArray();

				return new ReflectionComponentFactoryFactory<TComponent>( constructorInfo, facilities ).CreateFactory();
				//return new ReflectionComponentFactory<TComponent>( constructorInfo, facilities );
			}
		}

		public override void Validate( BuildContext context )
		{
			Type decoratorType = typeof(TDecorator);
			Type componentType = typeof(TComponent);
			var constructorInfos = decoratorType.GetConstructors();
			if( constructorInfos.Length != 1 ) {
				throw context.LogAndReturnException( new InvalidComponentDecoratorException( componentType, decoratorType, "Component decorator must have exactly one constructor." ) );
			}
			if( !constructorInfos.First().GetParameters().Any( p => p.ParameterType == componentType ) ) {
				throw context.LogAndReturnException( new InvalidComponentDecoratorException( componentType, decoratorType,
					"Component decorator constructor must have a parameter of type \"" + componentType.FullName + "\"." ) );
			}
		}

		private static IUntypedComponentFacilityOrFactory GetParameterFactory( BuildContext context, IUntypedComponentFacilityOrFactory decorated, ParameterInfo parameterInfo )
		{
			if( parameterInfo.ParameterType == typeof(TComponent) ) {
				return decorated;
			}
			var namedAttribute = (NamedComponentAttribute)parameterInfo.GetCustomAttributes( typeof(NamedComponentAttribute), true ).FirstOrDefault();
			var componentRegistration = context.ResolveConstructorParameter( parameterInfo.ParameterType, namedAttribute == null ? null : namedAttribute.Name );
			return componentRegistration.GetFacility( context );
		}
	}
}