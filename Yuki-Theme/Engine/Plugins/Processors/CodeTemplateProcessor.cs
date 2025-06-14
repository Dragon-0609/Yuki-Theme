using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;
using YukiTheme.Components;
using YukiTheme.Engine.Plugins.Definitions.CodeTemplate;
using YukiTheme.Tools;

namespace YukiTheme.Engine.Processors
{
	public class CodeTemplateProcessor : InjectorProcessor
	{
		public override string Name => CodeTemplates_.StringsPrefix;

		private object _ctForm;
		private DockContent _form;

		protected override void Process(PluginGUIItem plugin)
		{
			var execute = plugin.GetByReflection<PluginGUIItemExecuteDelegate>("executeDelegate");

			object pluginCore = execute.Target;

			_ctForm = pluginCore.GetByReflection(nameof(CodeTemplates_.ctForm), false);

			_form = ((DockContent)_ctForm);

			_form.Closed += ClosedCtForm;
			ColorUpdated += UpdateCtColors;

			InjectListView();

			UpdateCtColors();

			_form.Hide();
			_form.Show();
		}

		private void InjectListView()
		{
			Type ctType = _ctForm.GetType();

			// Console.WriteLine($"Type: {ctType.Name}");
			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

			ListView list = _ctForm.GetByReflection<ListView>(nameof(CodeTemplatesForm.listBox1));

			var indexChanged = ctType.GetMethod(nameof(CodeTemplatesForm.listBox1_SelectedIndexChanged), flags);
			var sizeChanged = ctType.GetMethod(nameof(CodeTemplatesForm.listBox1_SizeChanged), flags);
			var mouseDoubleClick = ctType.GetMethod(nameof(CodeTemplatesForm.listBox1_MouseDoubleClick_1), flags);

			ListViewExt listEx = new ListViewExt();

			listEx.BackColor = list.BackColor;
			listEx.Dock = list.Dock;
			listEx.Font = list.Font;
			listEx.FullRowSelect = list.FullRowSelect;
			listEx.HideSelection = list.HideSelection;
			listEx.Location = list.Location;
			listEx.Margin = list.Margin;
			listEx.MultiSelect = list.MultiSelect;
			listEx.Name = list.Name;
			listEx.Size = list.Size;
			listEx.TabIndex = list.TabIndex;
			listEx.TileSize = list.TileSize;
			listEx.UseCompatibleStateImageBehavior = list.UseCompatibleStateImageBehavior;
			listEx.View = list.View;

			Dictionary<string, ListViewGroup> groups = new Dictionary<string, ListViewGroup>();
			foreach (ListViewGroup group in list.Groups)
			{
				ListViewGroup viewGroup = new ListViewGroup()
				{
					Header = group.Header, Name = group.Name
				};
				groups.Add(group.Name, viewGroup);
				listEx.Groups.Add(viewGroup);
			}

			string texts = "";
			foreach (ListViewItem item in list.Items)
			{
				ListViewItem viewItem = new ListViewItem(item.Text);
				listEx.Items.Add(viewItem);
				viewItem.Group = groups[item.Group.Name];
				texts += item.Text + Environment.NewLine;
			}

			Clipboard.SetText(texts);
			// listEx.Groups.AddRange(list.Groups);
			// listEx.Items.AddRange(list.Items);

			listEx.SelectedIndexChanged += (e, a) =>
			{
				// Console.WriteLine(
				// $"Executing selected index changed in form {indexChanged != null}, count: {listEx.SelectedItems.Count}, item: {(listEx.SelectedItems.Count > 0 ? listEx.SelectedItems[0].Index : "null")}");
				indexChanged?.Invoke(_ctForm, [e, a]);
			};
			listEx.SizeChanged += (e, a) =>
			{
				// Console.WriteLine($"Executing size changed in form {sizeChanged != null}");
				sizeChanged?.Invoke(_ctForm, [e, a]);
			};
			listEx.MouseDoubleClick += (e, a) =>
			{
				// if (listEx.SelectedItems == null || listEx.SelectedItems.Count == 0) return;
				// Console.WriteLine(
				// $"Executing mouse double click in form {mouseDoubleClick != null}, count: {listEx.SelectedItems.Count}, item: {(listEx.SelectedItems.Count > 0 ? listEx.SelectedItems[0].Index : "")}");
				mouseDoubleClick?.Invoke(_ctForm, [e, a]);
			};

			int listIndex = _form.Controls.IndexOf(list);
			_form.Controls.Remove(list);

			Control[] controls = new Control[_form.Controls.Count];
			for (var i = 0; i < _form.Controls.Count; i++)
			{
				var control = _form.Controls[i];
				controls[i] = control;
			}

			for (var i = 0; i < controls.Length; i++)
			{
				if (listIndex == i)
					_form.Controls.Add(listEx);
				_form.Controls.Add(controls[i]);
			}

			_ctForm.SetByReflection(ctType, nameof(CodeTemplatesForm.listBox1), listEx);
			// _ctForm.CallByReflection(nameof(CodeTemplatesForm.LoadTemplates), null, true);
		}

		private void ClosedCtForm(object sender, EventArgs e)
		{
			ColorUpdated -= UpdateCtColors;
		}

		private void UpdateCtColors()
		{
			ColorHelper.SetColors(_form, true, null);
		}
	}
}