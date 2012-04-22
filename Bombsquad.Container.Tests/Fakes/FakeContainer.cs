using System;

namespace Bombsquad.Container.Tests.Fakes
{
	public class FakeContainer : IContainer
	{
		public TComponent Resolve<TComponent>()
		{
			throw new NotImplementedException();
		}

		public object Resolve( Type type, string name )
		{
			throw new NotImplementedException();
		}

		public TComponent Resolve<TComponent>( string name )
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
