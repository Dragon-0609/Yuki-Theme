using System;
  using System.Drawing;                                                                                                                                                                                        
  using System.Windows.Forms;
  using YukiTheme.Components.Windows;
  using YukiTheme.Engine;
  using YukiTheme.Tools;

  namespace YukiTheme.Components.StatusBar;                                                                                                                                                                    

  public class DownloadBar : StatusBarItem
  {
      private Label _label;
      private ThinProgressBar _progress;
      private Button _cancel;
      private ToolStripControlHost _host;

      public DownloadBar()
      {
          DelayedCall.Run(1f, () =>
          {
              UpdateDownloader.Instance.OnProgressUpdated += SetProgress;
              UpdateDownloader.Instance.OnStarted += Show;
              UpdateDownloader.Instance.OnCompleted += Hide;
              UpdateDownloader.Instance.OnCanceled += Hide;
              UpdateDownloader.Instance.OnNetworkError += Hide;
          });
          
          ColorChanger.Instance.UpdatedColors += ChangeColor;

          DelayedCall.Run(0.2f, ChangeColor);
          Hide();
      }

      private void ChangeColor()
      {
          _progress.TrackColor = ColorReference.BackgroundDefaultColor;
          _progress.FillColor = ColorReference.KeywordColor;
          _progress.OutlineColor = Color.FromArgb(100, ColorReference.KeywordColor);
          
          _cancel.FlatAppearance.MouseOverBackColor = ColorReference.BackgroundClickColor;
      }

      private void Show()
      {
          _host.Visible = true;
      }

      private void Hide(string obj)
      {
          Hide();
      }

      private void Hide()
      {
          _host.Visible = false;
      }

      protected override ToolStripItem CreateItem()
      {
          var layout = new TableLayoutPanel
          {
              Dock = DockStyle.Fill,
              ColumnCount = 3,
              RowCount = 1,
              Margin = Padding.Empty,
              Padding = Padding.Empty,
              BackColor = Color.Transparent
          };
          layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));      // label
          layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // progress stretches
          layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));      // x button

          _label = new Label
          {
              AutoSize = true,
              Dock = DockStyle.Fill,
              TextAlign = ContentAlignment.MiddleLeft,
              Text = "Downloading...",
              Margin = Padding.Empty,
              Cursor = Cursors.Hand,
          };
          _label.Click += TryOpenDownloadWindow;
          
          _progress = new ThinProgressBar
          {
              Dock = DockStyle.Fill,
              Maximum = 100,
              FillColor = Color.FromArgb(0xE0, 0x6C, 0x6C),
              Margin = new Padding(4, 0, 4, 0),
              OutlineWidth = 0.1f,
              Cursor = Cursors.Hand,
          };
          _progress.Click += TryOpenDownloadWindow;

          
          _cancel = new Button
          {
              Text = "✕",
              AutoSize = false,
              FlatStyle = FlatStyle.Flat,
              Margin = Padding.Empty,
              Size = new Size(20, 20),
          };
          _cancel.FlatAppearance.BorderSize = 0;
          _cancel.Click += CancelClicked; 

          layout.Controls.Add(_label,    0, 0);
          layout.Controls.Add(_progress, 1, 0);
          layout.Controls.Add(_cancel,   2, 0);

          var panel = new Panel
          {
              AutoSize = false, Size = new Size(260, 18),
              BackColor = Color.Transparent
          };
          panel.Controls.Add(layout);

          _host = new ToolStripControlHost(panel)
          {
              AutoSize = false,
              Size = new Size(260, 18),
              BackColor = Color.Transparent
          };
          
          return _host;
      }

      private void TryOpenDownloadWindow(object sender, EventArgs e)
      {
          UpdateManager.Instance.OpenDownloadWindow();
      }

      private void CancelClicked(object sender, EventArgs e)
      {
          IDEConsole.Log($"Cancel clicked");
          UpdateDownloader.Instance.Cancel();
      }

      public void SetProgress(DownloadInfo info)
      {
          _label.Text = $"Downloading...";
          _progress.Value = Math.Min(100, Math.Max(0, info.Percent));
      }
  }