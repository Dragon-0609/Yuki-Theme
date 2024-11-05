using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using YukiTheme.Engine;
using YukiTheme.Style.Helpers;
using YukiTheme.Tools;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace YukiTheme.Components;

public partial class StickerWindow : SnapWindow
{
	private Point _anchorPoint;

	private readonly PositionCalculator _calculator;

	private GridWindow _grid;

	private readonly AlignmentHelper _helper;
	private bool _inDrag;
	internal PointF RelativePosition;

	public StickerWindow()
	{
		InitializeComponent();
		_calculator = new PositionCalculator(this);
		CanUsePercents = true;
		_helper = new AlignmentHelper(this);
	}

	private StickerPosition PositionInfo
	{
		get => new(RelativePosition.X, RelativePosition.Y, GetAlign());
		set
		{
			var position = value;
			RelativePosition = new PointF(position.X, position.Y);
			_helper.SetAlign(position.Align);
		}
	}

	internal void SetSize()
	{
		var dimensionSize = ImageHelper.CalculateDimension(IDEAlterer.GetSticker.Size);
		Width = dimensionSize.Width;
		Height = dimensionSize.Height;
#if LOG
            Console.WriteLine($"width: {Width}, height: {Height}");
#endif
	}


	internal void SaveData()
	{
		DatabaseManager.Save(SettingsConst.STICKER_POSITION, ConvertToSave());
	}

	private string ConvertToSave()
	{
		return StickerPositionConverter.ToString(PositionInfo);
	}

	internal void LoadData()
	{
		var data = DatabaseManager.Load(SettingsConst.STICKER_POSITION, "");
		PositionInfo = ConvertToLoad(data);
		SetBorderOutline();
	}

	private StickerPosition ConvertToLoad(string data)
	{
		var position = StickerPositionConverter.FromString(data);
		return position;
	}

	internal void SetBorderOutline()
	{
		BorderOutlineX = RelativePosition.X;
		BorderOutlineY = RelativePosition.Y;
	}


	private void Sticker_MouseEnter(object sender, MouseEventArgs e)
	{
		if (DatabaseManager.Load(SettingsConst.HIDE_ON_HOVER))
		{
			var anim = new DoubleAnimation(0.01, TimeSpan.FromMilliseconds(300));
			Sticker.BeginAnimation(OpacityProperty, anim);
		}
	}

	private void Sticker_MouseLeave(object sender, MouseEventArgs e)
	{
		if (DatabaseManager.Load(SettingsConst.HIDE_ON_HOVER))
		{
			var anim = new DoubleAnimation(1, TimeSpan.FromMilliseconds(300));
			Sticker.BeginAnimation(OpacityProperty, anim);
		}
	}

	protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
	{
		if (e.ClickCount == 1)
			StartDragging(e);
		else if (e.ClickCount == 2) MouseDoubleClickAction();
	}

	private void StartDragging(MouseButtonEventArgs e)
	{
		if (DatabaseManager.Load(SettingsConst.ALLOW_POSITIONING) && !DatabaseManager.Load(SettingsConst.HIDE_ON_HOVER))
		{
			_anchorPoint = e.GetPosition(this);
			_inDrag = true;
			CaptureMouse();
			e.Handled = true;
			_calculator.PrepareData();
			_grid = new GridWindow
			{
				AlignX = AlignmentX.Left,
				AlignY = AlignmentY.Top,
				BorderOutlineX = 0,
				BorderOutlineY = 0
			};
			if (Target != null)
			{
				_grid.SetOwner(Target);
				_grid.Foreground = ColorReference.BorderColor.ToWPFColor().ToBrush();
				var rect = Target.GetAbsoluteRect();
				_grid.Width = rect.Width;
				_grid.Height = rect.Height;
			}
			else
			{
				_grid.SetOwner(TargetForm);
				_grid.Foreground = ColorReference.BorderColor.ToWPFColor().ToBrush();
				_grid.Width = TargetForm.Width;
				_grid.Height = TargetForm.Height;
			}

			_grid.ResetPosition();
			_grid.Show();
			_grid.Focus();
			Focus();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_inDrag)
		{
			var currentPoint = e.GetPosition(this);
			var pointX = Left + currentPoint.X - _anchorPoint.X;
			var pointY = Top + currentPoint.Y - _anchorPoint.Y;
			_calculator.KeepBounds(ref pointX, ref pointY);
			Left = pointX;
			Top = pointY; // this is not changing in your case	
		}
	}

	protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
	{
		if (_inDrag)
		{
			ReleaseMouseCapture();
			_inDrag = false;
			e.Handled = true;
			_grid.Close();

			var point = new Point(0, 0);

			if (Target != null)
				point = e.GetPosition(Owner);
			else if (TargetForm != null)
				point = TargetForm.PointToClient(System.Windows.Forms.Cursor.Position).ToWPFPoint();
			else
				throw new NullReferenceException("Owner wasn't set");

			_calculator.SetAligns(point);
			_calculator.SaveRelatedPosition(point);
		}
	}

	private void MouseDoubleClickAction()
	{
		// Console.WriteLine("Saving");
		if (!DatabaseManager.Load(SettingsConst.HIDE_ON_HOVER) && DatabaseManager.Load(SettingsConst.ALLOW_POSITIONING))
		{
			Console.WriteLine("Saving");
			SaveData();
			ResetPosition();
		}
	}

	internal void SetImage(ImageSource source)
	{
		Sticker.Source = source;
	}

	private AnchorStyles GetAlign()
	{
		var x = AlignX switch
		{
			AlignmentX.Left => AnchorStyles.Left,
			AlignmentX.Center => AnchorStyles.Left | AnchorStyles.Right,
			AlignmentX.Right => AnchorStyles.Right,
			_ => AnchorStyles.None
		};
		var y = AlignY switch
		{
			AlignmentY.Top => AnchorStyles.Top,
			AlignmentY.Center => AnchorStyles.Top | AnchorStyles.Bottom,
			AlignmentY.Bottom => AnchorStyles.Bottom,
			_ => AnchorStyles.None
		};
		AnchorStyles res;
		if (x == AnchorStyles.None)
		{
			res = y;
		}
		else
		{
			res = x;
			res |= y;
		}

		return res;
	}

	internal void FocusBack(object sender, MouseButtonEventArgs e)
	{
		if (!DatabaseManager.Load(SettingsConst.ALLOW_POSITIONING))
		{
			if (Target != null)
				Target.Focus();
			else if (TargetForm != null)
				TargetForm.Focus();
		}
	}
}