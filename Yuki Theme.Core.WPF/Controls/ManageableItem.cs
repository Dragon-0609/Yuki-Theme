using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class ManageableItem : ListViewItem
	{
		public bool IsGroup
		{
			get { return (bool)GetValue (IsGroupProperty); }
			set { SetValue (IsGroupProperty, value); }
		}

		public static readonly DependencyProperty IsGroupProperty =
			DependencyProperty.Register ("IsGroup", typeof (bool), typeof (ManageableItem),
			                             new PropertyMetadata (false));

		public bool IsOld
		{
			get { return (bool)GetValue (IsOldProperty); }
			set { SetValue (IsOldProperty, value); }
		}

		public static readonly DependencyProperty IsOldProperty =
			DependencyProperty.Register ("IsOld", typeof (bool), typeof (ManageableItem), new PropertyMetadata (true));

		public bool IsCollapsed
		{
			get
			{
				if (IsGroup)
					return (bool)GetValue (IsCollapsedProperty);
				else
				{
					return ParentGroup.IsCollapsed;
				}
			}
			set { SetValue (IsCollapsedProperty, value); }
		}

		public static readonly DependencyProperty IsCollapsedProperty =
			DependencyProperty.Register ("IsCollapsed", typeof (bool), typeof (ManageableItem),
			                             new PropertyMetadata (false));

		public ManageableItem ParentGroup
		{
			get { return (ManageableItem)GetValue (ParentGroupProperty); }
			set { SetValue (ParentGroupProperty, value); }
		}

		public static readonly DependencyProperty ParentGroupProperty =
			DependencyProperty.Register ("ParentGroup", typeof (ManageableItem), typeof (ManageableItem),
			                             new PropertyMetadata (null));

		public Visibility VGroup
		{
			get { return Visibility.Visible; }
			set
			{
				if (IsGroup)
					IsCollapsed = value == Visibility.Collapsed;
			}
		}

		public static readonly DependencyProperty VGroupProperty =
			DependencyProperty.Register ("VGroup", typeof (Visibility), typeof (ManageableItem),
			                             new PropertyMetadata (Visibility.Collapsed));


		public List <ManageableItem> children = new List <ManageableItem> ();

		public ManageableItem ()
		{
		}

		public ManageableItem (string text, string original, bool isGroup)
		{
			Content = text;
			IsGroup = isGroup;
			Tag = original;
		}

		public ManageableItem (string text, string original, bool isGroup, bool isOld, ManageableItem group)
		{
			Content = text;
			IsGroup = isGroup;
			Tag = original;
			IsOld = isOld;
			ParentGroup = group;
			group.children.Add (this);
		}

		public override string ToString ()
		{
			return (string)Content;
		}

		public void UpdateCollapse (bool value)
		{
			this.InvalidateProperty (IsCollapsedProperty);
			OnPropertyChanged (new DependencyPropertyChangedEventArgs (IsCollapsedProperty, !value, value));
			if (IsGroup)
			{
				foreach (ManageableItem child in children)
				{
					child.IsCollapsed = value;
				}
			}
		}
	}
}