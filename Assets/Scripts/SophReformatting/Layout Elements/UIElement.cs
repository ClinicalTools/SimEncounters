using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.Layout
{
    public class UIElement : UIBehaviour
    {
        public RectTransform RectTransform => (RectTransform)transform;
        public event Action<Transform> RectTransformDimensionsChange;

        protected override void OnRectTransformDimensionsChange()
        {
            RectTransformDimensionsChange?.Invoke(transform);

            base.OnRectTransformDimensionsChange();
        }
    }
}