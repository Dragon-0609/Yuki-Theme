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
			return API_Base.Current.Translate (name);
		}
	}
}