using System;
using System.Drawing;

namespace MechanikaDesign.WinForms.UI.ColorPicker
{
    public class ColorChangedEventArgs : EventArgs
    {
        private Color selectedColor;
        private HslColor selectedHslColor;

        public ColorChangedEventArgs(Color selectedColor)
        {
            this.selectedColor = selectedColor;
            selectedHslColor = HslColor.FromColor(selectedColor);
        }

        public ColorChangedEventArgs(HslColor selectedHslColor)
        {
            selectedColor = selectedHslColor.RgbValue;
            this.selectedHslColor = selectedHslColor;
        }

        public Color SelectedColor
        {
            get
            {
                return selectedColor;
            }
        }

        public HslColor SelectedHslColor
        {
            get
            {
                return selectedHslColor;
            }
        }
    }
}
