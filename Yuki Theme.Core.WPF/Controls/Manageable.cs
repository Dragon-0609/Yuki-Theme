﻿using System.Windows;
using System.Windows.Controls;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class Manageable : ListView
	{
		public Manageable ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Manageable), new FrameworkPropertyMetadata(typeof(Manageable)));
		}
		
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ManageableItem("Empty", false);
		}
	}
}