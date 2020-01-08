using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ColorSlider : IChangableValue<float>
    {
        private readonly ValueSliderUI valueSliderUI;

        private float value;
        public float Value {
            get {
                return value;
            }
            set {
                if (this.value == value)
                    return;

                this.value = value;

                UpdateLabel();
                UpdateSlider();

                ValueChanged?.Invoke(this, Value);
            }
        }
        public event ValueChangedEventHandler<float> ValueChanged;


        public ColorSlider(ValueSliderUI valueSliderUI, float value)
        {
            this.valueSliderUI = valueSliderUI;
            valueSliderUI.Slider.onValueChanged.AddListener(SliderChanged);
            Value = value;
        }

        protected void SliderChanged(float value)
        {
            if (value == Value)
                return;
            this.value = value;

            UpdateLabel();
        }

        private const int VALUE_LABEL_MAX = 255;
        protected void UpdateLabel()
        {
            valueSliderUI.ValueLabel.text = Mathf.RoundToInt(Value * VALUE_LABEL_MAX).ToString();
        }

        protected void UpdateSlider()
        {
            valueSliderUI.Slider.value = Value;
        }
    }
}