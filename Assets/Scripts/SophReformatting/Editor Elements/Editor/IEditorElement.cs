using System;
using UnityEngine.UIElements;

namespace ClinicalTools.EditorElements
{
    public interface IEditorElement
    {
        VisualElement Element { get; }
        event Action Updated;
    }
}