using System;
using System.Linq;
using System.Reflection;

namespace Bombsquad.Container
{
	internal class ReflectionComponentRegistration<TComponent, TImplementation> : ComponentRegistration<TComponent>
	{
		public ReflectionComponentRegistration( string name ) : base( name )
		{
		}

		protected override ComponentFactory<TComponent> CreateComponentFactory( BuildContext context )
		{
			var implementationType = typeof(TImplementation);
			var constructors = implementationType.GetConstructors().OrderByDescending( c => c.GetParameters().Length ).ToArray();
			if( constructors.Length == 0 ) {
				throw new InvalidComponentImplementationException( typeof(TComponent), typeof(TImplementation), "No constructors found in component" );
			}

			Exception lastException = null;
			foreach( var constructorInfo in constructors ) {
				try {
					using( context.Log.BeginScope( "Implementation: <{0}>.ctor({1})", implementationType.FullName, string.Join( ", ", constructorInfo.GetParameters().Select( GetParameterNameForLog ) ) ) ) {
						var facilities = constructorInfo.GetParameters().Select( p => GetParameterComponentFacility( context, p ) ).ToArray();
						return new ReflectionComponentFactoryFactory<TComponent>( constructorInfo, facilities ).CreateFactory();
					}
				}
				catch( UnsatisfiedDependencyException e ) {
					lastException = e;
				}
			}
			throw lastException;
		}

		private static string GetParameterNameForLog( ParameterInfo p )
		{
			var namedAttribute = (NamedComponentAttribute)p.GetCustomAttributes( typeof(NamedComponentAttribute), true ).FirstOrDefault();
			return string.Format( "<{0}, \"{1}\">", p.ParameterType.FullName, namedAttribute == null ? "(null)" : namedAttribute.Name );
		}

		private static IUntypedComponentFacilityOrFactory GetParameterComponentFacility( BuildContext context, ParameterInfo parameterInfo )
		{
			var namedAttribute = (NamedComponentAttribute)parameterInfo.GetCustomAttributes( typeof(NamedComponentAttribute), true ).FirstOrDefault();
			var componentRegistration = context.ResolveConstructorParameter( parameterInfo.ParameterType, namedAttribute == null ? null : namedAttribute.Name );
			return componentRegistration.GetFacility( context );
		}

		public override void Validate( BuildContext context )
		{
			base.Validate( context );
			var implementationType = typeof(TImplementation);
			var componentType = typeof(TComponent);
			if( !implementationType.IsClass ) {
				throw context.LogAndReturnException( new InvalidComponentImplementationException( componentType, implementationType, "Component implementation must be a class." ) );
			}
			if( implementationType.IsAbstract ) {
				throw context.LogAndReturnException( new InvalidComponentImplementationException( componentType, implementationType, "Component implementation must not be abstract." ) );
			}
		}
	}
}