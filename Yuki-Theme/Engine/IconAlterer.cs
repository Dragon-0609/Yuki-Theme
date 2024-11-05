using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VisualPascalABC;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class IconAlterer
{
	private static readonly Dictionary<string, string> _iconNames = new()
	{
		{ "tbNew", "addFile" },
		{ "miNew", "addFile" },
		{ "tbOpen", "menu-open" },
		{ "miOpen", "menu-open" },
		{ "tbSave", "menu-saveall" },
		{ "tbSaveAll", "menu-saveall" },
		{ "miSave", "menu-saveall" },
		{ "miSaveAll", "menu-saveall" },
		{ "cmSave", "menu-saveall" },
		{ "tsCut", "menu-cut" },
		{ "miCut", "menu-cut" },
		{ "cmCut", "menu-cut" },
		{ "tsCopy", "copy" },
		{ "miCopy", "copy" },
		{ "cmCopy", "copy" },
		{ "tsPaste", "menu-paste" },
		{ "miPaste", "menu-paste" },
		{ "cmPaste", "menu-paste" },
		{ "tsUndo", "undo" },
		{ "miUndo", "undo" },
		{ "tsRedo", "redo" },
		{ "miRedo", "redo" },
		{ "tsNavigBack", "back" },
		{ "miNavigBack", "back" },
		{ "tsNavigForw", "forward" },
		{ "miNavigForw", "forward" },
		{ "StartButton", "execute" },
		{ "miRun", "execute" },
		{ "cmRun", "execute" },
		{ "StartNoDebugButton", "runAll" },
		{ "miRunNoDebug", "runAll" },
		{ "stopButton", "suspend" },
		{ "miStop", "suspend" },
		{ "CompileButton", "compile" },
		{ "miBuild", "compile" },
		{ "StartDebugButton", "startDebugger" },
		{ "StopDebugButton", "restartDebugger" },
		{ "StepOverButton", "traceOver" },
		{ "mSTEPOVERToolStripMenuItem", "traceOver" },
		{ "StepIntoButton", "traceInto" },
		{ "mSTEPINToolStripMenuItem", "traceInto" },
		{ "StepOutButton", "stepOut" },
		{ "mSTEPToolStripMenuItem", "stepOut" },
		{ "ReCompileButton", "forceRefresh" },
		{ "miRebuild", "forceRefresh" },
		{ "tsOutputWindow", "console" },
		{ "tsShowOutputWindow", "console" },
		{ "OutputWindowForm", "console" },
		{ "tsAutoInsertCode", "intentionBulb" },
		{ "mAUTOINSERTToolStripMenuItem", "intentionBulb" },
		{ "toolStripButton1", "cwmPermissionEdit" },
		{ "tsFormat", "magicResolve" },
		{ "mFORMATToolStripMenuItem", "magicResolve" },
		{ "cmFormat", "magicResolve" },
		{ "miPrint", "print" },
		{ "miNewProject", "projectTab" },
		{ "miDelete", "close" },
		{ "miFind", "find" },
		{ "miFindNext", "findForward" },
		{ "miReplace", "replace" },
		{ "tsShowFindSymbolsResultWindow", "replace" },
		{ "FindSymbolsResultWindowForm", "replace" },
		{ "miOutputWindow", "moveToBottomLeft" },
		{ "tsShowErrorsListWindow", "notificationError" },
		{ "ErrorsListWindowForm", "notificationError" },
		{ "tsShowCompilerConsoleWindow", "toolWindowMessages" },
		{ "CompilerConsoleWindowForm", "toolWindowMessages" },
		{ "tsShowDebugVariablesListWindow", "dynamicUsages" },
		{ "DebugVariablesListWindowForm", "dynamicUsages" },
		{ "tsShowDebugWatchListWindow", "showHiddens" },
		{ "DebugWatchListWindowForm", "showHiddens" },
		{ "tsDisassembly", "MoveTo2" },
		{ "DisassemblyWindow", "MoveTo2" },
		{ "mOPTIONSToolStripMenuItem", "gearPlain" },
		{ "tsGotoDefinition", "showReadAccess" },
		{ "cmGotoDefinition", "showReadAccess" },
		{ "tsGotoRealization", "showWriteAccess" },
		{ "cmGotoRealization", "showWriteAccess" },
		{ "miGenerateRealization", "externalTools" },
		{ "cmGenerateRealization", "externalTools" },
		{ "tsHelp", "help" },
		{ "cmHelp", "help" }
	};

	private readonly Form1 _fm;

	private readonly string[] _iconsToDelete = { "miExit" };
	private readonly MenuStrip _menu;
	private readonly ToolStrip _tools;
	private readonly ContextMenuStrip _context;
	private readonly ContextMenuStrip _context2;

	private bool _internalchanges;

	internal IconAlterer(ToolStrip toolStrip, MenuStrip menuStrip, ContextMenuStrip contextMenuStrip,
		ContextMenuStrip contextMenuStrip2,
		Form1 form)
	{
		_tools = toolStrip;
		_menu = menuStrip;
		_fm = form;
		_context = contextMenuStrip;
		_context2 = contextMenuStrip2;
		Init();
	}

	private string[] IconsWithDarkVersion => IconRenderer._iconsWithDarkVersion;

	public static string[] IconNames => _iconNames.Select(i => i.Value).ToArray();

	private void Init()
	{
		foreach (ToolStripItem control in _tools.Items)
			if (control is ToolStripButton btn)
			{
				btn.AccessibleDescription = GetIconName(btn.Name);
				if (btn.AccessibleDescription is { Length: > 2 })
				{
					btn.ImageTransparentColor = Color.Transparent;
					btn.EnabledChanged += BtnOnEnabledChanged;
				}
			}

		foreach (ToolStripMenuItem control in _menu.Items)
		foreach (ToolStripItem item in control.DropDownItems)
			if (item is ToolStripMenuItem btn)
			{
				btn.AccessibleDescription = GetIconName(btn.Name);
				if (btn.AccessibleDescription is { Length: > 2 })
				{
					btn.ImageTransparentColor = Color.Transparent;
					btn.EnabledChanged += MenuOnEnabledChanged;
				}
				else
				{
					RemoveIcon(btn);
				}
			}

		foreach (ToolStripItem item in _context.Items)
			if (item is ToolStripMenuItem btn)
			{
				btn.AccessibleDescription = GetIconName(btn.Name);
				if (btn.AccessibleDescription is { Length: > 2 })
				{
					btn.ImageTransparentColor = Color.Transparent;
					btn.EnabledChanged += MenuOnEnabledChanged;
				}
				else
				{
					RemoveIcon(btn);
				}
			}

		foreach (ToolStripItem item in _context2.Items)
			if (item is ToolStripMenuItem btn)
			{
				btn.AccessibleDescription = GetIconName(btn.Name);
				if (btn.AccessibleDescription is { Length: > 2 })
				{
					btn.ImageTransparentColor = Color.Transparent;
					btn.EnabledChanged += MenuOnEnabledChanged;
				}
				else
				{
					RemoveIcon(btn);
				}
			}

		foreach (var content in _fm.BottomPane.Contents)
			// MessageBox.Show (content.DockHandler.Form.Name);
			content.DockHandler.Form.AccessibleDescription = GetIconName(content.DockHandler.Form.Name);
	}

	private void BtnOnEnabledChanged(object sender, EventArgs e)
	{
		if (sender is ToolStripButton btn)
		{
			var rest = GetState(btn);

			if (rest.Item1 || _internalchanges)
			{
				SvgRenderer.RenderSvg(btn, AquireContext(btn.AccessibleDescription, rest.Item2));
			}
		}
	}

	private void MenuOnEnabledChanged(object sender, EventArgs e)
	{
		if (sender is ToolStripMenuItem btn)
		{
			var rest = GetState(btn);


			if (rest.Item1 || _internalchanges)
			{
				SvgRenderer.RenderSvg(btn, AquireContext(btn.AccessibleDescription, rest.Item2));
			}
		}
	}

	private static SvgRenderInfo AquireContext(string description, string rest)
	{
		return new SvgRenderInfo(SvgRenderer.LoadSvg(description + rest), false, Size.Empty, true,
			ColorReference.BorderColor);
	}

	internal void UpdateColors()
	{
		_internalchanges = true;
		foreach (ToolStripItem control in _tools.Items)
			if (control is ToolStripButton { AccessibleDescription: { Length: > 2 } })
				BtnOnEnabledChanged(control, EventArgs.Empty);
		foreach (ToolStripMenuItem control in _menu.Items)
		foreach (ToolStripItem item in control.DropDownItems)
			if (item is ToolStripMenuItem { AccessibleDescription: { Length: > 2 } })
				MenuOnEnabledChanged(item, EventArgs.Empty);

		foreach (ToolStripItem item in _context.Items)
			if (item is ToolStripMenuItem { AccessibleDescription: { Length: > 2 } })
				MenuOnEnabledChanged(item, EventArgs.Empty);

		foreach (ToolStripItem item in _context2.Items)
			if (item is ToolStripMenuItem { AccessibleDescription: { Length: > 2 } })
				MenuOnEnabledChanged(item, EventArgs.Empty);


		foreach (var content in _fm.BottomPane.Contents)
			if (content.DockHandler.Form.AccessibleDescription is { Length: > 2 })
				UpdateIcon(content.DockHandler.Form);

		_internalchanges = false;
	}

	private string GetIconName(string str)
	{
		var res = "";
		if (_iconNames.ContainsKey(str))
			res = _iconNames[str];
		return res;
	}

	private void UpdateIcon(Form btn)
	{
		var add = "";
		if (HasDark(btn.AccessibleDescription))
		{
			var isDark = ColorHelper.IsDark(ColorReference.BackgroundColor);
			add = isDark ? "" : "_dark";
		}

		SvgRenderer.RenderSvg(btn, AquireContext(btn.AccessibleDescription, add));
	}

	private bool HasDark(string str)
	{
		return IconsWithDarkVersion.Contains(str);
	}

	private void RemoveIcon(ToolStripMenuItem mi)
	{
		if (needToDelete(mi.Name))
		{
			mi.Image?.Dispose();
			mi.Image = null;
		}
	}

	private void RemoveIcon(ToolStripButton mi)
	{
		if (needToDelete(mi.Name))
		{
			mi.Image?.Dispose();
			mi.Image = null;
		}
	}

	private bool needToDelete(string str)
	{
		return _iconsToDelete.Contains(str);
	}

	/// <summary>
	///     Get additional string for its state
	/// </summary>
	/// <param name="btn">Target</param>
	/// <returns></returns>
	private Tuple<bool, string> GetState(ToolStripButton btn)
	{
		return GetState(btn.AccessibleDescription, btn.Enabled);
	}

	/// <summary>
	///     Get additional string for its state
	/// </summary>
	/// <param name="btn">Target</param>
	/// <returns></returns>
	private Tuple<bool, string> GetState(ToolStripMenuItem btn)
	{
		return GetState(btn.AccessibleDescription, btn.Enabled);
	}

	/// <summary>
	///     Get additional string for its state
	/// </summary>
	/// <param name="accessibleDescription">Target</param>
	/// <param name="enabled">IsEnabled</param>
	/// <returns></returns>
	private Tuple<bool, string> GetState(string accessibleDescription, bool enabled)
	{
		var sad = "";
		var asDark = HasDark(accessibleDescription);
		if (asDark)
		{
			var isDark = enabled;

			if (isDark)
			{
				isDark = ColorHelper.IsDark(ColorReference.BackgroundColor);
				sad = isDark ? "" : "_dark";
			}
			else
			{
				sad = "_disabled"; // Here I show disabled icons.
			}
		}

		return new Tuple<bool, string>(asDark, sad);
	}
}