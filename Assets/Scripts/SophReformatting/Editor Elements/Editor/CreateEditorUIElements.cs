using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ClinicalTools.EditorElements
{
    public class CreateEditorUIElements
    {
        public VisualElement CreateRow()
        {
            return new VisualElement() {
                style = {
                    flexDirection = FlexDirection.Row
                }
            };
        }
        public Foldout CreateFoldout(string text)
        {
            return new Foldout() {
                text = text
            };
        }

        public Label CreateFieldLabel(string text) => CreateLabel(text, EditorGUIUtility.labelWidth);
        public Label CreateLabel(string text, float width)
        {
            return new Label() {
                text = text,
                style = {
                    width = width
                }
            };
        }

        public Toggle CreateToggleField(bool value)
        {
            var toggle = new Toggle() {
                value = value,
                style = {
                    width = 15
                }
            };

            return toggle;
        }

        public EnumField CreateEnumField(string label, Enum value)
        {
            var enumField = new EnumField(label, value);
            

            return enumField;
        }

        public FloatField CreateFloatField(string label, float value)
        {
            var floatField = new FloatField() {
                value = value,
                label = label,
                style = {
                    flexGrow = 1,
                }
            };

            return floatField;
        }
        public FloatField CreateFloatField(float value)
        {
            var floatField = new FloatField() {
                value = value,
                label = " ",
                style = {
                    flexGrow = 1,
                }
            };
            floatField.labelElement.style.minWidth = 5;

            return floatField;
        }
    }
}