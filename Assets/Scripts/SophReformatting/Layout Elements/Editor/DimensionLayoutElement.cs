using ClinicalTools.EditorElements;
using System;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    public class DimensionLayoutElement : IEditorElement
    {
        public VisualElement Element { get; } = new VisualElement();
        public event Action Updated;
        public NullableFloatElement MinElement { get; }
        public NullableFloatElement PreferredElement { get; }
        public NullableFloatElement MaxElement { get; }

        public DimensionLayoutElement(string dimensionName, DimensionLayout dimensionLayout)
        {
            MinElement = new NullableFloatElement($"Min {dimensionName}", dimensionLayout.Min);
            PreferredElement = new NullableFloatElement($"Preferred {dimensionName}", dimensionLayout.Preferred);
            MaxElement = new NullableFloatElement($"Max {dimensionName}", dimensionLayout.Max);

            MinElement.Updated += () => MinChanged(dimensionLayout, MinElement);
            PreferredElement.Updated += () => PreferredChanged(dimensionLayout, PreferredElement);
            MaxElement.Updated += () => MaxChanged(dimensionLayout, MaxElement);

            Element.Add(MinElement.Element);
            Element.Add(PreferredElement.Element);
            Element.Add(MaxElement.Element);
        }

        bool handlingValueChanged = false;
        private void MinChanged(DimensionLayout dimensionLayout, IEditorValueElement<float?> minElement)
        {
            // prevents infinite loop
            if (handlingValueChanged)
                return;
            handlingValueChanged = true;
            
            dimensionLayout.Min = minElement.Value;
            minElement.Value = dimensionLayout.Min;
            Updated?.Invoke();

            handlingValueChanged = false;
        }

        private void PreferredChanged(DimensionLayout dimensionLayout, IEditorValueElement<float?> preferredElement)
        {
            if (handlingValueChanged)
                return;
            handlingValueChanged = true;

            dimensionLayout.Preferred = preferredElement.Value;
            preferredElement.Value = dimensionLayout.Preferred;
            Updated?.Invoke();

            handlingValueChanged = false;
        }

        private void MaxChanged(DimensionLayout dimensionLayout, IEditorValueElement<float?> maxElement)
        {
            if (handlingValueChanged)
                return;
            handlingValueChanged = true;

            dimensionLayout.Max = maxElement.Value;
            maxElement.Value = dimensionLayout.Max;
            Updated?.Invoke();

            handlingValueChanged = false;
        }
    }
}