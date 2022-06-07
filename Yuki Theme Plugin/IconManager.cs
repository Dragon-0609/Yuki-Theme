using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VisualPascalABC;
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

		private bool internalchanges;

		public IconManager (ToolStrip toolStrip, MenuStrip menuStrip, ContextMenuStrip contextMenuStrip, ContextMenuStrip contextMenuStrip2, Form1 form)
		{
			tools = toolStrip;
			menu = menuStrip;
			fm = form;
			context = contextMenuStrip;
			context2 = contextMenuStrip2;
			// tools.RightToLeft = RightToLeft.Yes;
			Init ();
		}

		public void Init ()
		{
			foreach (var control in tools.Items)
				if (control is ToolStripButton)
				{
					var btn = (ToolStripButton)control;
					btn.AccessibleDescription = GetIconName (btn.Name);
					if (btn.AccessibleDescription != null && btn.AccessibleDescription.Length > 2)
					{
						btn.ImageTransparentColor = Color.Transparent;
						btn.EnabledChanged += BtnOnEnabledChanged;
					}
				}

			foreach (ToolStripMenuItem control in menu.Items)
			{
				foreach (var item in control.DropDownItems)
					if (item is ToolStripMenuItem)
					{
						var btn = (ToolStripMenuItem)item;
						btn.AccessibleDescription = GetIconName (btn.Name);
						if (btn.AccessibleDescription != null && btn.AccessibleDescription.Length > 2)
						{
							btn.ImageTransparentColor = Color.Transparent;
							btn.EnabledChanged += MenuOnEnabledChanged;
						} else
							RemoveIcon (btn);
					}
			}
			
			foreach (ToolStripItem item in context.Items)
			{
				if (item is ToolStripMenuItem)
				{
					var btn = (ToolStripMenuItem)item;
					btn.AccessibleDescription = GetIconName (btn.Name);
					if (btn.AccessibleDescription != null && btn.AccessibleDescription.Length > 2)
					{
						btn.ImageTransparentColor = Color.Transparent;
						btn.EnabledChanged += MenuOnEnabledChanged;
					} else
						RemoveIcon (btn);
				}
			}
			
			foreach (ToolStripItem item in context2.Items)
			{
				if (item is ToolStripMenuItem)
				{
					var btn = (ToolStripMenuItem)item;
					btn.AccessibleDescription = GetIconName (btn.Name);
					if (btn.AccessibleDescription != null && btn.AccessibleDescription.Length > 2)
					{
						btn.ImageTransparentColor = Color.Transparent;
						btn.EnabledChanged += MenuOnEnabledChanged;
					} else
						RemoveIcon (btn);
				}
			}
			
			foreach (var content in fm.BottomPane.Contents)
				// MessageBox.Show (content.DockHandler.Form.Name);
				content.DockHandler.Form.AccessibleDescription = GetIconName (content.DockHandler.Form.Name);
		}

		private void BtnOnEnabledChanged (object sender, EventArgs e)
		{
			if (sender is ToolStripButton)
			{
				ToolStripButton btn = (ToolStripButton)sender;
				Tuple <bool, string> rest = GetState (btn);

				if (rest.Item1 || internalchanges)
				{
					Assembly a = Assembly.GetExecutingAssembly ();
					Helper.RenderSvg (btn, Helper.LoadSvg (btn.AccessibleDescription + rest.Item2, a, IconFolder),
					                  false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.bgBorder);
				}
			}
		}

		private void MenuOnEnabledChanged (object sender, EventArgs e)
		{
			if (sender is ToolStripMenuItem)
			{
				ToolStripMenuItem btn = (ToolStripMenuItem)sender;
				Tuple <bool, string> rest = GetState (btn);


				if (rest.Item1 || internalchanges)
				{
					var a = Assembly.GetExecutingAssembly ();
					// MessageBox.Show (btn.Name);
					Helper.RenderSvg (btn, Helper.LoadSvg (btn.AccessibleDescription + rest.Item2, a, IconFolder),
					                  false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.bgBorder);
				}
			}
		}

		public void UpdateColors ()
		{
			internalchanges = true;
			foreach (var control in tools.Items)
				if (control is ToolStripButton)
					if (((ToolStripButton)control).AccessibleDescription != null &&
					    ((ToolStripButton)control).AccessibleDescription.Length > 2)
						BtnOnEnabledChanged (control, EventArgs.Empty);

			foreach (ToolStripMenuItem control in menu.Items)
			{
				foreach (var item in control.DropDownItems)
					if (item is ToolStripMenuItem)
						if (((ToolStripMenuItem)item).AccessibleDescription != null &&
						    ((ToolStripMenuItem)item).AccessibleDescription.Length > 2)
							MenuOnEnabledChanged (item, EventArgs.Empty);
			}

			
			foreach (ToolStripItem item in context.Items)
			{
				if (item is ToolStripMenuItem)
				{
					if (((ToolStripMenuItem)item).AccessibleDescription != null &&
					    ((ToolStripMenuItem)item).AccessibleDescription.Length > 2)
						MenuOnEnabledChanged (item, EventArgs.Empty);
				}
			}
			
			foreach (ToolStripItem item in context2.Items)
			{
				if (item is ToolStripMenuItem)
				{
					if (((ToolStripMenuItem)item).AccessibleDescription != null &&
					    ((ToolStripMenuItem)item).AccessibleDescription.Length > 2)
						MenuOnEnabledChanged (item, EventArgs.Empty);
				}
			}


			foreach (var content in fm.BottomPane.Contents)
				if (content.DockHandler.Form.AccessibleDescription != null &&
				    content.DockHandler.Form.AccessibleDescription.Length > 2)
					UpdateIcon (content.DockHandler.Form);
			internalchanges = false;
		}

		private string GetIconName (string str)
		{
			var res = "";

			switch (str)
			{
				case "tbNew" :
				case "miNew" :
				{
					res = "addFile";
				}
					break;

				case "tbOpen" :
				case "miOpen" :
				{
					res = "menu-open";
				}
					break;

				case "tbSave" :
				case "tbSaveAll" :
				case "miSave" :
				case "miSaveAll" :
				case "cmSave" :
				{
					res = "menu-saveall";
				}
					break;

				case "tsCut" :
				case "miCut" :
				case "cmCut" :
				{
					res = "menu-cut";
				}
					break;

				case "tsCopy" :
				case "miCopy" :
				case "cmCopy" :
				{
					res = "copy";
				}
					break;

				case "tsPaste" :
				case "miPaste" :
				case "cmPaste" :
				{
					res = "menu-paste";
				}
					break;

				case "tsUndo" :
				case "miUndo" :
				{
					res = "undo";
				}
					break;

				case "tsRedo" :
				case "miRedo" :
				{
					res = "redo";
				}
					break;

				case "tsNavigBack" :
				case "miNavigBack" :
				{
					res = "back";
				}
					break;

				case "tsNavigForw" :
				case "miNavigForw" :
				{
					res = "forward";
				}
					break;

				case "StartButton" :
				case "miRun" :
				case "cmRun" :
				{
					res = "execute";
				}
					break;

				case "StartNoDebugButton" :
				case "miRunNoDebug" :
				{
					res = "runAll";
				}
					break;

				case "stopButton" :
				case "miStop" :
				{
					res = "suspend";
				}
					break;

				case "CompileButton" :
				case "miBuild" :
				{
					res = "compile";
				}
					break;

				case "StartDebugButton" :
				{
					res = "startDebugger";
				}
					break;

				case "StopDebugButton" :
				{
					res = "restartDebugger";
				}
					break;

				case "StepOverButton" :
				case "mSTEPOVERToolStripMenuItem" :
				{
					res = "traceOver";
				}
					break;

				case "StepIntoButton" :
				case "mSTEPINToolStripMenuItem" :
				{
					res = "traceInto";
				}
					break;

				case "StepOutButton" :
				case "mSTEPToolStripMenuItem" :
				{
					res = "stepOut";
				}
					break;

				case "ReCompileButton" :
				case "miRebuild" :
				{
					res = "forceRefresh";
				}
					break;

				case "tsOutputWindow" :
				case "tsShowOutputWindow" :
				case "OutputWindowForm" :
				{
					res = "console";
				}
					break;

				case "tsAutoInsertCode" :
				case "mAUTOINSERTToolStripMenuItem" :
				{
					res = "intentionBulb";
				}
					break;

				case "toolStripButton1" :
				{
					res = "cwmPermissionEdit";
				}
					break;

				case "tsFormat" :
				case "mFORMATToolStripMenuItem" :
				case "cmFormat" :
				{
					res = "magicResolve";
				}
					break;

				case "miPrint" :
				{
					res = "print";
				}
					break;

				case "miNewProject" :
				{
					res = "projectTab";
				}
					break;

				case "miDelete" :
				{
					res = "close";
				}
					break;

				case "miFind" :
				{
					res = "find";
				}
					break;

				case "miFindNext" :
				{
					res = "findForward";
				}
					break;

				case "miReplace" :
				case "tsShowFindSymbolsResultWindow" :
				case "FindSymbolsResultWindowForm" :
				{
					res = "replace";
				}
					break;

				case "miOutputWindow" :
				{
					res = "moveToBottomLeft";
				}
					break;

				case "tsShowErrorsListWindow" :
				case "ErrorsListWindowForm" :
				{
					res = "notificationError";
				}
					break;

				case "tsShowCompilerConsoleWindow" :
				case "CompilerConsoleWindowForm" :
				{
					res = "toolWindowMessages";
				}
					break;

				case "tsShowDebugVariablesListWindow" :
				case "DebugVariablesListWindowForm" :
				{
					res = "dynamicUsages";
				}
					break;

				case "tsShowDebugWatchListWindow" :
				case "DebugWatchListWindowForm" :
				{
					res = "showHiddens";
				}
					break;

				case "tsDisassembly" :
				case "DisassemblyWindow" :
				{
					res = "MoveTo2";
				}
					break;

				case "mOPTIONSToolStripMenuItem" :
				{
					res = "gearPlain";
				}
					break;

				case "tsGotoDefinition" :
				case "cmGotoDefinition" :
				{
					res = "showReadAccess";
				}
					break;

				case "tsGotoRealization" :
				case "cmGotoRealization" :
				{
					res = "showWriteAccess";
				}
					break;

				case "miGenerateRealization" :
				case "cmGenerateRealization" :
				{
					res = "externalTools";
				}
					break;

				case "tsHelp" :
				case "cmHelp" :
				{
					res = "help";
				}
					break;

				/*
			case "" :
			{
				res = "";
			}
				break;
				*/
				default :
				{
					res = "";
				}
					break;
			}

			return res;
		}

		private void UpdateIcon (Form btn)
		{
			var add = "";
			if (hasDark (btn.AccessibleDescription))
			{
				var isDark = Helper.IsDark (YukiTheme_VisualPascalABCPlugin.bg);
				add = isDark ? "" : "_dark";
			}

			var a = Assembly.GetExecutingAssembly ();
			// MessageBox.Show (btn.Name);
			Helper.RenderSvg (btn, Helper.LoadSvg (btn.AccessibleDescription + add, a, IconFolder),
			                  false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.bgBorder);
		}

		private bool hasDark (string str)
		{
			var s = false;
			switch (str)
			{
				case "menu-cut" :
				case "menu-saveall" :
				case "menu-paste" :
				case "copy" :
				case "addFile" :
				case "undo" :
				case "redo" :
				case "stepOut" :
				case "traceInto" :
				case "console" :
				case "intentionBulb" :
				case "magicResolve" :
				case "menu-open" :
				case "restartDebugger" :
				case "traceOver" :
				case "back" :
				case "forward" :
				case "print" :
				case "projectTab" :
				case "close" :
				case "find" :
				case "findForward" :
				case "replace" :
				case "moveToBottomLeft" :
				case "toolWindowMessages" :
				case "dynamicUsages" :
				case "showHiddens" :
				case "MoveTo2" :
				case "gearPlain" :
				case "showReadAccess" :
				case "showWriteAccess" :
				case "externalTools" :
				case "help" :
				{
					s = true;
				}
					break;
			}

			return s;
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
			var res = false;
			switch (str)
			{
				case "miExit" :
				{
					res = true;
				}
					break;
			}

			return res;
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
			bool asDark = hasDark (accessibleDescription);
			if (asDark)
			{
				bool isDark = enabled;

				if (isDark)
				{
					isDark = Helper.IsDark (YukiTheme_VisualPascalABCPlugin.bg);
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
			bool asDark = hasDark (btn.AccessibleDescription);
			string dark = "";
			if (asDark)
			{
				bool isDark = Helper.IsDark (YukiTheme_VisualPascalABCPlugin.bg);
				dark = isDark ? "" : "_dark";
			}
			
			Assembly a = Assembly.GetExecutingAssembly ();
			return Helper.RenderSvg (btn.Size, Helper.LoadSvg (btn.AccessibleDescription + dark, a, IconFolder),
			                         false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.bgBorder);

		}
	}
}