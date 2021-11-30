using System;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class CustomButton : Button
	{
		public void ClickHere (EventArgs e)
		{
			OnClick (e);
		}
	}
}