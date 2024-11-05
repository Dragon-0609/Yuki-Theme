using System;
using System.Reflection;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace YukiTheme.Style;

public class Extender
{
	/// <summary>
	///     Inject New Styles to old Dock Panel
	/// </summary>
	/// <param name="dockPanel">Target</param>
	public static void SetSchema(DockPanel dockPanel)
	{
		var collection = (DockPaneCollection)typeof(DockPaneCollection)
			.GetConstructor(
				BindingFlags.NonPublic | BindingFlags.Instance,
				null, Type.EmptyTypes, null).Invoke(null);

		var add = collection.GetType()
			.GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
		var remove = collection.GetType()
			.GetMethod("Remove", BindingFlags.NonPublic | BindingFlags.Instance);
		foreach (var dockPane in dockPanel.Panes) add.Invoke(collection, new object[] { dockPane });

		foreach (var dockPane in collection) remove.Invoke(dockPanel.Panes, new object[] { dockPane });

		var collect2 = (DockContentCollection)typeof(DockContentCollection)
			.GetConstructor(
				BindingFlags.NonPublic | BindingFlags.Instance,
				null, Type.EmptyTypes, null).Invoke(null);

		var add2 = collect2.GetType()
			.GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
		var remove2 = collect2.GetType()
			.GetMethod("Remove", BindingFlags.NonPublic | BindingFlags.Instance);
		foreach (DockContent dockPane in dockPanel.Contents) add2.Invoke(collect2, new object[] { dockPane });

		foreach (DockContent dockPane in collect2) remove2.Invoke(dockPanel.Contents, new object[] { dockPane });

		DockPanelExtender.IDockPaneCaptionFactory ar = new YukiDockPaneCaptionFactory();
		DockPanelExtender.IAutoHideStripFactory au = new YukiAutoHideStripFactory();
		DockPanelExtender.IDockPaneStripFactory at = new YukiDockPaneStripFactory();
		dockPanel.Extender.AutoHideStripFactory = au;
		dockPanel.Extender.DockPaneCaptionFactory = ar;
		dockPanel.Extender.DockPaneStripFactory = at;


		foreach (var dockPane in collection) add.Invoke(dockPanel.Panes, new object[] { dockPane });

		var prop = typeof(DockPane).GetField("m_captionControl", BindingFlags.NonPublic | BindingFlags.Instance);

		foreach (var dockPane in dockPanel.Panes)
		{
			dockPane.Controls.Remove((Control)prop.GetValue(dockPane));

			prop.SetValue(dockPane, ar.CreateDockPaneCaption(dockPane));

			dockPane.Controls.Add((Control)prop.GetValue(dockPane));
		}


		foreach (DockContent dockPane in collect2) add2.Invoke(dockPanel.Contents, new object[] { dockPane });

		var dc = typeof(DockPanel).GetField("m_autoHideStripControl",
			BindingFlags.NonPublic | BindingFlags.Instance);
		dockPanel.Controls.Remove((Control)dc.GetValue(dockPanel));
		var ab = au.CreateAutoHideStrip(dockPanel);
		dc.SetValue(dockPanel, ab);
		dockPanel.Controls.Add(ab);


		var prop2 = typeof(DockPane).GetField("m_tabStripControl", BindingFlags.NonPublic | BindingFlags.Instance);

		foreach (var dockPane in dockPanel.Panes)
		{
			var pane = prop2.GetValue(dockPane);
			dockPane.Controls.Remove((Control)pane);
			var dpane = at.CreateDockPaneStrip(dockPane);
			prop2.SetValue(dockPane, dpane);
			dockPane.Controls.Add(dpane);
		}

		collection = null;
		collect2 = null;

		var prop3 = typeof(DockPanel).GetField("m_autoHideWindow",
			BindingFlags.NonPublic | BindingFlags.Instance);
		var m_autohide = prop3.GetValue(dockPanel);
		var prop4 = m_autohide.GetType().GetField("m_flagAnimate", BindingFlags.NonPublic | BindingFlags.Instance);
		prop4.SetValue(m_autohide, false);

		foreach (var content in dockPanel.Contents) content.DockHandler.DockStateChanged += (sender, args) => { prop4.SetValue(m_autohide, false); };
	}

	private class YukiDockPaneStripFactory : DockPanelExtender.IDockPaneStripFactory
	{
		public DockPaneStripBase CreateDockPaneStrip(DockPane pane)
		{
			return new YukiDockPaneStrip(pane);
		}
	}

	private class YukiAutoHideStripFactory : DockPanelExtender.IAutoHideStripFactory
	{
		public AutoHideStripBase CreateAutoHideStrip(DockPanel panel)
		{
			return new YukiAutoHideStrip(panel);
		}
	}

	private class YukiDockPaneCaptionFactory : DockPanelExtender.IDockPaneCaptionFactory
	{
		public DockPaneCaptionBase CreateDockPaneCaption(DockPane pane)
		{
			return new YukiDockPaneCaption(pane);
		}
	}
}