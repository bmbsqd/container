using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo( Bombsquad.Container.ContainerFactories.Name )]

namespace Bombsquad.Container
{
	internal static class ContainerFactories
	{
		public const string Name = "ContainerFactories";
	}

	internal class ReflectionComponentFactoryFactory<TComponent>
	{
		private readonly ConstructorInfo m_constructorInfo;
		private readonly IUntypedComponentFacilityOrFactory[] m_parameterFactories;
		private ILGenerator m_constructorIL;

		public ReflectionComponentFactoryFactory( ConstructorInfo constructorInfo, IUntypedComponentFacilityOrFactory[] parameterFactories )
		{
			m_constructorInfo = constructorInfo;
			m_parameterFactories = parameterFactories;
		}

		private static MethodInfo GetMethod<T, TResult>( Expression<Func<T, TResult>> m )
		{
			var mce = m.Body as MethodCallExpression;
			return mce != null ? mce.Method : null;
		}

		private static MethodInfo GetMethod<T>( Expression<Action<T>> m )
		{
			var mce = m.Body as MethodCallExpression;
			return mce != null ? mce.Method : null;
		}

		private static AssemblyBuilder m_factoriesAssembly;

		private static AssemblyBuilder FactoriesAssembly
		{
			get { return m_factoriesAssembly ?? (m_factoriesAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly( new AssemblyName( ContainerFactories.Name ), AssemblyBuilderAccess.RunAndCollect )); }
		}

		private static string ContainerFactoriesFileName
		{
			get
			{
				return ContainerFactories.Name + ".dll";
			}
		}

		private static ModuleBuilder m_factoriesModule;

		private static ModuleBuilder FactoriesModule
		{
			get { return m_factoriesModule ?? (m_factoriesModule = FactoriesAssembly.DefineDynamicModule( ContainerFactoriesFileName, ContainerFactoriesFileName, true )); }
		}

		private static MethodInfo m_createInstanceMethod;

		public static MethodInfo CreateInstanceMethod
		{
			get { return m_createInstanceMethod ?? (m_createInstanceMethod = GetMethod<ComponentFactory<TComponent>, TComponent>( c => c.CreateInstance() )); }
		}

		private static MethodInfo m_getUntypedInstanceMethod;

		private static MethodInfo GetUntypedInstanceMethod
		{
			get { return m_getUntypedInstanceMethod ?? (m_getUntypedInstanceMethod = GetMethod<IUntypedComponentFacilityOrFactory, object>( f => f.GetUntypedInstance() )); }
		}

		public ComponentFactory<TComponent> CreateFactory()
		{
			var typeBuilder = FactoriesModule.DefineType( "GeneratedFactories." + typeof(TComponent).Name + "ComponentFactory" + Guid.NewGuid().ToString( "n" ), TypeAttributes.NotPublic | TypeAttributes.Sealed,
				typeof(ComponentFactory<TComponent>) );
			BuildDisposeMethod( typeBuilder );

			m_constructorIL = typeBuilder.DefineConstructor( MethodAttributes.Public, CallingConventions.Standard, new[] {typeof(IUntypedComponentFacilityOrFactory[])} ).GetILGenerator();
			BuildCreateInstanceMethod( typeBuilder );

			var t = typeBuilder.CreateType();
			return (ComponentFactory<TComponent>)t.GetConstructor( new[] {typeof(IUntypedComponentFacilityOrFactory[])} ).Invoke( new[] {m_parameterFactories} );
		}

		private void BuildCreateInstanceMethod( TypeBuilder typeBuilder )
		{
			var m = typeBuilder.DefineMethod( CreateInstanceMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual );
			m.SetReturnType( typeof(TComponent) );
			typeBuilder.DefineMethodOverride( m, CreateInstanceMethod );

			var il = m.GetILGenerator();
			var constructorParameters = m_constructorInfo.GetParameters();
			for( var i = 0; i < constructorParameters.Length; i++ ) {
				var factory = m_parameterFactories[i];
				var factoryType = factory.GetType();
				Type componentType;
				if( TryGetComponentType( factoryType, typeof(ComponentFactory<>), out componentType ) ) {
					var componentFactory = typeof(ComponentFactory<>).MakeGenericType( componentType );
					var componentCreateInstanceMethod = componentFactory.GetMethod( CreateInstanceMethod.Name );
					var field = typeBuilder.DefineField( "m_" + constructorParameters[i].Name + "Factory", componentFactory, FieldAttributes.Private );
					il.Emit( OpCodes.Ldarg_0 );
					il.Emit( OpCodes.Ldfld, field );
					il.Emit( OpCodes.Callvirt, componentCreateInstanceMethod );
					GenerateConstructorFactoryField( i, componentFactory, field );
				}
				else if( TryGetComponentType( factoryType, typeof(ComponentFacility<>), out componentType ) ) {
					var componentFactory = typeof(ComponentFacility<>).MakeGenericType( componentType );
					var componentCreateInstanceMethod = componentFactory.GetMethod( GetMethod<ComponentFacility<TComponent>>( c => c.GetInstance() ).Name );
					var field = typeBuilder.DefineField( "m_" + constructorParameters[i].Name + "Factory", componentFactory, FieldAttributes.Private );
					il.Emit( OpCodes.Ldarg_0 );
					il.Emit( OpCodes.Ldfld, field );
					il.Emit( OpCodes.Callvirt, componentCreateInstanceMethod );
					GenerateConstructorFactoryField( i, componentFactory, field );
				}
				else if( typeof(IUntypedComponentFacilityOrFactory).IsAssignableFrom( factoryType ) ) {
					var field = typeBuilder.DefineField( "m_" + constructorParameters[i].Name + "Factory", typeof(IUntypedComponentFacilityOrFactory), FieldAttributes.Private );
					var parameterType = constructorParameters[i].ParameterType;
					il.Emit( OpCodes.Ldarg_0 );
					il.Emit( OpCodes.Ldfld, field );
					il.Emit( OpCodes.Callvirt, GetUntypedInstanceMethod );
					if( parameterType.IsValueType ) {
						il.Emit( OpCodes.Unbox_Any, parameterType );
					}
					else {
						il.Emit( OpCodes.Castclass, parameterType );
					}
					GenerateConstructorFactoryField( i, typeof(IUntypedComponentFacilityOrFactory), field );
				}
			}
			il.Emit( OpCodes.Newobj, m_constructorInfo );
			il.Emit( OpCodes.Ret );
			m_constructorIL.Emit( OpCodes.Ret );
		}

		private void BuildDisposeMethod( TypeBuilder typeBuilder )
		{
			var disposeMethod = GetMethod<IDisposable>( d => d.Dispose() );
			typeBuilder.DefineMethod( disposeMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual ).GetILGenerator().Emit( OpCodes.Ret );
		}

		private static bool TryGetComponentType( Type type, Type generic, out Type componentType )
		{
			for( var t = type; t != typeof(object) && t != null; t = t.BaseType ) {
				if( t.IsGenericType && t.GetGenericTypeDefinition() == generic ) {
					componentType = t.GetGenericArguments()[0];
					return true;
				}
			}
			componentType = null;
			return false;
		}

		private void GenerateConstructorFactoryField( int index, Type factoryType, FieldInfo field )
		{
			m_constructorIL.Emit( OpCodes.Ldarg_0 );
			m_constructorIL.Emit( OpCodes.Ldarg_1 );
			m_constructorIL.Emit( OpCodes.Ldc_I4, index );
			m_constructorIL.Emit( OpCodes.Ldelem_Ref );
			if( factoryType != typeof(IUntypedComponentFacilityOrFactory) ) {
				m_constructorIL.Emit( OpCodes.Castclass, factoryType );
			}
			m_constructorIL.Emit( OpCodes.Stfld, field );
		}
	}
}