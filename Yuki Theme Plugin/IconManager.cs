using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VisualPascalABC;
using Yuki_Theme.Core;

namespace Yuki_Theme_Plugin
{
	class IconManager
	{
		private       ToolStrip tools;
		private       MenuStrip menu;
		private const string    IconFolder = "Yuki_Theme_Plugin.Resources.icons";

		public IconManager (ToolStrip toolStrip, MenuStrip menuStrip)
		{
			tools = toolStrip;
			menu = menuStrip;
			Init ();
		}

		public void Init ()
		{
			foreach (var control in tools.Items)
			{
				if (control is ToolStripButton)
				{
					ToolStripButton btn = (ToolStripButton) control;
					btn.AccessibleDescription = GetIconName (btn.Name);
					if (btn.AccessibleDescription != null && btn.AccessibleDescription.Length > 2)
						btn.ImageTransparentColor = Color.Transparent;
				}
			}
			
			foreach (ToolStripMenuItem control in menu.Items)
			{
				foreach (var item in control.DropDownItems)
				{
					if (item is ToolStripMenuItem)
					{
						ToolStripMenuItem btn = (ToolStripMenuItem) item;
						btn.AccessibleDescription = GetIconName (btn.Name);
						if (btn.AccessibleDescription != null && btn.AccessibleDescription.Length > 2)
							btn.ImageTransparentColor = Color.Transparent;
						else
							removeIcon (btn);
					}
				}
			}
		}

		public void UpdateColors ()
		{
			foreach (var control in tools.Items)
			{
				if (control is ToolStripButton)
				{
					if (((ToolStripButton) control).AccessibleDescription != null &&
					    ((ToolStripButton) control).AccessibleDescription.Length > 2)
						UpdateIcon ((ToolStripButton) control);
				}
			}
			
			foreach (ToolStripMenuItem control in menu.Items)
			{
				foreach (var item in control.DropDownItems)
				{
					if (item is ToolStripMenuItem)
					{
						if (((ToolStripMenuItem) item).AccessibleDescription != null &&
						    ((ToolStripMenuItem) item).AccessibleDescription.Length > 2)
							UpdateIcon ((ToolStripMenuItem) item);
					}	
				}
			}
		}

		private string GetIconName (string str)
		{
			string res = "";

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
				{
					res = "menu-saveall";
				}
					break;

				case "tsCut" :
				case "miCut" :
				{
					res = "menu-cut";
				}
					break;

				case "tsCopy" :
				case "miCopy" :
				{
					res = "copy";
				}
					break;

				case "tsPaste" :
				case "miPaste" :
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
				{
					res = "notificationError";
				}
					break;

				case "tsShowCompilerConsoleWindow" :
				{
					res = "toolWindowMessages";
				}
					break;

				case "tsShowDebugVariablesListWindow" :
				{
					res = "dynamicUsages";
				}
					break;

				case "tsShowDebugWatchListWindow" :
				{
					res = "showHiddens";
				}
					break;

				case "tsDisassembly" :
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
				{
					res = "showReadAccess";
				}
					break;

				case "tsGotoRealization" :
				{
					res = "showWriteAccess";
				}
					break;

				case "miGenerateRealization" :
				{
					res = "externalTools";
				}
					break;

				case "tsHelp" :
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

		private void UpdateIcon (ToolStripButton btn)
		{
			string add = "";
			if (hasDark (btn.AccessibleDescription))
			{
				bool isDark = Helper.isDark (YukiTheme_VisualPascalABCPlugin.bg);
				add = isDark ? "" : "_dark";
			}

			var a = Assembly.GetExecutingAssembly ();
			// MessageBox.Show (btn.Name);
			Helper.renderSVG (btn, Helper.loadsvg (btn.AccessibleDescription + add, a, IconFolder),
			                  false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.bgBorder);
		}

		private void UpdateIcon (ToolStripMenuItem btn)
		{
			string add = "";
			if (hasDark (btn.AccessibleDescription))
			{
				bool isDark = Helper.isDark (YukiTheme_VisualPascalABCPlugin.bg);
				add = isDark ? "" : "_dark";
			}

			var a = Assembly.GetExecutingAssembly ();
			// MessageBox.Show (btn.Name);
			Helper.renderSVG (btn, Helper.loadsvg (btn.AccessibleDescription + add, a, IconFolder),
			                  false, Size.Empty, true, YukiTheme_VisualPascalABCPlugin.bgBorder);
		}

		private bool hasDark (string str)
		{
			bool s = false;
			switch (str)
			{
				case "menu-cut" :
				case "menu-saveall" :
				case "menu-paste" :
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

		private void removeIcon (ToolStripMenuItem mi)
		{
			if (needToDelete (mi.Name))
			{
				mi.Image?.Dispose ();
				mi.Image = null;
			}
		}

		private bool needToDelete (string str)
		{
			bool res = false;
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
		
	}
}