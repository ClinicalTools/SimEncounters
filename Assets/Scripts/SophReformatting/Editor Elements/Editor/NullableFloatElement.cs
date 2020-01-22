using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ClinicalTools.EditorElements
{
    public class NullableFloatElement : IEditorValueElement<float?>
    {
        public VisualElement Element { get; }

        private float? value;
        public float? Value {
            get { return value; }
            set {
                if (this.value == value)
                    return;

                if (this.value == null ^ value == null)
                    toggle.value = value == null;
                if (value != null)
                    floatField.value = (float)value;

                this.value = value;
            }
        }
        private readonly Toggle toggle;
        private readonly FloatField floatField;

        public event Action<float?> ValueChanged;
        public event Action Updated;


        private readonly CreateEditorUIElements createEditorUIElements = new CreateEditorUIElements();

        public NullableFloatElement(string name, float? value, float minValue = float.MinValue)
        {
            var notNull = value != null;
            var floatValue = (notNull) ? (float)value : 0;

            Element = createEditorUIElements.CreateRow();
            var label = createEditorUIElements.CreateFieldLabel(name);
            toggle = createEditorUIElements.CreateToggleField(notNull);
            floatField = createEditorUIElements.CreateFloatField(floatValue);

            toggle.RegisterValueChangedCallback((changeEvent) => BoolValueChanged(changeEvent, Element, floatField));
            floatField.RegisterValueChangedCallback((changeEvent) => FloatValueChanged(changeEvent, floatField, minValue));

            Element.Add(label);
            Element.Add(toggle);
            if (notNull)
                Element.Add(floatField);
        }

        private void FloatValueChanged(ChangeEvent<float> changeEvent, FloatField floatField, float minValue)
        {
            if (changeEvent.newValue < minValue)
                floatField.value = minValue;
            SetValue(floatField.value);
        }

        private void BoolValueChanged(ChangeEvent<bool> changeEvent, VisualElement parent, FloatField floatField)
        {
            if (changeEvent.newValue) {
                SetValue(floatField.value);
                parent.Add(floatField);
            } else {
                SetValue(null);
                parent.Remove(floatField);
            }
        }

        private void SetValue(float? value)
        {
            this.value = value;
            ValueChanged?.Invoke(value);
            Updated?.Invoke();
        }
    }
}