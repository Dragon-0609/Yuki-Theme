// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2533 $</version>
// </file>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using PascalABCCompiler;
using VisualPascalABC;

namespace Yuki_Theme_Plugin.Controls.CodeCompletion
{
	/// <summary>
	///     Description of CodeCompletionListView.
	/// </summary>
	public class YukiCodeCompletionListView : UserControl
	{
		//Dictionary<ICompletionData,int> cache = new Dictionary<ICompletionData,int>();
		private readonly Hashtable          cache = new Hashtable ();
		private readonly ICompletionData [] completionData;

		public  bool FirstInsert;
		private int  firstItem;
		private int  selectedItem = -1;

		public YukiCodeCompletionListView (ICompletionData [] completionData, bool is_by_dot)
		{
			if (VisualPABCSingleton.MainForm.UserOptions.ShowCompletionInfoByGroup && is_by_dot)
				completionData = SortByGroup (completionData);
			else
				Array.Sort (completionData);
			this.completionData = completionData;
			for (var i = 0; i < completionData.Length; i++)
				cache.Add (completionData [i], i);
//			this.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKey);
//			SetStyle(ControlStyles.Selectable, false);
//			SetStyle(ControlStyles.UserPaint, true);
//			SetStyle(ControlStyles.DoubleBuffer, false);
		}

		public ImageList ImageList { get; set; }

		public int FirstItem
		{
			get => firstItem;
			set
			{
				if (firstItem != value)
				{
					firstItem = value;
					OnFirstItemChanged (EventArgs.Empty);
				}
			}
		}

		public ICompletionData SelectedCompletionData
		{
			get
			{
				if (selectedItem < 0) return null;
				return completionData [selectedItem];
			}
		}

		public int ItemHeight => Math.Max (ImageList.ImageSize.Height, (int)(Font.Height * 1.1));

		public int MaxVisibleItem => Height / ItemHeight;

		private ICompletionData [] SortByGroup (ICompletionData [] compData)
		{
			var consts = new List <ICompletionData> ();
			var meths = new List <ICompletionData> ();
			var extension_meths = new List <ICompletionData> ();
			var props = new List <ICompletionData> ();
			var fields = new List <ICompletionData> ();
			var vars = new List <ICompletionData> ();
			var events = new List <ICompletionData> ();
			var others = new List <ICompletionData> ();
			var res = new List <ICompletionData> ();
			foreach (var data in compData)
				if (data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberLocal)
				{
					vars.Add (data);
				} else if (data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberConstant)
				{
					consts.Add (data);
				} else if (data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberMethod ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberInternalMethod ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberPrivateMethod ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberProtectedMethod)
				{
					if (!data.Description.Contains ("(" + StringResources.Get ("CODE_COMPLETION_EXTENSION") + ")"))
						meths.Add (data);
					else
						extension_meths.Add (data);
				} else if (data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberProperty ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberInternalProperty ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberPrivateProperty ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberProtectedProperty)
				{
					props.Add (data);
				} else if (data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberField ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberInternalField ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberPrivateField ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberProtectedField)
				{
					fields.Add (data);
				} else if (data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberEvent ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberInternalEvent ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberPrivateEvent ||
				           data.ImageIndex == CodeCompletionProvider.ImagesProvider.IconNumberProtectedEvent)
				{
					events.Add (data);
				} else
				{
					others.Add (data);
				}

			meths.Sort ();
			extension_meths.Sort ();
			props.Sort ();
			fields.Sort ();
			vars.Sort ();
			consts.Sort ();
			events.Sort ();
			others.Sort ();
			res.AddRange (vars);
			res.AddRange (consts);
			res.AddRange (fields);
			res.AddRange (props);
			res.AddRange (meths);

			res.AddRange (events);
			res.AddRange (others);
			res.AddRange (extension_meths);
			return res.ToArray ();
		}

		public void Close ()
		{
			if (completionData != null)
			{
				Array.Clear (completionData, 0, completionData.Length);
				cache.Clear ();
			}

			base.Dispose ();
		}

		public int GetIndexByData (ICompletionData data)
		{
			var ind = -1;
			var o = cache [data];
			if (o != null) return (int)o;
			return -1;
			//if (cache.TryGetValue(data,out ind)) return ind;
			//return -1;
		}

		public void SelectIndexByCompletionData (ICompletionData data)
		{
			var index = GetIndexByData (data);
			SelectIndex (index);
		}

		public void SelectIndex (int index)
		{
			var oldSelectedItem = selectedItem;
			var oldFirstItem = firstItem;

			index = Math.Max (0, index);
			selectedItem = Math.Max (0, Math.Min (completionData.Length - 1, index));
			if (selectedItem < firstItem) FirstItem = selectedItem;
			if (firstItem + MaxVisibleItem <= selectedItem) FirstItem = selectedItem - MaxVisibleItem + 1;
			if (oldSelectedItem != selectedItem)
			{
				if (firstItem != oldFirstItem)
				{
					Invalidate ();
				} else
				{
					var min = Math.Min (selectedItem, oldSelectedItem) - firstItem;
					var max = Math.Max (selectedItem, oldSelectedItem) - firstItem;
					Invalidate (new Rectangle (0, 1 + min * ItemHeight, Width, (max - min + 1) * ItemHeight));
				}

				OnSelectedItemChanged (EventArgs.Empty);
			}
		}

		public void CenterViewOn (int index)
		{
			var oldFirstItem = FirstItem;
			var firstItem = index - MaxVisibleItem / 2;
			if (firstItem < 0)
				FirstItem = 0;
			else if (firstItem >= completionData.Length - MaxVisibleItem)
				FirstItem = completionData.Length - MaxVisibleItem;
			else
				FirstItem = firstItem;
			if (FirstItem != oldFirstItem) Invalidate ();
		}

		public void CalcWidth ()
		{
			var len = Width;
			var g = Graphics.FromHwnd (Handle);
			for (var i = 0; i < completionData.Length; i++)
			{
				var sz = g.MeasureString (completionData [i].Text, Font);
				if (len < sz.ToSize ().Width + 16)
					len = sz.ToSize ().Width + 16;
			}

			//Width = len;
			Parent.Width = len + 6;
		}

		public void ClearSelection ()
		{
			if (selectedItem < 0)
				return;
			var itemNum = selectedItem - firstItem;
			selectedItem = -1;
			Invalidate (new Rectangle (0, itemNum * ItemHeight, Width, (itemNum + 1) * ItemHeight + 1));
			Update ();
			OnSelectedItemChanged (EventArgs.Empty);
		}

		public void PageDown ()
		{
			SelectIndex (selectedItem + MaxVisibleItem);
		}

		public void PageUp ()
		{
			SelectIndex (selectedItem - MaxVisibleItem);
		}

		public void SelectNextItem ()
		{
			SelectIndex (selectedItem + 1);
		}

		public void SelectPrevItem ()
		{
			SelectIndex (selectedItem - 1);
		}

		public void SelectItemWithStart (string startText)
		{
			if (startText == null || startText.Length == 0)
				return;
			if (SelectedCompletionData != null && FirstInsert)
			{
				FirstInsert = false;
				return;
			}

			var originalStartText = startText;
			startText = startText.ToLower ();
			var bestIndex = -1;
			var bestQuality = -1;
			// Qualities: 0 = match start
			//            1 = match start case sensitive
			//            2 = full match
			//            3 = full match case sensitive
			double bestPriority = 0;
			for (var i = 0; i < completionData.Length; ++i)
			{
				var itemText = completionData [i].Text;
				var lowerText = itemText.ToLower ();
				if (lowerText.StartsWith (startText))
				{
					var priority = completionData [i].Priority;
					int quality;
					if (lowerText == startText)
					{
						if (itemText == originalStartText)
							quality = 3;
						else
							quality = 2;
					} else if (itemText.StartsWith (originalStartText))
					{
						quality = 1;
					} else
					{
						quality = 0;
					}

					bool useThisItem;
					if (bestQuality < quality)
					{
						useThisItem = true;
					} else
					{
						if (bestIndex == selectedItem)
							useThisItem = false;
						else if (i == selectedItem)
							useThisItem = bestQuality == quality;
						else
							useThisItem = bestQuality == quality && bestPriority < priority;
					}

					if (useThisItem)
					{
						bestIndex = i;
						bestPriority = priority;
						bestQuality = quality;
					}
				}
			}

			if (bestIndex < 0)
			{
				ClearSelection ();
			} else
			{
				if (bestIndex < firstItem || firstItem + MaxVisibleItem <= bestIndex)
				{
					SelectIndex (bestIndex);
					CenterViewOn (bestIndex);
				} else
				{
					SelectIndex (bestIndex);
				}
			}
		}

		protected override void OnPaint (PaintEventArgs pe)
		{
			float yPos = 1;
			float itemHeight = ItemHeight;
			var dfont = Math.Max ((int)((itemHeight - Font.Height) / 2), 0);
			// Maintain aspect ratio
			var imageWidth = (int)(itemHeight * ImageList.ImageSize.Width / ImageList.ImageSize.Height);
			var curItem = firstItem;
			var g = pe.Graphics;
			//g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			while (curItem < completionData.Length && yPos < Height)
			{
				var drawingBackground = new RectangleF (1, yPos, Width - 2, itemHeight);
				if (drawingBackground.IntersectsWith (pe.ClipRectangle))
				{
					var imageExists = ImageList != null && completionData [curItem].ImageIndex < ImageList.Images.Count;
					var drawingBackgroundText = new RectangleF (imageExists ? imageWidth + 3 : 1, yPos, Width - 2, itemHeight);
					// draw Background
					// completion background
					
					if (curItem == selectedItem)
					{
						g.FillRectangle (YukiTheme_VisualPascalABCPlugin.Colors.bgBrush, drawingBackground);
						g.FillRectangle (YukiTheme_VisualPascalABCPlugin.Colors.selectionBrush, drawingBackgroundText);
						//Pen pen = new Pen(Color.Black); pen.DashStyle;
						//g.DrawRectangle(pen, drawingBackgroundText.X, drawingBackgroundText.Y, drawingBackgroundText.Width, drawingBackgroundText.Height);
					} else
					{
						g.FillRectangle (YukiTheme_VisualPascalABCPlugin.Colors.bgBrush, drawingBackground);
					}
		
					// draw Icon
					var xPos = 0;
					if (imageExists && completionData [curItem].ImageIndex != -1)
					{
						g.DrawImage (ImageList.Images [completionData [curItem].ImageIndex],
						             new RectangleF (2, yPos, imageWidth, itemHeight));
						xPos = imageWidth + 4;
					}

					// draw text

					g.DrawString (completionData [curItem].Text, Font, YukiTheme_VisualPascalABCPlugin.Colors.clrBrush, xPos, yPos + dfont);
				}

				yPos += itemHeight;
				++curItem;
			}

			g.DrawRectangle (SystemPens.Control, new Rectangle (0, 0, Width - 1, Height - 1));
		}

		protected override void OnMouseDown (MouseEventArgs e)
		{
			float yPos = 1;
			var curItem = firstItem;
			float itemHeight = ItemHeight;

			while (curItem < completionData.Length && yPos < Height)
			{
				var drawingBackground = new RectangleF (1, yPos, Width - 2, itemHeight);
				if (drawingBackground.Contains (e.X, e.Y))
				{
					SelectIndex (curItem);
					break;
				}

				yPos += itemHeight;
				++curItem;
			}
		}

		protected override void OnPaintBackground (PaintEventArgs pe)
		{
		}

		protected virtual void OnSelectedItemChanged (EventArgs e)
		{
			if (SelectedItemChanged != null) SelectedItemChanged (this, e);
		}

		protected virtual void OnFirstItemChanged (EventArgs e)
		{
			if (FirstItemChanged != null) FirstItemChanged (this, e);
		}

		public event EventHandler SelectedItemChanged;
		public event EventHandler FirstItemChanged;
	}
}