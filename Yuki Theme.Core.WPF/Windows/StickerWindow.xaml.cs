using System;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using WpfAnimatedGif;
using Yuki_Theme.Core.WPF.Controls;
using Drawing = System.Drawing;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class StickerWindow : SnapWindow
	{
		private Drawing.Image  originalImage = null;
		private Drawing.Image  renderImage   = null;
		private Drawing.PointF _relativePosition;

		public StickerWindow ()
		{
			InitializeComponent ();
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

		public void LoadSticker ()
		{
			if (renderImage != null)
			{
				renderImage.Dispose ();
				renderImage = null;
			}
			
			if (Settings.swSticker)
			{
				if (Settings.useCustomSticker && Settings.customSticker.Exist ())
				{
					renderImage = Drawing.Image.FromFile (Settings.customSticker);
				} else
				{
					if (originalImage != null)
					{
						if (API.currentTheme.StickerOpacity != 100)
						{
							renderImage = Helper.SetOpacity (originalImage, API.currentTheme.StickerOpacity);
							originalImage.Dispose ();
						} else
							renderImage = originalImage;
						Visibility = Visibility.Visible;
					} else
					{
						renderImage = null;
						Visibility = Visibility.Hidden;
					}
				}

				if (renderImage != null)
				{
					SetStickerSize ();
					ChangeImage (renderImage);
					UpdatePosition ();
				} else
				{
					Visibility = Visibility.Hidden;
				}
			} else
			{
				Visibility = Visibility.Hidden;
			}
		}

		public void LoadImage (Drawing.Image image)
		{
			originalImage = image;
			LoadSticker ();
		}

		private void UpdatePosition ()
		{
			ResetPosition ();
		}

		public void SetStickerSize ()
		{
			if (renderImage != null)
			{
				Drawing.Size dimensionSize = Helper.CalculateDimension (renderImage.Size);
				Width = dimensionSize.Width;
				Height = dimensionSize.Height;
			}
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
			string data = Settings.database.ReadData (Settings.STICKERPOSITION, "");
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
	}
}