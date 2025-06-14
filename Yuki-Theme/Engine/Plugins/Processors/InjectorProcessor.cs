using System;
using System.Windows.Forms;
using VisualPascalABCPlugins;

namespace YukiTheme.Engine.Processors
{
	public abstract class InjectorProcessor
	{
		private ToolStripMenuItem _item;
		public abstract string Name { get; }

		protected virtual bool InjectImmediate => false;

		protected event Action ColorUpdated;

		protected abstract void Process(PluginGUIItem plugin);

		public void Process(ToolStripMenuItem item)
		{
			_item = item;
			if (InjectImmediate)
			{
				Process(_item.Tag as PluginGUIItem);
			}
			else
			{
				item.Click += Clicked;
			}
		}

		private void Clicked(object sender, EventArgs e)
		{
			if (_item.Tag is not PluginGUIItem plugin) return;

			_item.Click -= Clicked;
			Process(plugin);
		}

		public void OnColorUpdate()
		{
			ColorUpdated?.Invoke();
		}
	}
}