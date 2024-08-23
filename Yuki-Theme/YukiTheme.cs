using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using VisualPascalABCPlugins;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme;

//имя класса *_VisualPascalABCPlugin
// ReSharper disable once InconsistentNaming
public class YukiTheme_VisualPascalABCPlugin : IVisualPascalABCPlugin
{
	private static YukiTheme_VisualPascalABCPlugin _instance;

	private static Stopwatch _timer;

	private readonly IDEAlterer _alterer;
	private bool _reload;

	public YukiTheme_VisualPascalABCPlugin(IWorkbench Workbench)
	{
		_instance = this;
		_alterer = new IDEAlterer(Workbench);
	}

	public static string GetCurrentFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
	public string Name => "Yuki Theme";

	public string Version => "1.0";

	public string Copyright => "Copyright © 2021-2023 by Dragon-LV";

	public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
	{
		var Item = new PluginGUIItem("Yuki Theme", "Yuki Theme", ResourceHelper.LoadImage("yuki128_2.png"), Color.Black, () => { });
		MenuItems.Add(Item);
		Console.WriteLine("Adding");
		_timer = new Stopwatch();
		_timer.Start();
		_alterer.Init();

		LinkCreator creator = new();
		if (creator.CheckLinks())
		{
			_reload = true;
			_alterer.Reload();
		}

		new WpfColorContainer();
		// ToolBarItems.Add(Item);
	}

	internal static void StopTimer()
	{
		if (_timer.IsRunning)
		{
			_timer.Stop();
			Console.WriteLine($"Elapsed time: {_timer.ElapsedMilliseconds} ms");

			if (_instance._reload)
				_instance._alterer.Reload();
		}
	}
}