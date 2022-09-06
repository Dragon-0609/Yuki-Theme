using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using VisualPascalABC;
using WeifenLuo.WinFormsUI.Docking;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Interfaces;

namespace Yuki_Theme_Plugin
{
	internal class IconManager : IIconManager
	{
		private const    string           IconFolder = "Yuki_Theme_Plugin.Resources.icons";
		private readonly Form1            fm;
		private readonly MenuStrip        menu;
		private readonly ToolStrip        tools;
		private          ContextMenuStrip context;
		private          ContextMenuStrip context2;

		private readonly Dictionary <string, string> _iconNames = new ()
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
			{ "cmHelp", "help" },
		};

		private readonly string [] _iconsWithDarkVersion =
		{
			"menu-cut",
			"menu-saveall",
			"menu-paste",
			"copy",
			"addFile",
			"undo",
			"redo",
			"stepOut",
			"traceInto",
			"console",
			"intentionBulb",
			"magicResolve",
			"menu-open",
			"restartDebugger",
			"traceOver",
			"back",
			"forward",
			"print",
			"projectTab",
			"close",
			"find",
			"findForward",
			"replace",
			"moveToBottomLeft",
			"toolWindowMessages",
			"dynamicUsages",
			"showHiddens",
			"MoveTo2",
			"gearPlain",
			"showReadAccess",
			"showWriteAccess",
			"externalTools",
			"help"
		};

		private readonly string [] _iconsToDelete = { "miExit" };

		private bool internalchanges;

		public IconManager (ToolStrip toolStrip, MenuStrip menuStrip, ContextMenuStrip contextMenuStrip, ContextMenuStrip contextMenuStrip2,
		                    Form1     form)
		{
			tools = toolStrip;
			menu = menuStrip;
			fm = form;
			context = contextMenuStrip;
			context2 = contextMenuStrip2;
			Init ();
		}

		private void Init ()
		{
			foreach (ToolStripItem control in tools.Items)
				if (control is ToolStripButton btn)
				{
					btn.AccessibleDescription = GetIconName (btn.Name);
					if (btn.AccessibleDescription is { Length: > 2 })
					{
						btn.ImageTransparentColor = Color.Transparent;
						btn.EnabledChanged += BtnOnEnabledChanged;
					}
				}

			foreach (ToolStripMenuItem control in menu.Items)
			{
				foreach (ToolStripItem item in control.DropDownItems)
					if (item is ToolStripMenuItem btn)
					{
						btn.AccessibleDescription = GetIconName (btn.Name);
						if (btn.AccessibleDescription is { Length: > 2 })
						{
							btn.ImageTransparentColor = Color.Transparent;
							btn.EnabledChanged += MenuOnEnabledChanged;
						} else
							RemoveIcon (btn);
					}
			}

			foreach (ToolStripItem item in context.Items)
			{
				if (item is ToolStripMenuItem btn)
				{
					btn.AccessibleDescription = GetIconName (btn.Name);
					if (btn.AccessibleDescription is { Length: > 2 })
					{
						btn.ImageTransparentColor = Color.Transparent;
						btn.EnabledChanged += MenuOnEnabledChanged;
					} else
						RemoveIcon (btn);
				}
			}

			foreach (ToolStripItem item in context2.Items)
			{
				if (item is ToolStripMenuItem btn)
				{
					btn.AccessibleDescription = GetIconName (btn.Name);
					if (btn.AccessibleDescription is { Length: > 2 })
					{
						btn.ImageTransparentColor = Color.Transparent;
						btn.EnabledChanged += MenuOnEnabledChanged;
					} else
						RemoveIcon (btn);
				}
			}

			foreach (IDockContent content in fm.BottomPane.Contents)
				// MessageBox.Show (content.DockHandler.Form.Name);
				content.DockHandler.Form.AccessibleDescription = GetIconName (content.DockHandler.Form.Name);
		}

		private void BtnOnEnabledChanged (object sender, EventArgs e)
		{
			if (sender is ToolStripButton btn)
			{
				Tuple <bool, string> rest = GetState (btn);

				if (rest.Item1 || internalchanges)
				{
					Assembly a = Assembly.GetExecutingAssembly ();
					Helper.RenderSvg (btn, Helper.LoadSvg (btn.AccessibleDescription + rest.Item2, a, IconFolder),
					                  false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.Colors.bgBorder);
				}
			}
		}

		private void MenuOnEnabledChanged (object sender, EventArgs e)
		{
			if (sender is ToolStripMenuItem btn)
			{
				Tuple <bool, string> rest = GetState (btn);


				if (rest.Item1 || internalchanges)
				{
					Assembly a = Assembly.GetExecutingAssembly ();
					// MessageBox.Show (btn.Name);
					Helper.RenderSvg (btn, Helper.LoadSvg (btn.AccessibleDescription + rest.Item2, a, IconFolder),
					                  false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.Colors.bgBorder);
				}
			}
		}

		public void UpdateColors ()
		{
			internalchanges = true;
			foreach (ToolStripItem control in tools.Items)
				if (control is ToolStripButton { AccessibleDescription: { Length: > 2 } })
					BtnOnEnabledChanged (control, EventArgs.Empty);

			foreach (ToolStripMenuItem control in menu.Items)
			{
				foreach (ToolStripItem item in control.DropDownItems)
					if (item is ToolStripMenuItem { AccessibleDescription: { Length: > 2 } })
						MenuOnEnabledChanged (item, EventArgs.Empty);
			}


			foreach (ToolStripItem item in context.Items)
			{
				if (item is ToolStripMenuItem { AccessibleDescription: { Length: > 2 } }) 
					MenuOnEnabledChanged (item, EventArgs.Empty);
			}

			foreach (ToolStripItem item in context2.Items)
			{
				if (item is ToolStripMenuItem { AccessibleDescription: { Length: > 2 } }) 
					MenuOnEnabledChanged (item, EventArgs.Empty);
			}


			foreach (IDockContent content in fm.BottomPane.Contents)
				if (content.DockHandler.Form.AccessibleDescription is { Length: > 2 })
					UpdateIcon (content.DockHandler.Form);
			internalchanges = false;
		}

		private string GetIconName (string str)
		{
			string res = "";
			if (_iconNames.ContainsKey (str))
				res = _iconNames [str];
			return res;
		}

		private void UpdateIcon (Form btn)
		{
			string add = "";
			if (HasDark (btn.AccessibleDescription))
			{
				bool isDark = Helper.IsDark (YukiTheme_VisualPascalABCPlugin.Colors.bg);
				add = isDark ? "" : "_dark";
			}

			Assembly a = Assembly.GetExecutingAssembly ();
			// MessageBox.Show (btn.Name);
			Helper.RenderSvg (btn, Helper.LoadSvg (btn.AccessibleDescription + add, a, IconFolder),
			                  false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.Colors.bgBorder);
		}

		private bool HasDark (string str)
		{
			return _iconsWithDarkVersion.Contains (str);
		}

		private void RemoveIcon (ToolStripMenuItem mi)
		{
			if (needToDelete (mi.Name))
			{
				mi.Image?.Dispose ();
				mi.Image = null;
			}
		}

		private void RemoveIcon (ToolStripButton mi)
		{
			if (needToDelete (mi.Name))
			{
				mi.Image?.Dispose ();
				mi.Image = null;
			}
		}

		private bool needToDelete (string str)
		{
			return _iconsToDelete.Contains (str);
		}

		/// <summary>
		/// Get additional string for its state
		/// </summary>
		/// <param name="btn">Target</param>
		/// <returns></returns>
		private Tuple <bool, string> GetState (ToolStripButton btn)
		{
			return GetState (btn.AccessibleDescription, btn.Enabled);
		}

		/// <summary>
		/// Get additional string for its state
		/// </summary>
		/// <param name="btn">Target</param>
		/// <returns></returns>
		private Tuple <bool, string> GetState (ToolStripMenuItem btn)
		{
			return GetState (btn.AccessibleDescription, btn.Enabled);
		}

		/// <summary>
		/// Get additional string for its state
		/// </summary>
		/// <param name="accessibleDescription">Target</param>
		/// <param name="enabled">IsEnabled</param>
		/// <returns></returns>
		private Tuple <bool, string> GetState (string accessibleDescription, bool enabled)
		{
			string sad = "";
			bool asDark = HasDark (accessibleDescription);
			if (asDark)
			{
				bool isDark = enabled;

				if (isDark)
				{
					isDark = Helper.IsDark (YukiTheme_VisualPascalABCPlugin.Colors.bg);
					sad = isDark ? "" : "_dark";
				} else
				{
					sad = "_disabled"; // Here I show disabled icons.
				}
			}

			return new Tuple <bool, string> (asDark, sad);
		}

		public Image RenderToolBarItemImage (ToolStripItem btn)
		{
			bool asDark = HasDark (btn.AccessibleDescription);
			string dark = "";
			if (asDark)
			{
				bool isDark = Helper.IsDark (YukiTheme_VisualPascalABCPlugin.Colors.bg);
				dark = isDark ? "" : "_dark";
			}

			Assembly a = Assembly.GetExecutingAssembly ();
			return Helper.RenderSvg (btn.Size, Helper.LoadSvg (btn.AccessibleDescription + dark, a, IconFolder),
			                         false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.Colors.bgBorder);
		}
	}
}