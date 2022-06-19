using System.Windows;
using System.Windows.Forms.Integration;
using FastColoredTextBoxNS;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class CodeTextboxHost : WindowsFormsHost
	{
		public FastColoredTextBox box = new FastColoredTextBox ();

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register (
			"Text", typeof (string), typeof (CodeTextboxHost),
			new PropertyMetadata ("", new PropertyChangedCallback (
				                      (d, e) =>
				                      {
					                      var textBoxHost = d as CodeTextboxHost;
					                      if (textBoxHost != null &&
					                          textBoxHost.box != null)
					                      {
						                      textBoxHost.box.Text =
							                      textBoxHost.GetValue (e.Property) as
								                      string;
					                      }
				                      }), null));

		public CodeTextboxHost ()
		{
			Child = box;

			box.TextChanged += BoxTextChanged;
			
			box.AllowDrop = false;
			box.AllowMacroRecording = false;
			box.AutoScrollMinSize = new System.Drawing.Size (255, 66);
			box.Cursor = System.Windows.Forms.Cursors.IBeam;
			box.Font = new System.Drawing.Font ("Consolas", 12F);
			box.HighlightFoldingIndicator = false;
			box.IsReplaceMode = false;
			box.LineInterval = 4;
			box.Paddings = new System.Windows.Forms.Padding (10, 0, 0, 0);
			box.PreferredLineWidth = 70;
			box.ReadOnly = true;
			box.ReservedCountOfLineNumberChars = 2;
			box.SelectionHighlightingForLineBreaksEnabled = false;
			box.ShowFoldingLines = true;
			box.Text = "begin\r\nWriteln(\'Hello World\');\r\nend.";
			box.Zoom = 100;
			
			box.SendToBack ();
		}

		private void BoxTextChanged (object sender, TextChangedEventArgs e)
		{
			SetValue (TextProperty, box.Text);
		}

		public string Text
		{
			get { return (string)GetValue (TextProperty); }
			set { SetValue (TextProperty, value); }
		}
	}
}