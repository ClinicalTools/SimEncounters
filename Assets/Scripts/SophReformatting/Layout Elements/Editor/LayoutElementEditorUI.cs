using ClinicalTools.EditorElements;
using System;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    public class LayoutElementEditorUI : IEditorElement
    {
        public VisualElement Element { get; } = new VisualElement();
        public event Action Updated;

        public LayoutElementEditorUI(DimensionLayout width, DimensionLayout height)
        {
            var widthElement = new DimensionLayoutElement("Width", width);
            widthElement.Updated += Updated;
            var heightElement = new DimensionLayoutElement("Height", height);
            heightElement.Updated += Updated;

            Element.Add(widthElement.MinElement.Element);
            Element.Add(heightElement.MinElement.Element);
            Element.Add(widthElement.PreferredElement.Element);
            Element.Add(heightElement.PreferredElement.Element);
            Element.Add(widthElement.MaxElement.Element);
            Element.Add(heightElement.MaxElement.Element);
        }
    }
}