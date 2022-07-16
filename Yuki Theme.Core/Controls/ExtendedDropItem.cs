namespace Yuki_Theme.Core.Controls
{
	public class ExtendedDropItem
	{
		private string name = "";

		public ExtendedDropItem (string accessName)
		{
			name = accessName;
		}

		public override string ToString ()
		{
			return API.Translate (name);
		}
	}
}