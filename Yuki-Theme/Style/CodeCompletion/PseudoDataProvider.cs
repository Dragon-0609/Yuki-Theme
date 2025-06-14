using System;
using ICSharpCode.TextEditor.Gui.InsightWindow;
using VisualPascalABC;
using YukiTheme.Tools;

namespace YukiTheme.Style.CodeCompletion
{
	public static class PseudoDataProvider
	{
		private static Type _providerType;


		static PseudoDataProvider()
		{
			_providerType = typeof(Form1).Assembly.GetType("VisualPascalABC.DefaultInsightDataProvider", true);
		}

		public static IInsightDataProvider Create(params object[] args)
		{
			return (IInsightDataProvider)Activator.CreateInstance(_providerType, args);
		}

		public static T Get<T>(IInsightDataProvider provider, string name)
		{
			return MiscHelper.GetByReflection<T>(_providerType,
				provider, name, true);
		}

		public static void Set(IInsightDataProvider provider, string name, object value)
		{
			provider.SetByReflection(_providerType, name, value);
		}
	}
}