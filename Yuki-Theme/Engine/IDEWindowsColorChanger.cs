using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualPascalABC;
using YukiTheme.Tools;

namespace YukiTheme.Engine
{
	// ReSharper disable once InconsistentNaming
	public class IDEWindowsColorChanger
	{
		private List<Control> _controlsToUpdate = new();
		private HandleWindowFinding<DisassemblyWindow> _disassemblyFinder;
		private HandleWindowFinding<FindSymbolsResultWindowForm> _symbolFinder;
		private HandleWindowFinding<ImmediateWindow> _immediateFinder;
		private HandleWindowFinding<BottomDockContentForm> _variableFinder;
		private HandleWindowFinding<DebugWatchListWindowForm> _watchFinder;


		public void Subscribe()
		{
			ColorChanger.Instance.UpdatedColors += UpdateColors;
			_disassemblyFinder = new("DisassemblyWindow", "tsDisassembly", AddToUpdate, UpdateColors);
			_symbolFinder = new("FindSymbolsResultWindow", AddToUpdate, UpdateColors);
			_immediateFinder = new("ImmediateWindow", AddToUpdate, UpdateColors);
			_variableFinder = new("DebugVariablesListWindow", AddToUpdate, UpdateColors);
			_watchFinder = new("DebugWatchListWindow", AddToUpdate, UpdateColors);

			_disassemblyFinder.Find();
			_symbolFinder.Find();
			_immediateFinder.Find();
			_variableFinder.Find();
			_watchFinder.Find();
		}

		private void AddToUpdate(Control target)
		{
			_controlsToUpdate.Add(target);
		}

		private void UpdateColors()
		{
			foreach (var control in _controlsToUpdate)
			{
				ColorHelper.SetColors(control, false, null);
			}
		}
	}
}