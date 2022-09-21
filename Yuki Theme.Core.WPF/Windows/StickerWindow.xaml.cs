using System;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WpfAnimatedGif;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Interfaces;
using Drawing = System.Drawing;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class StickerWindow : SnapWindow, Sticker.IView
	{
		internal Drawing.PointF _relativePosition;

		private Sticker.IModel _model;

		private Sticker.IPresenter _presenter;
		private Point              _anchorPoint;
		private bool               _inDrag;

		private PositionCalculator _calculator;

		private GridWindow _grid;

		public Action <string> WriteToConsole;

		public StickerWindow ()
		{
			InitializeComponent ();
			_model = new StickerModel ();
			_presenter = new StickerPresenter (_model, this);
			_calculator = new PositionCalculator (this);
		}


		public static StickerWindow CreateStickerControl (Window parent)
		{
			StickerWindow stickerWindow = new StickerWindow ();
			stickerWindow.SetOwner (parent);
			stickerWindow.ReadData ();
			return stickerWindow;
		}

		public static StickerWindow CreateStickerControl (Form parent)
		{
			StickerWindow stickerWindow = new StickerWindow ();
			stickerWindow.SetOwner (parent);
			stickerWindow.ReadData ();
			return stickerWindow;
		}

		public void LoadSticker () => _presenter.LoadSticker ();

		public void SetStickerSize () => _presenter.SetStickerSize ();

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
			} else
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
			align = GetPositioningValues (unit);
			AlignX = align.ConvertToX ();
			AlignY = align.ConvertToY ();

			SetBorderOutline ();
		}


		private string ConvertLocationToSave ()
		{
			string s =
				$"{_relativePosition.X}|{_relativePosition.Y}|{(int)(AlignX.ConvertToAlign () | AlignY.ConvertToAlign ())}|{(int)Settings.unit}";
			return s;
		}

		internal void SaveData ()
		{
			Settings.database.UpdateData (SettingsConst.STICKER_POSITION, ConvertLocationToSave ());
		}

		private AnchorStyles GetPositioningValues (RelativeUnit unit)
		{
			AnchorStyles align;
			string data = Settings.database.ReadData (SettingsConst.STICKER_POSITION, "");
			if (data != "")
			{
				try
				{
					string [] cc = data.Split ('|');
					float x = float.Parse (cc [0]);
					float y = float.Parse (cc [1]);
					_relativePosition = new Drawing.PointF (x, y);
					align = (AnchorStyles)(int.Parse (cc [2]));
					RelativeUnit un = (RelativeUnit)(int.Parse (cc [3]));
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
					align = SetDefaultPositions (unit);
				}
			} else
			{
				align = SetDefaultPositions (unit);
			}
			return align;
		}
		private AnchorStyles SetDefaultPositions (RelativeUnit unit)
		{
			_relativePosition = unit == RelativeUnit.Pixel ? 
				new Drawing.Point (BORDER_OUTLINE_X, Helper.mode == ProductMode.Plugin ? PLUGIN_BORDER_OUTLINE_Y : BORDER_OUTLINE_Y) : GetMargin ();
			return AnchorStyles.Bottom | AnchorStyles.Right;
		}

		private static Drawing.Point GetMargin ()
		{
			int margin = 1;
			if (Helper.mode == ProductMode.Plugin)
			{
				margin = 10;
			}
			return new Drawing.Point (margin, margin);
		}

		internal void SetBorderOutline ()
		{

			borderOutlineX = _relativePosition.X;
			borderOutlineY = _relativePosition.Y;

			// borderOutlineX = borderOutlineY = BORDER_OUTLINE;
		}

		public void LoadImage (Drawing.Image image) => _presenter.LoadImage (image);
		public void Release () => _model.ReleaseImages ();


		private void Sticker_MouseEnter (object sender, MouseEventArgs e)
		{
			if (Settings.hideOnHover)
			{
				DoubleAnimation anim = new DoubleAnimation (0.01, TimeSpan.FromMilliseconds (Settings.hideDelay));
				Sticker.BeginAnimation (OpacityProperty, anim);
			}
		}
		private void Sticker_MouseLeave (object sender, MouseEventArgs e)
		{
			if (Settings.hideOnHover)
			{
				DoubleAnimation anim = new DoubleAnimation (1, TimeSpan.FromMilliseconds (Settings.hideDelay));
				Sticker.BeginAnimation (OpacityProperty, anim);
			}
		}
		/*private bool IsMouseInside ()
		{
			Point cursor = Mouse.GetPosition (this);
			cursor.X -= 10;
			cursor.Y -= 10;
			cursor.X = -cursor.X + RenderSize.Width;
			cursor.Y = -cursor.Y + RenderSize.Height;
			return (0 < cursor.X && cursor.X < RenderSize.Width) && (0 < cursor.Y && cursor.Y < RenderSize.Height);
		}*/


		public void UpdateStickerVisibility ()
		{
			Visibility = _presenter.StickerAvailable () ? Visibility.Visible : Visibility.Hidden;
		}

		protected override void OnMouseLeftButtonDown (MouseButtonEventArgs e)
		{
			if (e.ClickCount == 1)
			{
				StartDragging (e);
			} else if (e.ClickCount == 2)
			{
				MouseDoubleClickAction ();
			}
		}
		private void StartDragging (MouseButtonEventArgs e)
		{
			if (Settings.positioning && !Settings.hideOnHover)
			{
				_anchorPoint = e.GetPosition (this);
				_inDrag = true;
				CaptureMouse ();
				e.Handled = true;
				_calculator.PrepareData ();
				_grid = new GridWindow
				{
					AlignX = AlignmentX.Left,
					AlignY = AlignmentY.Top,
					borderOutlineX = 0,
					borderOutlineY = 0
				};
				if (target != null)
				{
					_grid.SetOwner (target);
					_grid.Foreground = WPFHelper.borderBrush;
					Rect rect = target.GetAbsoluteRect ();
					_grid.Width = rect.Width;
					_grid.Height = rect.Height;
				} else
				{
					_grid.SetOwner (targetForm);
					_grid.Foreground = ColorKeeper.bgBorder.ToWPFColor ().ToBrush ();
					_grid.Width = targetForm.Width;
					_grid.Height = targetForm.Height;
				}

				_grid.ResetPosition ();
				_grid.Show ();
				_grid.Focus ();
				Focus ();
			}
		}

		protected override void OnMouseMove (MouseEventArgs e)
		{
			if (_inDrag)
			{
				Point currentPoint = e.GetPosition (this);
				double pointX = this.Left + currentPoint.X - _anchorPoint.X;
				double pointY = this.Top + currentPoint.Y - _anchorPoint.Y;
				_calculator.KeepBounds (ref pointX, ref pointY);
				this.Left = pointX;
				this.Top = pointY; // this is not changing in your case	
			}
		}

		protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e)
		{
			if (_inDrag)
			{
				ReleaseMouseCapture ();
				_inDrag = false;
				e.Handled = true;
				_grid.Close ();

				Point point = new Point (0, 0);

				if (target != null)
					point = e.GetPosition (Owner);
				else if (targetForm != null)
					point = targetForm.PointToClient (System.Windows.Forms.Cursor.Position).ToWPFPoint ();
				else
					throw new NullReferenceException ("Owner wasn't set");

				_calculator.SetAligns (point);
				_calculator.SaveRelatedPosition (point);
				Console.WriteLine (_relativePosition.ToString ());
			}
		}

		private void MouseDoubleClickAction ()
		{
			if (!Settings.hideOnHover && Settings.positioning)
			{
				SaveData ();
			}
		}
	}
}