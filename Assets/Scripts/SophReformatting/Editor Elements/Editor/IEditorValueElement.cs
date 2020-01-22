using System;

namespace ClinicalTools.EditorElements
{
    public interface IEditorValueElement<T> : IEditorElement
    {
        T Value { get; set; }
        event Action<T> ValueChanged;
    }
}