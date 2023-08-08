using System;
using System.Windows.Interop;
using YukiTheme.Style.Helpers;

namespace YukiTheme.Style.Controls
{
	public partial class GridWindow : SnapWindow
	{
		public GridWindow()
		{
			InitializeComponent();
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			var hwnd = new WindowInteropHelper(this).Handle;
			WindowsServices.SetWindowExTransparent(hwnd);
		}
	}
}