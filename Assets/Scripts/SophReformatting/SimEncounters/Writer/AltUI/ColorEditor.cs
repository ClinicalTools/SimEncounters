using UnityEngine;
using UnityEngine.UI;
using ClinicalTools.SimEncounters.Extensions;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ColorEditor
    {
        // the image is stored rather than the color, to ensure the custom color is always retrieved correctly
        protected Image SelectedImage { get; set; }
        public Color Value => SelectedImage.color;

        protected IChangableValue<Color> Sliders { get; }
        protected IChangableValue<Color> HexField { get; }

        protected Toggle CustomColorToggle { get; }
        protected Image CustomColorImage { get; }

        public ColorEditor(ColorUI colorUI)
        {
            CustomColorToggle = colorUI.CustomColorToggle;
            CustomColorImage = CustomColorToggle.GetComponent<Image>();

            var color = CustomColorImage.color;

            Sliders = new ColorSlidersGroup(colorUI.Sliders, color);
            HexField = new HexColorField(colorUI.HexColorField, color);

            AddListeners(colorUI);
        }

        protected virtual void AddListeners(ColorUI colorUI)
        {
            Sliders.ValueChanged += SlidersValueChanged;
            HexField.ValueChanged += HexFieldValueChanged;

            foreach (var toggle in colorUI.ColorToggles) {
                toggle.AddOnSelectListener(() => ColorToggleSelected(toggle));
            }
        }

        private void ColorToggleSelected(Toggle toggle)
        {
            var toggleImage = toggle.GetComponent<Image>();
            SelectedImage = toggleImage;
        }

        private void SlidersValueChanged(object sender, Color color)
        {
            UpdateCustomColor(color);
            Sliders.Value = color;
        }
        private void HexFieldValueChanged(object sender, Color color)
        {
            UpdateCustomColor(color);
            HexField.Value = color;
        }

        private void UpdateCustomColor(Color color)
        {
            CustomColorImage.color = color;
            CustomColorToggle.isOn = true;
        }
    }
}