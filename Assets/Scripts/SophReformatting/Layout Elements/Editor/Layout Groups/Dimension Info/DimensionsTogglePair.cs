using ClinicalTools.EditorElements;
using System;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    public class DimensionsTogglePair : IEditorElement
    {
        public VisualElement Element { get; set; }
        public event Action Updated;
        public bool WidthValue { get; set; }
        public event Action<bool> WidthValueChanged;
        public bool HeightValue { get; set; }
        public event Action<bool> HeightValueChanged;

        private const float dimensionLabelWidth = 100;

        private readonly CreateEditorUIElements createEditorUIElements = new CreateEditorUIElements();

        public DimensionsTogglePair(string name, bool widthValue, bool heightValue)
        {
            Element = createEditorUIElements.CreateRow();
            var label = createEditorUIElements.CreateFieldLabel(name);
            var widthToggle = createEditorUIElements.CreateToggleField(widthValue);
            var widthLabel = createEditorUIElements.CreateLabel("Width", dimensionLabelWidth);
            var heightToggle = createEditorUIElements.CreateToggleField(heightValue);
            var heightLabel = createEditorUIElements.CreateLabel("Height", dimensionLabelWidth);

            widthToggle.RegisterValueChangedCallback((changeEvent) => SetWidthValue(changeEvent.newValue));
            heightToggle.RegisterValueChangedCallback((changeEvent) => SetHeightValue(changeEvent.newValue));

            Element.Add(label);
            Element.Add(widthToggle);
            Element.Add(widthLabel);
            Element.Add(heightToggle);
            Element.Add(heightLabel);
        }

        private void SetWidthValue(bool value)
        {
            WidthValue = value;
            WidthValueChanged?.Invoke(value);
            Updated?.Invoke();
        }
        private void SetHeightValue(bool value)
        {
            HeightValue = value;
            HeightValueChanged?.Invoke(value);
            Updated?.Invoke();
        }
    }
}