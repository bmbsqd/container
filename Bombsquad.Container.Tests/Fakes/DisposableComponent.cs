using System;

namespace Bombsquad.Container.Tests.Fakes
{
	public class DisposableComponent : IDisposable
	{
		public static bool DisposeWasCalled;

		public void Dispose()
		{
			DisposeWasCalled = true;
		}
	}
}
