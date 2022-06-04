using System;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;
namespace Yuki_Theme.Core.WPF.Controls
{
	public class ToolBarListItem : ListViewItem
	{
		public static Action <ToolBarListItem, bool> OnVisibilityChanged = null;
		public static Action <ToolBarListItem, bool> OnRightChanged = null;

		public static bool FreezeAllBehaviour = false;
		
		public bool IsShown
		{
			get => (bool)GetValue (IsShownProperty);
			set
			{
				SetValue (IsShownProperty, value);
				if (!FreezeAllBehaviour && isInited && OnVisibilityChanged != null) OnVisibilityChanged (this, value);
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
				if (!FreezeAllBehaviour && isInited && OnRightChanged != null) OnRightChanged (this, value);
			}
		}

		public static readonly DependencyProperty IsRightProperty =
			DependencyProperty.Register ("IsRight", typeof (bool), typeof (ToolBarListItem), new PropertyMetadata (false));

		public WinForms.ToolStripItem item;
		
		public override string ToString ()
		{
			return (string)Content;
		}

		private bool isInited = false;

		public ToolBarListItem ()
		{
			Content = "";
			item = null;
			isInited = true;
		}

		public ToolBarListItem (string text, bool isVisible, bool isRight)
		{
			Content = text;
			IsShown = isVisible;
			IsRight = isRight;
			item = null;
			isInited = true;
		}

		public ToolBarListItem (string text, bool isVisible, bool isRight, WinForms.ToolStripItem stripItem)
		{
			Content = text;
			IsShown = isVisible;
			IsRight = isRight;
			item = stripItem;
			isInited = true;
		}
		
	}
}