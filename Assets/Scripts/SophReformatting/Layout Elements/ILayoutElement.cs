using System;
using UnityEngine;

namespace ClinicalTools.Layout
{
    public interface ILayoutElement
    {
        event Action ValueChanged;

        RectTransform RectTransform { get; }
        IDimensionLayout Height { get; }
        IDimensionLayout Width { get; }

        void UpdateSize(float width, float height);
    }
}