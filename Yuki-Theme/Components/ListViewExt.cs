// Converted to C# from https://stackoverflow.com/a/71009137

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace YukiTheme.Components
{
	[ToolboxItem(true)]
	public class ListViewExt : ListView
	{
		private Color _groupHeadingBackColor = Color.Gray;

		public Color GroupHeadingBackColor
		{
			get => _groupHeadingBackColor;
			set => _groupHeadingBackColor = value;
		}

		private Color _groupHeadingForeColor = Color.Black;

		public Color GroupHeadingForeColor
		{
			get => _groupHeadingForeColor;
			set => _groupHeadingForeColor = value;
		}

		private Font _groupHeadingFont;

		public Font GroupHeadingFont
		{
			get => _groupHeadingFont;
			set => _groupHeadingFont = value;
		}

		private Color _separatorColor;

		public Color SeparatorColor
		{
			get => _separatorColor;
			set => _separatorColor = value;
		}

		private Color _listViewSelectionColor;

		public Color ListViewSelectionColor
		{
			get => _listViewSelectionColor;
			set => _listViewSelectionColor = value;
		}

		public const int LVCDI_ITEM = 0x0;
		public const int LVCDI_GROUP = 0x1;
		public const int LVCDI_ITEMSLIST = 0x2;

		public const int LVM_FIRST = 0x1000;
		public const int LVM_GETGROUPRECT = (LVM_FIRST + 98);
		public const int LVM_ENABLEGROUPVIEW = (LVM_FIRST + 157);
		public const int LVM_SETGROUPINFO = (LVM_FIRST + 147);
		public const int LVM_GETGROUPINFO = (LVM_FIRST + 149);
		public const int LVM_REMOVEGROUP = (LVM_FIRST + 150);
		public const int LVM_MOVEGROUP = (LVM_FIRST + 151);
		public const int LVM_GETGROUPCOUNT = (LVM_FIRST + 152);
		public const int LVM_GETGROUPINFOBYINDEX = (LVM_FIRST + 153);
		public const int LVM_MOVEITEMTOGROUP = (LVM_FIRST + 154);

		public const int WM_LBUTTONUP = 0x202;

		[StructLayout(LayoutKind.Sequential)]
		public struct NMHDR
		{
			public IntPtr hwndFrom;
			public IntPtr idFrom;
			public int code;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left, top, right, bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NMCUSTOMDRAW
		{
			public NMHDR hdr;
			public int dwDrawStage;
			public IntPtr hdc;
			public RECT rc;
			public IntPtr dwItemSpec;
			public uint uItemState;
			public IntPtr lItemlParam;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NMLVCUSTOMDRAW
		{
			public NMCUSTOMDRAW nmcd;
			public int clrText;
			public int clrTextBk;
			public int iSubItem;
			public int dwItemType;
			public int clrFace;
			public int iIconEffect;
			public int iIconPhase;
			public int iPartId;
			public int iStateId;
			public RECT rcText;
			public uint uAlign;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct LVGROUP
		{
			public uint cbSize;
			public uint mask;
			public IntPtr pszHeader;
			public int cchHeader;
			public IntPtr pszFooter;
			public int cchFooter;
			public int iGroupId;
			public uint stateMask;
			public uint state;
			public uint uAlign;
			public IntPtr pszSubtitle;
			public uint cchSubtitle;
			public IntPtr pszTask;
			public uint cchTask;
			public IntPtr pszDescriptionTop;
			public uint cchDescriptionTop;
			public IntPtr pszDescriptionBottom;
			public uint cchDescriptionBottom;
			public int iTitleImage;
			public int iExtendedImage;
			public int iFirstItem;
			public uint cItems;
			public IntPtr pszSubsetTitle;
			public uint cchSubsetTitle;
		}

		[Flags]
		public enum CDRF
		{
			CDRF_DODEFAULT = 0x0,
			CDRF_NEWFONT = 0x2,
			CDRF_SKIPDEFAULT = 0x4,
			CDRF_DOERASE = 0x8,
			CDRF_SKIPPOSTPAINT = 0x100,
			CDRF_NOTIFYPOSTPAINT = 0x10,
			CDRF_NOTIFYITEMDRAW = 0x20,
			CDRF_NOTIFYSUBITEMDRAW = 0x20,
			CDRF_NOTIFYPOSTERASE = 0x40
		}

		[Flags]
		public enum CDDS
		{
			CDDS_PREPAINT = 0x1,
			CDDS_POSTPAINT = 0x2,
			CDDS_PREERASE = 0x3,
			CDDS_POSTERASE = 0x4,
			CDDS_ITEM = 0x10000,
			CDDS_ITEMPREPAINT = CDDS_ITEM | CDDS_PREPAINT,
			CDDS_ITEMPOSTPAINT = CDDS_ITEM | CDDS_POSTPAINT,
			CDDS_ITEMPREERASE = CDDS_ITEM | CDDS_PREERASE,
			CDDS_ITEMPOSTERASE = CDDS_ITEM | CDDS_POSTERASE,
			CDDS_SUBITEM = 0x20000
		}

		public const int LVGF_HEADER = 0x1;
		public const int LVGF_STATE = 0x4;
		public const int LVGF_GROUPID = 0x10;

		public const int LVGGR_HEADER = 1;

		[DllImport("user32.dll", EntryPoint = "SendMessageW", SetLastError = true)]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref LVGROUP lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessageW", SetLastError = true)]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref RECT lParam);

		protected override void WndProc(ref Message m)
		{
			const int NM_CUSTOMDRAW = -12;
			const int WM_REFLECT = 0x2000;
			const int WM_NOTIFY = 0x4E;

			if (m.Msg == WM_REFLECT + WM_NOTIFY)
			{
				NMHDR hdr = (NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NMHDR));
				if (hdr.code == NM_CUSTOMDRAW)
				{
					NMLVCUSTOMDRAW draw = (NMLVCUSTOMDRAW)Marshal.PtrToStructure(m.LParam, typeof(NMLVCUSTOMDRAW));

					switch ((CDDS)draw.nmcd.dwDrawStage)
					{
						case CDDS.CDDS_PREPAINT:
							if (draw.dwItemType == LVCDI_GROUP)
							{
								RECT rectHeader = new RECT { top = LVGGR_HEADER };
								int groupId = draw.nmcd.dwItemSpec.ToInt32();
								SendMessage(m.HWnd, LVM_GETGROUPRECT, groupId, ref rectHeader);

								using (Graphics g = Graphics.FromHdc(draw.nmcd.hdc))
								{
									Rectangle rect = new Rectangle(rectHeader.left, rectHeader.top,
										rectHeader.right - rectHeader.left, rectHeader.bottom - rectHeader.top);
									using (Brush bgBrush = new SolidBrush(_groupHeadingBackColor))
										g.FillRectangle(bgBrush, rect);

									LVGROUP group = new LVGROUP
									{
										cbSize = (uint)Marshal.SizeOf(typeof(LVGROUP)),
										mask = LVGF_STATE | LVGF_GROUPID | LVGF_HEADER
									};
									SendMessage(m.HWnd, LVM_GETGROUPINFO, groupId, ref group);

									string text = Marshal.PtrToStringUni(group.pszHeader);
									SizeF size = g.MeasureString(text, _groupHeadingFont);

									rect.Offset(10, (int)((rect.Height - size.Height) / 2));
									using (Brush textBrush = new SolidBrush(_groupHeadingForeColor))
									{
										g.DrawString(text, _groupHeadingFont, textBrush, rect);
										rect.Offset(0, -(int)((rect.Height - size.Height) / 2));

										using (Pen linePen = new Pen(_separatorColor))
										{
											float textWidth = g.MeasureString(text, _groupHeadingFont).Width;
											g.DrawLine(linePen, rect.X + textWidth + 10, rect.Y + rect.Height / 2,
												rect.X + (rect.Width * 95 / 100), rect.Y + rect.Height / 2);
										}
									}
								}

								m.Result = (IntPtr)CDRF.CDRF_SKIPDEFAULT;
								return;
							}
							else
							{
								m.Result = (IntPtr)CDRF.CDRF_NOTIFYITEMDRAW;
							}

							break;
						case CDDS.CDDS_ITEMPREPAINT:
							// m.Result = (IntPtr)(CDRF.CDRF_NOTIFYSUBITEMDRAW | CDRF.CDRF_NOTIFYPOSTPAINT);

							int index = (int)draw.nmcd.dwItemSpec;
							if (index >= 0 && index < this.Items.Count)
							{
								ListViewItem item = this.Items[index];
								bool isSelected = item.Selected;

								Rectangle itemBounds = this.GetItemRect(index);

								using (Graphics g = Graphics.FromHdc(draw.nmcd.hdc))
								{
									Color bgColor = isSelected ? _listViewSelectionColor : BackColor;
									Color textColor = this.ForeColor;

									using (Brush bgBrush = new SolidBrush(bgColor))
										g.FillRectangle(bgBrush, itemBounds);

									using (Brush textBrush = new SolidBrush(textColor))
										g.DrawString(item.Text, this.Font, textBrush, itemBounds.Location);
								}

								m.Result = (IntPtr)CDRF.CDRF_SKIPDEFAULT; // Prevent default draw
								return;
							}
							else
							{
								m.Result = (IntPtr)(CDRF.CDRF_NOTIFYSUBITEMDRAW | CDRF.CDRF_NOTIFYPOSTPAINT);
							}

							break;
					}
				}
			}

			base.WndProc(ref m);
		}

		public ListViewExt()
		{
			_groupHeadingFont = this.Font;
			/*OwnerDraw = true;
		DrawItem += OnDrawItem;
		DrawColumnHeader += listView1_DrawColumnHeader;
		DrawSubItem += listView1_DrawSubItem;*/
		}

		private void OnDrawItem(object sender, DrawListViewItemEventArgs e)
		{
			Console.WriteLine("OnDrawItem");
			var lView = sender as ListView;

			using (var bkBrush = new SolidBrush(_listViewSelectionColor))
			{
				e.Graphics.FillRectangle(bkBrush, e.Bounds);
			}

			return;
			// if (lView.View == View.Details) return;

			TextFormatFlags flags = GetTextAlignment(lView, 0);
			Color itemColor = e.Item.ForeColor;

			if (e.Item.Selected)
			{
				using (var bkBrush = new SolidBrush(_listViewSelectionColor))
				{
					e.Graphics.FillRectangle(bkBrush, e.Bounds);
				}

				itemColor = e.Item.BackColor;
			}
			else
			{
				e.DrawBackground();
			}

			TextRenderer.DrawText(e.Graphics, e.Item.Text, e.Item.Font, e.Bounds, itemColor, flags);

			if (lView.View == View.Tile && e.Item.SubItems.Count > 1)
			{
				var subItem = e.Item.SubItems[1];
				flags = GetTextAlignment(lView, 1);
				TextRenderer.DrawText(e.Graphics, subItem.Text, subItem.Font, e.Bounds, SystemColors.GrayText, flags);
			}
		}

		private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			Console.WriteLine("Sub item");

			using (var bkBrush = new SolidBrush(_listViewSelectionColor))
			{
				e.Graphics.FillRectangle(bkBrush, e.Bounds);
			}

			return;
			var lView = sender as ListView;
			TextFormatFlags flags = GetTextAlignment(lView, e.ColumnIndex);
			Color itemColor = e.Item.ForeColor;

			if (e.Item.Selected)
			{
				if (e.ColumnIndex == 0 || lView.FullRowSelect)
				{
					using (var bkgrBrush = new SolidBrush(_listViewSelectionColor))
					{
						e.Graphics.FillRectangle(bkgrBrush, e.Bounds);
					}

					itemColor = e.Item.BackColor;
				}
			}
			else
			{
				e.DrawBackground();
			}

			TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, e.Bounds, itemColor, flags);
		}

		protected void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
			=> e.DrawDefault = true;

		private TextFormatFlags GetTextAlignment(ListView lstView, int colIndex)
		{
			TextFormatFlags flags = (lstView.View == View.Tile)
				? (colIndex == 0) ? TextFormatFlags.Default : TextFormatFlags.Bottom
				: TextFormatFlags.VerticalCenter;

			if (lstView.View == View.Details) flags |= TextFormatFlags.LeftAndRightPadding;

			if (lstView.Columns[colIndex].TextAlign != HorizontalAlignment.Left)
			{
				flags |= (TextFormatFlags)((int)lstView.Columns[colIndex].TextAlign ^ 3);
			}

			return flags;
		}
	}
}