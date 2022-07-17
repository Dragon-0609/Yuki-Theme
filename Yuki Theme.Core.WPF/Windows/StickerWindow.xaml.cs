﻿using System;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using WpfAnimatedGif;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Interfaces;
using Drawing = System.Drawing;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class StickerWindow : SnapWindow, Sticker.IView
	{
		private Drawing.PointF _relativePosition;

		private Sticker.IModel _model;

		private Sticker.IPresenter _presenter;

		public StickerWindow ()
		{
			InitializeComponent ();
			_model = new StickerModel ();
			_presenter = new StickerPresenter (_model, this);
		}
		
		
		public static StickerWindow CreateStickerControl (Window parent)
		{
			StickerWindow stickerWindow = new StickerWindow
			{
				target = parent,
				Owner = parent
			};
			stickerWindow.ReadData();
			return stickerWindow;
		}

		public static StickerWindow CreateStickerControl (Form parentForm)
		{
			StickerWindow stickerWindow = new StickerWindow
			{
				targetForm = parentForm
			};
			WindowInteropHelper helper = new WindowInteropHelper (stickerWindow)
			{
				Owner = parentForm.Handle
			};
			stickerWindow.ReadData();
			return stickerWindow;
		}

		public void LoadSticker () => _presenter.LoadSticker ();

		public void SetStickerSize ()
		{
			
		}

		public void ChangeVisibility (Visibility visibility)
		{
			Visibility = visibility;
		}

		void Sticker.IView.ChangeImage (Drawing.Image image)
		{
			ChangeImage (image);
		}

		public void SetSize (Drawing.Size size)
		{
			Width = size.Width;
			Height = size.Height;
		}

		private void ChangeImage (Drawing.Image image)
		{
			if (Equals (image.RawFormat, ImageFormat.Gif))
			{
				ImageBehavior.SetAnimatedSource (Sticker, image.ToWPFGIFImage ());
			}
			else
				Sticker.Source = image.ToWPFImage ();
		}

		private void FocusBack (object sender, MouseButtonEventArgs e)
		{
			if (!Settings.positioning)
			{
				if (target != null)
					target.Focus ();
				else if (targetForm != null)
					targetForm.Focus ();
			}
		}
		
		
		public void ReadData ()
		{
			_relativePosition = Drawing.PointF.Empty;
			AnchorStyles align;
			RelativeUnit unit = Settings.unit;
			string data = Settings.database.ReadData (SettingsConst.STICKER_POSITION, "");
			if (data != "")
			{
				try
				{
					string [] cc = data.Split ('|');
					float x = float.Parse (cc [0]);
					float y = float.Parse (cc [1]);
					_relativePosition = new Drawing.PointF (x, y);
					align = (AnchorStyles) (int.Parse (cc [2]));
					RelativeUnit un = (RelativeUnit) (int.Parse (cc [3]));
					if (unit != un)
					{
						float tunitx = Convert.ToInt32 (Owner.Width) / 100f;
						float tunity = Convert.ToInt32 (Owner.Height) / 100f;
						if (un == RelativeUnit.Percent)
						{
							_relativePosition = new Drawing.PointF (_relativePosition.X * tunitx, _relativePosition.Y * tunity);
						} else
						{
							_relativePosition = new Drawing.PointF (_relativePosition.X / tunitx, _relativePosition.Y / tunity);
						}
					}
				} catch (Exception)
				{
					_relativePosition = unit == RelativeUnit.Pixel ? new Drawing.Point(BORDER_OUTLINE)  : new Drawing.Point (1, 1);
					align = AnchorStyles.Bottom | AnchorStyles.Right;
				}
			} else
			{
				_relativePosition = unit == RelativeUnit.Pixel ? new Drawing.Point (BORDER_OUTLINE) : new Drawing.Point (1, 1);
				align = AnchorStyles.Bottom | AnchorStyles.Right;
			}

			AlignX = align.ConvertToX ();
			AlignY = align.ConvertToY ();

			SetBorderOutline ();
		}

		private void SetBorderOutline ()
		{
			if (Settings.useCustomSticker)
			{
				borderOutlineX = _relativePosition.X;
				borderOutlineY = _relativePosition.Y;
			} else
			{
				borderOutlineX = borderOutlineY = BORDER_OUTLINE;
			}
		}

		public void LoadImage (Drawing.Image image) => _presenter.LoadImage (image);
	}
}