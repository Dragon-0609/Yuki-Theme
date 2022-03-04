using System.Collections.Generic;
using System.Windows.Forms;

namespace Yuki_Theme_Plugin.Controls.DockStyles
{
	public class ToolItemGroup
	{
		public List <ToolStripItem> items = new List <ToolStripItem> ();
		
		private ToolItemGroup _prev;
		
		public ToolItemGroup        next;

		public ToolItemGroup prev
		{
			get { return _prev; }
			set
			{
				_prev = value;
				value.next = this;
			}
		}

		public ToolStripSeparator separator = null;

		public bool isLast = false;

		public bool IsEmpty ()
		{
			return items.Count < 1;
		}

		public bool HasItem (string nm)
		{
			bool b = false;
			foreach (ToolStripItem item in items)
			{
				if (item.Name == nm)
				{
					b = true;
					break;
				}
			}

			return b;
		}

		public ToolStripItem GetItem (string nm)
		{
			ToolStripItem b = null;
			foreach (ToolStripItem item in items)
			{
				if (item.Name == nm)
				{
					b = item;
					break;
				}
			}

			return b;
		}

		public bool isAllHidden ()
		{
			bool b = true;
			foreach (ToolStripItem item in items)
			{
				b = b && !item.Visible;
			}

			return b;
		}

		public bool isAllRight ()
		{
			bool b = true;
			foreach (ToolStripItem item in items)
			{
				b = b && item.Alignment == ToolStripItemAlignment.Right;
			}

			return b;
		}
	}
}