using ClinicalTools.EditorElements;
using System;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    public class SpacingEditorUI : IEditorElement
    {
        public VisualElement Element { get; }
        public event Action Updated;

        private readonly CreateEditorUIElements createEditorUIElements = new CreateEditorUIElements();
        
        public SpacingEditorUI(SpacedPadding spacedPadding)
        {
            var spacingField = createEditorUIElements.CreateFloatField("Spacing", spacedPadding.Spacing);
            spacingField.RegisterValueChangedCallback((changeEvent) => SpacingChanged(spacedPadding, changeEvent.newValue));
            Element = spacingField;
        }

        private void SpacingChanged(SpacedPadding spacedPadding, float value)
        {
            spacedPadding.Spacing = value;
            Updated?.Invoke();
        }
    }
}