namespace Bombsquad.Container
{
	internal class ValueComponentRegistration<TComponent> : ComponentRegistration<TComponent>
	{
		private readonly TComponent m_value;

		public ValueComponentRegistration( TComponent value, string name ) : base( name )
		{
			m_value = value;
		}

		protected override ComponentFactory<TComponent> CreateComponentFactory( BuildContext context )
		{
			using ( context.Log.BeginScope( "Value: {0}", m_value ) )
			{
				return new ValueComponentFactory<TComponent>( m_value );
			}
		}
	}
}
