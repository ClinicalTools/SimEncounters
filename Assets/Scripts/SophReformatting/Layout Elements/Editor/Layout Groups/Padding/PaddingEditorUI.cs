using ClinicalTools.EditorElements;
using System;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    public class PaddingEditorUI : IEditorElement
    {
        public VisualElement Element { get; }
        public event Action Updated;

        private readonly CreateEditorUIElements createEditorUIElements = new CreateEditorUIElements();
        
        public PaddingEditorUI(Padding horizontalPadding, Padding verticalPadding)
        {
            Element = createEditorUIElements.CreateFoldout("Padding");
            
            var leftFloatField = createEditorUIElements.CreateFloatField("Left", horizontalPadding.Start);
            leftFloatField.RegisterValueChangedCallback((changeEvent) => StartChanged(horizontalPadding, changeEvent.newValue));
            var rightFloatField = createEditorUIElements.CreateFloatField("Right", horizontalPadding.End);
            rightFloatField.RegisterValueChangedCallback((changeEvent) => EndChanged(horizontalPadding, changeEvent.newValue));
            var upFloatField = createEditorUIElements.CreateFloatField("Up", verticalPadding.Start);
            upFloatField.RegisterValueChangedCallback((changeEvent) => StartChanged(verticalPadding, changeEvent.newValue));
            var downFloatField = createEditorUIElements.CreateFloatField("Down", verticalPadding.End);
            downFloatField.RegisterValueChangedCallback((changeEvent) => EndChanged(verticalPadding, changeEvent.newValue));

            Element.Add(leftFloatField);
            Element.Add(rightFloatField);
            Element.Add(upFloatField);
            Element.Add(downFloatField);
        }


        private void StartChanged(Padding padding, float value)
        {
            padding.Start = value;
            Updated?.Invoke();
        }
        private void EndChanged(Padding padding, float value)
        {
            padding.End = value;
            Updated?.Invoke();
        }
    }
}