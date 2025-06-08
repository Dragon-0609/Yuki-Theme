using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Fluent;
using Fluent.Lists;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;
using YukiTheme.Components;
using YukiTheme.Engine.Plugins.Definitions.CodeTemplate;
using YukiTheme.Tools;

namespace YukiTheme.Engine.Processors
{
	public class InternalErrorReportProcessor : InjectorProcessor
	{
		public override string Name => InternalErrorReport_.StringsPrefix;

		protected override bool InjectImmediate => true;

		private object _form1;
		private object _form2;
		private Form _formVisual1;
		private Form _formVisual2;

		protected override void Process(PluginGUIItem plugin)
		{
			var execute = plugin.GetByReflection<PluginGUIItemExecuteDelegate>("executeDelegate");

			object pluginCore = execute.Target;
			
			_form1 = pluginCore.GetByReflection(nameof(InternalErrorReport_.CompilerInternalErrorReport), false);
			_form2 = pluginCore.GetByReflection(nameof(InternalErrorReport_.ErrorReport), false);

			_formVisual1 = ((Form)_form1);
			_formVisual2 = ((Form)_form2);
			ColorUpdated += UpdateColors;

			// InjectListView();

			UpdateColors();
		}

		private void ClosedFormVisual1(object sender, EventArgs e)
		{
			ColorUpdated -= UpdateColors;
		}

		private void UpdateColors()
		{
			ColorHelper.SetColors(_formVisual1, true, null);
			ColorHelper.SetColors(_formVisual2, true, null);
		}
	}
}