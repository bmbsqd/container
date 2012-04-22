using System.Collections;
using System.Web;

namespace Bombsquad.Container.Web.Tests
{
	public class FakeHttpContext : HttpContextBase
	{
		public readonly IDictionary m_items = new Hashtable();

		public override IDictionary Items
		{
			get
			{
				return m_items;
			}
		}
	}
}