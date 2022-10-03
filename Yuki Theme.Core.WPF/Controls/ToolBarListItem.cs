using System;
using System.Windows;
using System.Windows.Controls;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Interfaces;
using WinForms = System.Windows.Forms;
namespace Yuki_Theme.Core.WPF.Controls
{
	public class ToolBarListItem : ListViewItem
	{
		public string itemName;

		public ToolBarList ParentList;
		public Size ItemSize;

		public bool IsShown
		{
			get => ParentList._controller.IsVisible (itemName);
			set => ParentList._controller.SetVisible (itemName, value);
		}

		public bool IsRight
		{
			get => ParentList._controller.IsRight (itemName);
			set => ParentList._controller.SetRight (itemName, value);
		}

		public override string ToString ()
		{
			return (string)Content;
		}

		public ToolBarListItem (ToolBarList parentList)
		{
			ParentList = parentList;
			Content = "";
			itemName = string.Empty;
		}

		public ToolBarListItem (string text, ToolBarList parentList)
		{
			ParentList = parentList;
			Content = text;
			itemName = string.Empty;
		}

		public ToolBarListItem (string text, string ItemName, ToolBarList parentList)
		{
			Content = text;
			itemName = ItemName;
			ParentList = parentList;
		}

	}
}