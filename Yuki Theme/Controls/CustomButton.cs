using System;
using System.Windows.Forms;

namespace Yuki_Theme.Controls
{
	public class CustomButton : Button
	{
		public void ClickHere (EventArgs e)
		{
			OnClick (e);
		}
	}
}