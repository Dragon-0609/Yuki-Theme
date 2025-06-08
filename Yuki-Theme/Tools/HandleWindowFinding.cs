using System;
using System.Windows.Forms;
using VisualPascalABC;
using YukiTheme.Engine;

namespace YukiTheme.Tools
{
	public class HandleWindowFinding<T> where T : Form
	{
		private Action<Control> _onFound;
		private string _name;
		private string _menuItemName;
		private Action _onForceUpdate;
		private static Form1 MainForm => IDEAlterer.Instance.Form1;

		public HandleWindowFinding(string name, Action<Control> onFound, Action onForceUpdate)
		{
			_name = name;
			_onFound = onFound;
			_onForceUpdate = onForceUpdate;
		}

		public HandleWindowFinding(string name, string menuItemName, Action<Control> onFound, Action onForceUpdate)
		{
			_menuItemName = menuItemName;
			_name = name;
			_onFound = onFound;
			_onForceUpdate = onForceUpdate;
		}

		public void Find()
		{
			T window = MainForm.GetByReflection<T>(_name);

			if (window == null)
			{
				if (string.IsNullOrEmpty(_menuItemName))
				{
					Console.WriteLine($"No menu item was specified for {_name}");
					return;
				}

				ToolStripMenuItem menuItem = MainForm.GetByReflection<ToolStripMenuItem>(_menuItemName);
				menuItem.Click += GetWindow;
			}
			else
			{
				_onFound?.Invoke(window);
			}
		}

		private void GetWindow(object sender, EventArgs e)
		{
			MainForm.GetByReflection<ToolStripMenuItem>("tsDisassembly").Click -= GetWindow;

			T window = MainForm.GetByReflection<T>(_name);
			_onFound?.Invoke(window);
			_onForceUpdate?.Invoke();
		}
	}
}