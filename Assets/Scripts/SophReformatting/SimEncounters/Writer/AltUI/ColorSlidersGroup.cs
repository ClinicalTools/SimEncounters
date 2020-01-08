using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ColorSlidersGroup : IChangableValue<Color>
    {
        private Color value;
        public Color Value {
            get {
                return value;
            }
            set {
                this.value = value;

                ValueChanged?.Invoke(this, Value);
            }
        }

        public event ValueChangedEventHandler<Color> ValueChanged;

        private readonly ColorSlider redSlider, greenSlider, blueSlider;

        public ColorSlidersGroup(ColorSlidersUI colorSlidersUI, Color value)
        {
            redSlider = new ColorSlider(colorSlidersUI.Red, value.r);
            redSlider.ValueChanged += RedChanged;
            greenSlider = new ColorSlider(colorSlidersUI.Green, value.g);
            greenSlider.ValueChanged += GreenChanged;
            blueSlider = new ColorSlider(colorSlidersUI.Blue, value.b);
            blueSlider.ValueChanged += BlueChanged;
        }

        protected virtual void UpdateSliders()
        {
            redSlider.Value = Value.r;
            greenSlider.Value = Value.g;
            blueSlider.Value = Value.b;
        }
        protected virtual void RedChanged(object sender, float value)
        {
            if (value == Value.r)
                return;

            var color = Value;
            color.r = value;
            Value = color;
        }
        protected virtual void GreenChanged(object sender, float value)
        {
            if (value == Value.g)
                return;

            var color = Value;
            color.g = value;
            Value = color;
        }
        protected virtual void BlueChanged(object sender, float value)
        {
            if (value == Value.b)
                return;

            var color = Value;
            color.b = value;
            Value = color;
        }
    }
}