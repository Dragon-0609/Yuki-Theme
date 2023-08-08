using System.Windows;
using System.Windows.Media;
using YukiTheme.Style.Controls;
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
			borderOutlineX = 0,
			borderOutlineY = 0,
		};
		var targetForm = IDEAlterer.Alterer.Form1;
		_grid.SetOwner(targetForm);
		_grid.Foreground = ColorReference.BorderColor().ToWPFColor().ToBrush();
		_grid.Width = targetForm.Width;
		_grid.Height = targetForm.Height;

		_grid.ResetPosition();
		_grid.Show();
		_grid.Focus();
		IDEAlterer.Alterer.Form1.Focus();
		ColorChanger.Changer.UpdatedColors += UpdateColors;
	}

	public void UpdateColors()
	{
		if (_grid != null)
		{
			_grid.Foreground = ColorReference.BorderColor().ToWPFColor().ToBrush();
		}
	}
}