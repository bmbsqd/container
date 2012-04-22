using System;

namespace Bombsquad.Container.Tests.Fakes
{
	public class FakeComponentScope<T> : IComponentScope<T>
	{
		public static int NumberOfInstancesCreated = 0;

		public T GetOrCreateInstance( Func<T> factory )
		{
			NumberOfInstancesCreated++;
			return factory();
		}

		public void Dispose()
		{
		}
	}
}