using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class WidthAsProportionToHeight : UIBehaviour
    {
        public float WidthPerHeight { get => widthPerHeight; set => widthPerHeight = value; }
        [SerializeField] private float widthPerHeight = 1;

        // I would assign LayoutElement in Awake, but Awake isn't always guaranteed in Editor mode
        private bool checkedForLayoutElement;
        private LayoutElement layoutElement;
        protected virtual LayoutElement LayoutElement {
            get {
                if (!checkedForLayoutElement) {
                    checkedForLayoutElement = true;
                    layoutElement = GetComponent<LayoutElement>();
                }

                return layoutElement;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            UpdateWidth();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var currentHeight = ((RectTransform)transform).rect.height;
            if (Mathf.Abs(currentHeight - height) < Tolerance)
                return;

            height = currentHeight;
            UpdateWidth();
        }

        private float lastFontSizePerHeight;
        protected virtual void Update()
        {
            if (lastFontSizePerHeight != WidthPerHeight)
                UpdateWidth();
        }

        private float height;
        private const float Tolerance = .0001f;
        private void UpdateWidth()
        {
            if (height < Tolerance)
                height = ((RectTransform)transform).rect.height;

            lastFontSizePerHeight = WidthPerHeight;

            var width = WidthPerHeight * height;
            if (LayoutElement != null && !LayoutElement.ignoreLayout)
                LayoutElement.preferredWidth = width;
            else
                ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
    }
}