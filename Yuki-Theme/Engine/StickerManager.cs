using System.Windows;
using System.Windows.Media;
using YukiTheme.Components;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class StickerManager : WindowManager
{
    private StickerWindow _sticker;

    internal override void Show()
    {
        _sticker = new StickerWindow()
        {
            AlignX = AlignmentX.Right,
            AlignY = AlignmentY.Bottom,
            BorderOutlineX = 20,
            BorderOutlineY = 35,
        };
        var targetForm = IDEAlterer.Instance.Form1;
        _sticker.SetOwner(targetForm);
        _sticker.AutoSize = false;
        _sticker.Show();
        _sticker.Focus();
        IDEAlterer.Instance.Form1.Focus();
        UpdateSticker();
        _sticker.LoadData();
        _sticker.ResetPosition();
        PluginEvents.Instance.StickerMarginReset += () =>
        {
            _sticker.LoadData();
            _sticker.ResetPosition();
        };
        UpdateCap();
    }

    internal override bool IsWindowNull() => _sticker == null;

    internal override void SetVisibility(Visibility visibility)
    {
        _sticker.Visibility = visibility;
    }

    internal void UpdateSticker()
    {
        if (!IsWindowNull())
        {
            if (IDEAlterer.HasSticker)
            {
                _sticker.SetImage(IDEAlterer.GetStickerWPF);
                _sticker.SetSize();
            }

            UpdateOpacity();
        }

        _sticker.ResetPosition();
        UpdateVisibility();
    }

    protected override bool CanShow() => IDEAlterer.CanShowSticker;

    private void UpdateOpacity()
    {
        if (!IsWindowNull())
        {
            _sticker.Opacity = 1;
        }
    }

    internal override void ReloadSettings()
    {
        UpdateVisibility();
        UpdateCap();
    }

    private void UpdateCap()
    {
        if (!IsWindowNull())
        {
            _sticker.SetSize();
            _sticker.ResetPosition();
        }
    }
}