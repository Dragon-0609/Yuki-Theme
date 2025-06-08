using System.Collections.Generic;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;

namespace YukiTheme.Engine.Plugins.Definitions.CodeTemplate
{
	public class CodeTemplates_ : IVisualPascalABCPlugin
	{
		public static string StringsPrefix = "VPP_CODE_TEMPLATES_PLUGIN_";
		private Images images = new Images();
		public VisualPascalABC.Form1 MainForm;
		IWorkbench workbench;
		public CodeTemplatesForm ctForm = new CodeTemplatesForm();
		private bool firstTime = true;

		public CodeTemplates_(IWorkbench workbench)
		{
			this.workbench = workbench;
			this.MainForm = workbench.MainForm as VisualPascalABC.Form1;
			ctForm.MainForm = MainForm;
		}

		public string Name
		{
			get { return "Code Templates Plugin"; }
		}

		public string Version
		{
			get { return "0.1"; }
		}

		public string Copyright
		{
			get { return "Copyright © 2019 by Stanislav Mikhalkovich"; }
		}

		public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
		{
			PluginGUIItem Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION",
				images.PluginImage.Image, images.PluginImage.BackColor, Execute);
			MenuItems.Add(Item);
			ToolBarItems.Add(Item);
		}

		public void Execute()
		{
			var ta = MainForm.CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea;
			if (firstTime)
			{
				//MainForm.ProjectPane = null;
				MainForm.AddWindowToDockPanel(ctForm, MainForm.MainDockPanel, ctForm.Dock, DockState.DockLeft, false,
					MainForm.ProjectPane, -1);
				ctForm.Hide();
				ctForm.Show();
				//MainForm.ProjectPane = ctForm.Pane;
				firstTime = false;
			}
			else
			{
				if (ctForm.Visible)
					ctForm.Hide();
				else ctForm.Show();
			}

			ta.Focus();
		}
	}
}