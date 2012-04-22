using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Bombsquad.Container
{
	internal class ReflectionComponentFactory<TComponent> : ComponentFactory<TComponent>
	{
		private readonly ConstructorInfo m_constructorInfo;
		private readonly IUntypedComponentFacilityOrFactory[] m_parameterFactories;
		private readonly Func<IUntypedComponentFacilityOrFactory[], TComponent> m_factory;

		public ReflectionComponentFactory( ConstructorInfo constructorInfo, IEnumerable<IUntypedComponentFacilityOrFactory> parameterFactories )
		{
			m_constructorInfo = constructorInfo;
			m_parameterFactories = parameterFactories.ToArray();
			m_factory = CreateConstructor();
		}

		private Func<IUntypedComponentFacilityOrFactory[], TComponent> CreateConstructor()
		{
			var factoryMethod = new DynamicMethod( "Factory", typeof(TComponent), new[] {typeof(IUntypedComponentFacilityOrFactory[])}, true );
			GenerateFactoryIL( factoryMethod, m_constructorInfo );
			return (Func<IUntypedComponentFacilityOrFactory[], TComponent>)factoryMethod.CreateDelegate( typeof(Func<IUntypedComponentFacilityOrFactory[], TComponent>) );
		}

		private static MethodInfo GetMethod<T, TResult>( Expression<Func<T, TResult>> m )
		{
			var mce = m.Body as MethodCallExpression;
			return mce != null ? mce.Method : null;
		}

		private static void GenerateFactoryIL( DynamicMethod m, ConstructorInfo constructor )
		{
			var il = m.GetILGenerator();
			var constructorParameters = constructor.GetParameters();
			for( var i = 0; i < constructorParameters.Length; i++ ) {
				il.Emit( OpCodes.Ldarg_0 );
				il.Emit( OpCodes.Ldc_I4, i );
				il.Emit( OpCodes.Ldelem_Ref );
				il.Emit( OpCodes.Callvirt, GetUntypedInstanceMethod );
				il.Emit( OpCodes.Castclass, constructorParameters[i].ParameterType );
			}
			il.Emit( OpCodes.Newobj, constructor );
			il.Emit( OpCodes.Ret );
		}

		private static MethodInfo m_getUntypedInstanceMethod;
		private static MethodInfo GetUntypedInstanceMethod
		{
			get { return m_getUntypedInstanceMethod ?? (m_getUntypedInstanceMethod = GetMethod<IUntypedComponentFacilityOrFactory, object>( f => f.GetUntypedInstance() )); }
		}

		public override TComponent CreateInstance()
		{
			return m_factory( m_parameterFactories );
		}

		public override void Dispose()
		{
		}
	}
}