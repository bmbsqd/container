namespace Bombsquad.Container.Tests.Fakes
{
	public class DualConstructorFakeComponent : IFakeComponentWithValue<int>
	{
		private readonly int m_value;

		public DualConstructorFakeComponent()
		{
		}

		public DualConstructorFakeComponent( int value )
		{
			m_value = value;
		}

		public int Value
		{
			get { return m_value; }
		}
	}
}