using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VisualPascalABCPlugins;
using YukiTheme.Engine.Processors;

namespace YukiTheme.Engine
{
	public class PluginInjector
	{
		private MenuStrip _menu;
		private Timer _loadingTimer;

		private event Action OnColorUpdate;

		private InjectorProcessor[] _processors;

		private bool _inited = false;

		public void InjectWithDelay(MenuStrip menu)
		{
			_menu = menu;
			Init();
			StartLoadingTimer();
		}

		private void Init()
		{
			if (_inited) return;
			_inited = true;

			_processors =
			[
				new CodeTemplateProcessor(),
				new CompilerControllerProcessor(),
				new InternalErrorReportProcessor(),
				new LanguageConverterProcessor(),
			];
			foreach (var processor in _processors)
			{
				OnColorUpdate += processor.OnColorUpdate;
			}
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
					InjectorProcessor processor = _processors.FirstOrDefault(p => plugin.Text.StartsWith(p.Name));
					if (processor != null)
					{
						processor.Process(item);
					}
				}
			}
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
			return _processors.Any(p => plugin.Text.Contains(p.Name));
		}

		private void UpdateColors()
		{
			OnColorUpdate?.Invoke();
		}
	}
}