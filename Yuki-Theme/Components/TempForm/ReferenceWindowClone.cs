using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using VisualPascalABC;

namespace YukiTheme.Components.TempForm;

public class ReferenceWindowClone : ReferenceForm
{
	internal static OpenFileDialog openFileDialog1;
	internal static Button button1;
	internal static Button btnOk;
	internal static Button btnCancel;
	internal static ColumnHeader chVersion;
	internal static ColumnHeader chComponentName;
	internal static ListView lvGac;
	internal static TabPage tpAssemblies;
	internal static TabPage tpGAC;
	internal static TabControl tabControl1;
	internal static TabPage tpNuGet;
	internal static TextBox tbPackageName;
	internal static Label label1;
	internal static Button btnInstallPackage;
	internal static Button btnSearchPackages;
	internal static TextBox tbLog;
	internal static Label label2;

	internal static bool Get<T>(ReferenceForm target, string name, out T output) where T : Control
	{
		output = null;

		var field = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public);

		if (field != null)
		{
			output = (T)field.GetValue(target);
			return true;
		}

		return false;
	}
}