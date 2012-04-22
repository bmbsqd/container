namespace Bombsquad.Container.Tests.Fakes
{
	public class ManyConstructorsFakeComponent : IFakeComponentWithValue<int>
	{
		private readonly int m_value;

		public ManyConstructorsFakeComponent()
		{
		}

		public ManyConstructorsFakeComponent( int value )
		{
			m_value = value;
		}

		public ManyConstructorsFakeComponent( string value )
		{
		}

		public ManyConstructorsFakeComponent( int value, string otherValue )
		{
		}

		public int Value
		{
			get { return m_value; }
		}
	}
}