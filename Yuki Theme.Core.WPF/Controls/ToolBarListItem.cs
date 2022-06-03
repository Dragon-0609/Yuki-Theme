using System;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;
namespace Yuki_Theme.Core.WPF.Controls
{
	public class ToolBarListItem : ListViewItem
	{
		public static Action <ToolBarListItem, bool> OnVisibilityChanged;
		public static Action <ToolBarListItem, bool> OnRightChanged;

		public static bool FreezeAllBehaviour = false;
		
		public bool IsShown
		{
			get => (bool)GetValue (IsShownProperty);
			set
			{
				SetValue (IsShownProperty, value);
				if (!FreezeAllBehaviour && OnVisibilityChanged != null) OnVisibilityChanged (this, value);
			}
		}

		public static readonly DependencyProperty IsShownProperty =
			DependencyProperty.Register ("IsShown", typeof (bool), typeof (ToolBarListItem),
			                             new PropertyMetadata (true));

		public bool IsRight
		{
			get => (bool)GetValue (IsRightProperty);
			set
			{
				SetValue (IsRightProperty, value);
				if (!FreezeAllBehaviour && OnRightChanged != null) OnRightChanged (this, value);
			}
		}

		public static readonly DependencyProperty IsRightProperty =
			DependencyProperty.Register ("IsRight", typeof (bool), typeof (ToolBarListItem), new PropertyMetadata (false));

		public WinForms.ToolStripItem item;
		
		public override string ToString ()
		{
			return (string)Content;
		}

		public ToolBarListItem ()
		{
			Content = "";
			item = null;
		}

		public ToolBarListItem (string text, bool isVisible, bool isRight)
		{
			Content = text;
			IsShown = isVisible;
			IsRight = isRight;
			item = null;
		}

		public ToolBarListItem (string text, bool isVisible, bool isRight, WinForms.ToolStripItem stripItem)
		{
			Content = text;
			IsShown = isVisible;
			IsRight = isRight;
			item = stripItem;
		}
		
	}
}