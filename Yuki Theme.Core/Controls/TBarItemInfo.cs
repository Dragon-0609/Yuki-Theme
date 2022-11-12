using System;
using System.Drawing;

namespace Yuki_Theme.Core.Controls
{
	[Serializable]
	public class TBarItemInfo
	{
		public bool   IsVisible;
		public bool   IsRight;
		public string Name;
		public string AccessibleName;
		public string Text;
		public Size   Size;

		public TBarItemInfo (string name, string accessibleName, string text, bool isVisible, bool isRight, Size size)
		{
			Name = name;
			AccessibleName = accessibleName;
			Text = text;
			IsVisible = isVisible;
			IsRight = isRight;
			Size = size;
		}
	}
}