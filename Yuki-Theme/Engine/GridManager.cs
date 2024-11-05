using System.Windows.Media;
using YukiTheme.Components;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class GridManager
{
	private GridWindow _grid;

	public void Show()
	{
		_grid = new GridWindow
		{
			AlignX = AlignmentX.Left,
			AlignY = AlignmentY.Top,
			BorderOutlineX = 0,
			BorderOutlineY = 0
		};
		var targetForm = IDEAlterer.Instance.Form1;
		_grid.SetOwner(targetForm);
		_grid.Foreground = ColorReference.BorderColor.ToWPFColor().ToBrush();
		_grid.Width = targetForm.Width;
		_grid.Height = targetForm.Height;

		_grid.ResetPosition();
		_grid.Show();
		_grid.Focus();
		IDEAlterer.Instance.Form1.Focus();
		ColorChanger.Instance.UpdatedColors += UpdateColors;
	}

	private void UpdateColors()
	{
		if (_grid != null) _grid.Foreground = ColorReference.BorderColor.ToWPFColor().ToBrush();
	}
}