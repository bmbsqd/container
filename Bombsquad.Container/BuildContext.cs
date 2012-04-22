using System;
using System.Collections.Generic;

namespace Bombsquad.Container
{
	internal class BuildContext
	{
		private readonly IDictionary<ComponentKey, ComponentRegistration> m_registrations;
		private readonly Stack<Type> m_visited = new Stack<Type>();

		public BuildContext( IDictionary<ComponentKey, ComponentRegistration> registrations, BuildLog log )
		{
			m_registrations = registrations;
			Log = log;
		}

		public ComponentRegistration ResolveConstructorParameter( Type parameterType, string name )
		{
			ComponentRegistration registration;
			if( m_registrations.TryGetValue( new ComponentKey( parameterType, name ), out registration ) ) {
				return registration;
			}
			throw LogAndReturnException( new UnsatisfiedDependencyException( m_visited.Peek(), parameterType ) );
		}

		public IDisposable Visit<TComponent>()
		{
			var componentType = typeof(TComponent);
			if( m_visited.Contains( componentType ) ) {
				throw LogAndReturnException( new CircularComponentDependencyException( componentType ) );
			}
			m_visited.Push( componentType );
			return new Unvisitor( this );
		}

		public Exception LogAndReturnException( ContainerBuilderException exception )
		{
			Log.WriteLine( "Exception ({0}) : {1}", exception.GetType().Name, exception.Message );
			exception.BuildLog = Log.ToString();
			return exception;
		}

		public BuildLog Log { get; private set; }

		private class Unvisitor : IDisposable
		{
			private readonly BuildContext m_context;

			public Unvisitor( BuildContext context )
			{
				m_context = context;
			}

			public void Dispose()
			{
				m_context.m_visited.Pop();
			}
		}
	}
}