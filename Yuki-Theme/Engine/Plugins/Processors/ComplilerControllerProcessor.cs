using System;
using System.Windows.Forms;
using VisualPascalABCPlugins;
using YukiTheme.Tools;

namespace YukiTheme.Engine.Processors
{
	public class CompilerControllerProcessor : InjectorProcessor
	{
		public override string Name => CompilerController_.StringsPrefix;

		private object _compilerForm;
		private Form _form;

		protected override void Process(PluginGUIItem plugin)
		{
			var execute = plugin.GetByReflection<PluginGUIItemExecuteDelegate>("executeDelegate");

			object pluginCore = execute.Target;
			_compilerForm = pluginCore.GetByReflection(nameof(CompilerController_.CompilerInformation), false);

			_form = ((Form)_compilerForm);

			ColorUpdated += UpdateColors;

			// InjectListView();

			UpdateColors();
		}

		private void ClosedForm(object sender, EventArgs e)
		{
			ColorUpdated -= UpdateColors;
		}

		private void UpdateColors()
		{
			ColorHelper.SetColors(_form, true, null);
		}
	}
}