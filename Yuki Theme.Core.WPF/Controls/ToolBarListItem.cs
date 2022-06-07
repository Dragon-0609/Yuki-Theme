using System;
using System.Windows;
using System.Windows.Controls;
using Yuki_Theme.Core.Interfaces;
using WinForms = System.Windows.Forms;
namespace Yuki_Theme.Core.WPF.Controls
{
	public class ToolBarListItem : ListViewItem
	{
		public static ICamouflage camouflage;

		public static IIconManager manager;
		
		public WinForms.ToolStripItem item;

		public bool IsShown
		{
			get => camouflage.IsVisible (item.Name);
			set => camouflage.SetVisible (item.Name, value);
		}

		public bool IsRight
		{
			get => camouflage.IsRight (item.Name);
			set => camouflage.SetRight (item.Name, value);
		}

		public override string ToString ()
		{
			return (string)Content;
		}

		public ToolBarListItem ()
		{
			Content = "";
			item = null;
		}

		public ToolBarListItem (string text)
		{
			Content = text;
			item = null;
		}

		public ToolBarListItem (string text, WinForms.ToolStripItem stripItem)
		{
			Content = text;
			item = stripItem;
		}

	}
}