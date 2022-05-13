using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class ManageableItem : ListViewItem
	{
		public bool IsGroup
		{
			get { return (bool)GetValue(IsGroupProperty); }
			set { SetValue(IsGroupProperty, value); }
		}
		public static readonly DependencyProperty IsGroupProperty =
			DependencyProperty.Register("IsGroup", typeof(bool), typeof(ManageableItem),
			                            new PropertyMetadata(false));
		
		public bool IsOld
		{
			get { return (bool)GetValue(IsOldProperty); }
			set { SetValue(IsOldProperty, value); }
		}

		public static readonly DependencyProperty IsOldProperty =
			DependencyProperty.Register ("IsOld", typeof (bool), typeof (ManageableItem), new PropertyMetadata (true));
		
		public bool IsCollapsed
		{
			get { return (bool)GetValue(IsCollapsedProperty); }
			set { SetValue(IsCollapsedProperty, value); }
		}
		public static readonly DependencyProperty IsCollapsedProperty =
			DependencyProperty.Register("IsCollapsed", typeof(bool), typeof(ManageableItem),
			                            new PropertyMetadata(false));
		
		public bool ParentGroup
		{
			get { return (bool)GetValue(ParentGroupProperty); }
			set { SetValue(ParentGroupProperty, value); }
		}
		public static readonly DependencyProperty ParentGroupProperty =
			DependencyProperty.Register("ParentGroup", typeof(ManageableItem), typeof(ManageableItem),
			                            new PropertyMetadata(null));
		
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(ManageableItem),
			                            new PropertyMetadata(""));

		public List <ManageableItem> children = new List <ManageableItem> ();

		
		public ManageableItem ()
		{
		}
		
		public ManageableItem (string text, bool isGroup)
		{
			Title = text;
			IsGroup = isGroup;
		}

		public ManageableItem (string text, bool isGroup, bool isOld)
		{
			Title = text;
			IsGroup = isGroup;
			IsOld = isOld;
		}

		public override string ToString ()
		{
			return Title;
		}
	}
}