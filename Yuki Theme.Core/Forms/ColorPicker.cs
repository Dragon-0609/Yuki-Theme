using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MechanikaDesign.WinForms.UI.ColorPicker;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme.Core.Forms
{
	public partial class ColorPicker : Form
	{
		#region Fields

		private HslColor   colorHsl    = HslColor.FromAhsl(0xff);
		private ColorModes colorMode   = ColorModes.Hue;
		public  Color      colorRgb    = Color.Empty;
		private bool       lockUpdates = false;
		private int        cbox        = 3;
		// private MForm      form;
		public bool allowSave = true;
		#endregion
		
		public ColorPicker (/*MForm fm*/)
		{
			InitializeComponent ();
			// form = fm;
			this.colorBox2D.ColorMode = this.colorMode;
			this.colorSlider.ColorMode = this.colorMode;
			this.StartPosition = FormStartPosition.CenterParent;
			Icon = Helper.GetYukiThemeIcon (new Size (32, 32));
			tabHexagon.Enter += TabHexagonOnEnter;
			tabWheel.Enter += TabWheelOnEnter;
			tabColorBox.Enter += TabColorBoxOnEnter;
			OKBTN.Text = CLI.Translate ("messages.buttons.select");
			CNBTN.Text = CLI.Translate ("download.cancel");
		}

		public Color MainColor
		{
			get => labelCurrentColor.BackColor;
			set
			{
				colorRgb = value;
				labelCurrentColor.BackColor = colorRgb;
				cbox = 3;
				tabControlMain.SelectedIndex = 0;
				TabColorBoxOnEnter(this, EventArgs.Empty);
				UpdateColorFields ();
			}
		}

		private void colorHexagon_ColorChanged (object sender, ColorChangedEventArgs args)
		{
			labelCurrentColor.BackColor = colorHexagon.SelectedColor;
			//	Dragon.ColorEd.NsQ=colorHexagon.SelectedColor;
			//		Console.WriteLine (colorHexagon.SelectedColor.R+" "+colorHexagon.SelectedColor.G+" "+colorHexagon.SelectedColor.B);
			textboxHexColor.Text = ColorTranslator.ToHtml (colorHexagon.SelectedColor);
		}

		private void colorWheel_ColorChanged(object sender, EventArgs e)
        {
            labelCurrentColor.BackColor = colorWheel.Color;
//			Dragon.ColorEd.NsQ=colorWheel.Color;
            textboxHexColor.Text = ColorTranslator.ToHtml(colorWheel.Color);
        }

        private void colorSlider_ColorChanged(object sender, ColorChangedEventArgs args)
        {
            if (!this.lockUpdates)
            {
                HslColor colorHSL = this.colorSlider.ColorHSL;
                this.colorHsl = colorHSL;
                this.colorRgb = this.colorHsl.RgbValue;
                this.lockUpdates = true;
                this.colorBox2D.ColorHSL = this.colorHsl;
                this.lockUpdates = false;
                labelCurrentColor.BackColor = this.colorRgb;
                textboxHexColor.Text = ColorTranslator.ToHtml(this.colorRgb);
//				Dragon.ColorEd.NsQ=colorRgb;
                UpdateColorFields();
            }  
        }

        private void colorBox2D_ColorChanged(object sender, ColorChangedEventArgs args)
        {
            if (!this.lockUpdates)
            {
                HslColor colorHSL = this.colorBox2D.ColorHSL;
                this.colorHsl = colorHSL;
                this.colorRgb = this.colorHsl.RgbValue;
                this.lockUpdates = true;
                this.colorSlider.ColorHSL = this.colorHsl;
                this.lockUpdates = false;
                labelCurrentColor.BackColor = this.colorRgb;
                textboxHexColor.Text = ColorTranslator.ToHtml(this.colorRgb);
                UpdateColorFields();
            }    
        }

        private void ColorModeChangedHandler(object sender, EventArgs e)
        {
            if (sender == this.radioRed)
            {
                this.colorMode = ColorModes.Red;
            }
            else if (sender == this.radioGreen)
            {
                this.colorMode = ColorModes.Green;
            }
            else if (sender == this.radioBlue)
            {
                this.colorMode = ColorModes.Blue;
            }
            else if (sender == this.radioHue)
            {
                this.colorMode = ColorModes.Hue;
            }
            else if (sender == this.radioSaturation)
            {
                this.colorMode = ColorModes.Saturation;
            }
            else if (sender == this.radioLuminance)
            {
                this.colorMode = ColorModes.Luminance;
            }
            this.colorSlider.ColorMode = this.colorMode;
            this.colorBox2D.ColorMode = this.colorMode;        
        }

        private void UpdateColorFields()
        {
            this.lockUpdates = true;
            this.numRed.Value = this.colorRgb.R;
            this.numGreen.Value = this.colorRgb.G;
            this.numBlue.Value = this.colorRgb.B;
			int val = (int)(((decimal)this.colorHsl.H) * 360M);
			if (val >= 360) {
				val = 359;
			}
			if (val < 0) {
				val = 0;
			}
			this.numHue.Value = val;
            this.numSaturation.Value = (int)(((decimal)this.colorHsl.S) * 100M);
            this.numLuminance.Value = (int)(((decimal)this.colorHsl.L) * 100M);
            this.lockUpdates = false;
        }

        private void UpdateRgbFields(Color newColor)
        {
            this.colorHsl = HslColor.FromColor(newColor);
            this.colorRgb = newColor;
            this.lockUpdates = true;
            this.numHue.Value = (int)(((decimal)this.colorHsl.H) * 360M);
            this.numSaturation.Value = (int)(((decimal)this.colorHsl.S) * 100M);
            this.numLuminance.Value = (int)(((decimal)this.colorHsl.L) * 100M);
            this.lockUpdates = false;
            this.colorSlider.ColorHSL = this.colorHsl;
            this.colorBox2D.ColorHSL = this.colorHsl;
			textboxHexColor.Text = ColorTranslator.ToHtml(this.colorRgb);
			labelCurrentColor.BackColor = colorRgb;
//			Dragon.ColorEd.NsQ=colorRgb;
        }

        private void UpdateHslFields(HslColor newColor)
        {
            this.colorHsl = newColor;
            this.colorRgb = newColor.RgbValue;
            this.lockUpdates = true;
            this.numRed.Value = this.colorRgb.R;
            this.numGreen.Value = this.colorRgb.G;
            this.numBlue.Value = this.colorRgb.B;
            this.lockUpdates = false;
            this.colorSlider.ColorHSL = this.colorHsl;
            this.colorBox2D.ColorHSL = this.colorHsl;
			textboxHexColor.Text = ColorTranslator.ToHtml(this.colorRgb);
			labelCurrentColor.BackColor = colorRgb;
//			Dragon.ColorEd.NsQ=colorRgb;
		//	UpdateRgbFields (colorRgb);
        }

        private void numRed_ValueChanged(object sender, EventArgs e)
        {
            if (!this.lockUpdates)
            {
                UpdateRgbFields(Color.FromArgb((int)this.numRed.Value, (int)this.numGreen.Value, (int)this.numBlue.Value));

            }
        }

        private void numGreen_ValueChanged(object sender, EventArgs e)
        {
            if (!this.lockUpdates)
            {
                UpdateRgbFields(Color.FromArgb((int)this.numRed.Value, (int)this.numGreen.Value, (int)this.numBlue.Value));
            }
        }

        private void numBlue_ValueChanged(object sender, EventArgs e)
        {
            if (!this.lockUpdates)
            {
                UpdateRgbFields(Color.FromArgb((int)this.numRed.Value, (int)this.numGreen.Value, (int)this.numBlue.Value));
            }
        }

        private void numHue_ValueChanged(object sender, EventArgs e)
        {
            if (!this.lockUpdates)
            {
                HslColor newColor = HslColor.FromAhsl((double)(((float)((int)this.numHue.Value)) / 360f), this.colorHsl.S, this.colorHsl.L);
                this.UpdateHslFields(newColor);
            }
        }

        private void numSaturation_ValueChanged(object sender, EventArgs e)
        {
            if (!this.lockUpdates)
            {
				HslColor newColor = HslColor.FromAhsl(this.colorHsl.A, this.colorHsl.H, (double)(((float)((int)this.numSaturation.Value)) / 100f), (double)(((float)((int)this.numLuminance.Value)) / 100f));
                this.UpdateHslFields(newColor);
            }
            
        }
		
        private void numLuminance_ValueChanged(object sender, EventArgs e)
        {
            if (!this.lockUpdates)
            {
				HslColor newColor = HslColor.FromAhsl(this.colorHsl.A, this.colorHsl.H, (double)(((float)((int)this.numSaturation.Value)) / 100f), (double)(((float)((int)this.numLuminance.Value)) / 100f));
                this.UpdateHslFields(newColor);
            }
        }

		private void hexSet(object sender, EventArgs e)
		{
			try {
				string s = textboxHexColor.Text;
				string h = "";
				if (s.StartsWith ("hex:")) {
					h = s.Substring (4);
					if (h.Length < 7) {
						for (int i = h.Length; i < 7; i++) {
							h += "0";

						}
					}
				} else {
					if (s.StartsWith ("#")) {
						h = s;
						if (h.Length < 7) {
							for (int i = h.Length; i < 7; i++) {
								h += "0";
							}
						}
					} else if (s.Contains ("#")) {
						if (s.IndexOf ("#") < 5) {
							h = s.Substring (s.IndexOf ("#"));
							if (h.Length < 7) {
								for (int i = h.Length; i < 7; i++) {
									h += "0";
								}
							}
						}
					} else {
						if (!s.Contains ("h") && !s.Contains ("x") && !s.Contains (":")) {
							h = "#" + s;
							if (h.Length < 7) {
								for (int i = h.Length; i < 7; i++) {
									h += "0";
								}
							}
						} 
					}
				}
				if (h.Length > 3) {
					Color rgb = ColorTranslator.FromHtml (h.ToUpper ());
					//		UpdateRGB (new RGB (rgb.R, rgb.G, rgb.B));
					if(cbox==3){
					UpdateRgbFields (rgb);
					}
					else if(cbox==2) {
						colorWheel.HslColor= rgb;
					}
					else if(cbox==1){
						labelCurrentColor.BackColor=rgb;
					}
				}
			} catch {

			}
		}

		private void color_Wheel_ColorChanged(object sender, EventArgs e)
		{
			labelCurrentColor.BackColor = colorWheel.Color;
			textboxHexColor.Text = ColorTranslator.ToHtml(colorWheel.Color);
		}

		private void TabHexagonOnEnter (object sender, EventArgs e)
		{
				cbox=1;
		}

		private void TabWheelOnEnter (object sender, EventArgs e)
		{
			colorWheel.HslColor=labelCurrentColor.BackColor;
			cbox=2;
		}

		private void TabColorBoxOnEnter (object sender, EventArgs e)
		{
			UpdateRgbFields(labelCurrentColor.BackColor);
			cbox=3;
		}

		private void OKBTN_Click (object sender, EventArgs e)
		{
			if (!allowSave)
			{
				MessageBox.Show ((IWin32Window)this, CLI.Translate ("colors.default.error.full"),
				                 CLI.Translate ("colors.default.error.short"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
			} else
				DialogResult = DialogResult.OK;
		}

		private void CNBTN_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void ColorPicker_Shown (object sender, EventArgs e)
		{
			tabColorBox.BackColor = tabWheel.BackColor = tabHexagon.BackColor = textboxHexColor.BackColor =
				OKBTN.BackColor = CNBTN.BackColor = tabControlMain.BackColor =
					BackColor = numBlue.BackColor = numGreen.BackColor = numRed.BackColor =
						numLuminance.BackColor = numSaturation.BackColor = numHue.BackColor = Helper.bgColor;
			labelCurrent.ForeColor = OKBTN.ForeColor = CNBTN.ForeColor = 
				OKBTN.FlatAppearance.BorderColor = CNBTN.FlatAppearance.BorderColor = tabColorBox.ForeColor = 
				textboxHexColor.ForeColor = labelHex.ForeColor = 
					numBlue.ForeColor = numGreen.ForeColor = numRed.ForeColor =
						numLuminance.ForeColor = numSaturation.ForeColor = numHue.ForeColor = ForeColor = Helper.fgColor;
			OKBTN.FlatAppearance.MouseOverBackColor = CNBTN.FlatAppearance.MouseOverBackColor = 
				numBlue.ButtonHighlightColor = numGreen.ButtonHighlightColor = numRed.ButtonHighlightColor =
				numLuminance.ButtonHighlightColor = numSaturation.ButtonHighlightColor = 
					numHue.ButtonHighlightColor = Helper.bgClick;

			numBlue.BorderColor = numGreen.BorderColor = numRed.BorderColor =
				numLuminance.BorderColor = numSaturation.BorderColor = numHue.BorderColor = Helper.bgBorder;
		}

		private void textboxHexColor_KeyPress (object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return) {
				e.Handled = true;
				textboxHexColor.Hide ();
				textboxHexColor.Show ();
				Focus();
			}
		}

		private void screenColorPicker1_Selecting (object sender, CancelEventArgs e)
		{
			screenColorPicker1_Selected(sender, EventArgs.Empty);
		}

		private void screenColorPicker1_Selected (object sender, EventArgs e)
		{
			colorRgb = screenColorPicker1.Color;
			labelCurrentColor.BackColor = colorRgb;
			TabColorBoxOnEnter (this, EventArgs.Empty);
			UpdateColorFields ();
		}
	}
}