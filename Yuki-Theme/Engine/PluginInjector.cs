using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CodeTemplatesPlugin;
using Fluent;
using Fluent.Lists;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;
using YukiTheme.Tools;

namespace YukiTheme.Engine
{
	public class PluginInjector
	{
		private const string CODE_TEMPLATES_PLUGIN = "VPP_CODE_TEMPLATES_PLUGIN";
		private MenuStrip _menu;
		private Timer _loadingTimer;

		private string[] _pluginsToInject =
		[
			CODE_TEMPLATES_PLUGIN
		];

		private event Action OnColorUpdate;

		private ToolStripMenuItem _codeTemplates;
		private DockContent _ctForm;

		public void InjectWithDelay(MenuStrip menu)
		{
			_menu = menu;
			StartLoadingTimer();
		}

		private void StartLoadingTimer()
		{
			_loadingTimer = new Timer { Interval = 100 };
			_loadingTimer.Tick += Load;
			_loadingTimer.Start();
		}

		private void Load(object sender, EventArgs e)
		{
			_loadingTimer.Stop();
			ColorChanger.Instance.UpdatedColors += UpdateColors;
			Inject();
		}

		private void Inject()
		{
			Console.WriteLine($"Injecting to plugins");
			List<ToolStripMenuItem> toProcessLater = FindPluginsToInject();

			if (toProcessLater.Any())
			{
				Inject(toProcessLater);
			}
			else
			{
				Console.WriteLine("Plugins to inject were not found");
			}
		}

		private void Inject(List<ToolStripMenuItem> toProcessLater)
		{
			foreach (ToolStripMenuItem item in toProcessLater)
			{
				if (item.Tag is PluginGUIItem plugin)
				{
					if (plugin.Text.StartsWith(CODE_TEMPLATES_PLUGIN))
					{
						InjectToCodeTemplates(item);
					}
				}
			}
		}

		private void InjectToCodeTemplates(ToolStripMenuItem item)
		{
			_codeTemplates = item;
			item.Click += CodeTemplatesClicked;
		}

		private void CodeTemplatesClicked(object sender, EventArgs e)
		{
			if (_codeTemplates.Tag is not PluginGUIItem plugin) return;

			_codeTemplates.Click -= CodeTemplatesClicked;

			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

			PluginGUIItemExecuteDelegate execute =
				(PluginGUIItemExecuteDelegate)plugin.GetType().GetField("executeDelegate", flags).GetValue(plugin);

			object pluginCore = execute.Target;

			Console.WriteLine($"Plugin Core: {pluginCore.GetType()}");

			_ctForm = (DockContent)pluginCore.GetType().GetField("ctForm", flags).GetValue(pluginCore);


			_ctForm.Closed += ClosedCtForm;
			OnColorUpdate += UpdateCtColors;
			UpdateCtColors();

			_ctForm.Hide();
			_ctForm.Show();
		}

		private void ClosedCtForm(object sender, EventArgs e)
		{
			OnColorUpdate -= UpdateCtColors;
		}

		private void UpdateCtColors()
		{
			Console.WriteLine($"Updating ct colors");
			ColorHelper.SetColors(_ctForm, true, null);
		}

		private List<ToolStripMenuItem> FindPluginsToInject()
		{
			List<ToolStripMenuItem> toProcessLater = new List<ToolStripMenuItem>();
			foreach (ToolStripItem menuItem in _menu.Items)
			{
				if (menuItem is ToolStripMenuItem item)
				{
					foreach (ToolStripItem toolStripMenuItem in item.DropDownItems)
					{
						if (toolStripMenuItem is ToolStripMenuItem { Tag: PluginGUIItem plugin } menu)
						{
							if (IsPluginToInject(plugin))
							{
								Console.WriteLine($"Adding {menu.Text} to injected plugins");
								toProcessLater.Add(menu);
							}
						}
					}
				}
			}

			return toProcessLater;
		}

		private bool IsPluginToInject(PluginGUIItem plugin)
		{
			return _pluginsToInject.Any(p => plugin.Text.Contains(p));
		}

		private void UpdateColors()
		{
			OnColorUpdate?.Invoke();
		}
	}
}