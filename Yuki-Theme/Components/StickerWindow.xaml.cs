using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using YukiTheme.Engine;
using YukiTheme.Style.Helpers;
using YukiTheme.Tools;
using Drawing = System.Drawing;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace YukiTheme.Components
{
    public partial class StickerWindow : SnapWindow
    {
        internal Drawing.PointF RelativePosition;

        private Point _anchorPoint;
        private bool _inDrag;

        private PositionCalculator _calculator;

        private GridWindow _grid;

        public StickerWindow()
        {
            InitializeComponent();
            _calculator = new PositionCalculator(this);
            CanUsePercents = true;
        }

        public static StickerWindow CreateStickerControl(Form parent)
        {
            StickerWindow stickerWindow = new StickerWindow();
            stickerWindow.SetOwner(parent);
            stickerWindow.ReadData();
            return stickerWindow;
        }


        public void SetSize()
        {
            Drawing.Size dimensionSize = ImageHelper.CalculateDimension(IDEAlterer.GetSticker.Size);
            Width = dimensionSize.Width;
            Height = dimensionSize.Height;
            Console.WriteLine($"width: {Width}, height: {Height}");
        }

        public void ReadData()
        {
            RelativePosition = Drawing.PointF.Empty;
            AnchorStyles align;
            RelativeUnit unit = RelativeUnit.Pixel;
            align = GetPositioningValues(unit);
            AlignX = align.ConvertToX();
            AlignY = align.ConvertToY();

            SetBorderOutline();
            ResetPosition();
        }


        private string ConvertLocationToSave()
        {
            string s = "";
            // $"{_relativePosition.X}|{_relativePosition.Y}|{(int)(AlignX.ConvertToAlign () | AlignY.ConvertToAlign ())}|{(int)Settings.unit}";
            return s;
        }

        internal void SaveData()
        {
            // Settings.database.UpdateData (SettingsConst.STICKER_POSITION, ConvertLocationToSave ());
        }

        private AnchorStyles GetPositioningValues(RelativeUnit unit)
        {
            AnchorStyles align;
            string data = "";
            // string data = Settings.database.ReadData (SettingsConst.STICKER_POSITION, "");
            if (data != "")
            {
                try
                {
                    string[] cc = data.Split('|');
                    float x = float.Parse(cc[0]);
                    float y = float.Parse(cc[1]);
                    RelativePosition = new Drawing.PointF(x, y);
                    align = (AnchorStyles)(int.Parse(cc[2]));
                    RelativeUnit un = (RelativeUnit)(int.Parse(cc[3]));

                    if (unit != un)
                    {
                        float tunitx = Convert.ToInt32(Owner.Width) / 100f;
                        float tunity = Convert.ToInt32(Owner.Height) / 100f;
                        if (un == RelativeUnit.Percent)
                        {
                            RelativePosition = new Drawing.PointF(RelativePosition.X * tunitx, RelativePosition.Y * tunity);
                        }
                        else
                        {
                            RelativePosition = new Drawing.PointF(RelativePosition.X / tunitx, RelativePosition.Y / tunity);
                        }
                    }
                }
                catch (Exception)
                {
                    align = SetDefaultPositions(unit);
                }
            }
            else
            {
                align = SetDefaultPositions(unit);
            }

            return align;
        }

        private AnchorStyles SetDefaultPositions(RelativeUnit unit)
        {
            RelativePosition = unit == RelativeUnit.Pixel ? new Drawing.Point(BORDER_OUTLINE_X, PLUGIN_BORDER_OUTLINE_Y) : GetMargin();
            return AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private static Drawing.Point GetMargin()
        {
            int margin = 10;
            return new Drawing.Point(margin, margin);
        }

        internal void SetBorderOutline()
        {
            BorderOutlineX = RelativePosition.X;
            BorderOutlineY = RelativePosition.Y;

            // borderOutlineX = borderOutlineY = BORDER_OUTLINE;
        }


        private void Sticker_MouseEnter(object sender, MouseEventArgs e)
        {
            if (false)
            {
                DoubleAnimation anim = new DoubleAnimation(0.01, TimeSpan.FromMilliseconds(300));
                Sticker.BeginAnimation(OpacityProperty, anim);
            }
        }

        private void Sticker_MouseLeave(object sender, MouseEventArgs e)
        {
            if (false)
            {
                DoubleAnimation anim = new DoubleAnimation(1, TimeSpan.FromMilliseconds(300));
                Sticker.BeginAnimation(OpacityProperty, anim);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                StartDragging(e);
            }
            else if (e.ClickCount == 2)
            {
                MouseDoubleClickAction();
            }
        }

        private void StartDragging(MouseButtonEventArgs e)
        {
            if (false)
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
                    _grid.Foreground = ColorReference.BorderColor().ToWPFColor().ToBrush();
                    Rect rect = Target.GetAbsoluteRect();
                    _grid.Width = rect.Width;
                    _grid.Height = rect.Height;
                }
                else
                {
                    _grid.SetOwner(TargetForm);
                    _grid.Foreground = ColorReference.BorderColor().ToWPFColor().ToBrush();
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
                Point currentPoint = e.GetPosition(this);
                double pointX = this.Left + currentPoint.X - _anchorPoint.X;
                double pointY = this.Top + currentPoint.Y - _anchorPoint.Y;
                _calculator.KeepBounds(ref pointX, ref pointY);
                this.Left = pointX;
                this.Top = pointY; // this is not changing in your case	
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

                Point point = new Point(0, 0);

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
            // if (!Settings.hideOnHover && Settings.positioning)
            // {
            // SaveData();
            // }
        }

        public void SetImage(ImageSource source)
        {
            Sticker.Source = source;
        }
    }
}