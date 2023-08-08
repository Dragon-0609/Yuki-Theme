using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using VisualPascalABCPlugins;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme
{
	//имя класса *_VisualPascalABCPlugin
	// ReSharper disable once InconsistentNaming
	public class YukiTheme_VisualPascalABCPlugin : IVisualPascalABCPlugin
	{
		private static Stopwatch _timer;

		private IDEAlterer _alterer;
		public string Name => "Yuki Theme";

		public string Version => "1.0";

		public string Copyright => "Copyright © 2021-2023 by Dragon-LV";

		public static string GetCurrentFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		public void Execute()
		{
			Console.WriteLine("Test");
		}

		internal static void StopTimer()
		{
			if (_timer.IsRunning)
			{
				_timer.Stop();
				Console.WriteLine($"Elapsed time: {_timer.ElapsedMilliseconds} ms");
			}
		}

		public YukiTheme_VisualPascalABCPlugin(IWorkbench Workbench)
		{
			Console.WriteLine("Initing Yuki Theme");
			_alterer = new IDEAlterer(Workbench);
		}

		public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
		{
			Console.WriteLine("Adding Yuki Theme");
			PluginGUIItem Item = new PluginGUIItem("Yuki Theme", "Yuki Theme", ImageHelper.Load("yuki128_2.png"), Color.Black, Execute);
			MenuItems.Add(Item);
			_timer = new Stopwatch();
			_timer.Start();
			_alterer.Init();

			// ToolBarItems.Add(Item);
		}
	}
}