using System.Drawing;

namespace Yuki_Theme.Core.Interfaces;

public interface IColorUpdatable
{
	event ColorUpdate OnColorUpdate;
}

public delegate void ColorUpdate (Color bg, Color fg, Color bgClick);