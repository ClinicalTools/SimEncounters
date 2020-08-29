using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class WidthAsProportionToHeight : UIBehaviour
    {
        public float WidthPerHeight { get => widthPerHeight; set => widthPerHeight = value; }
        [SerializeField] private float widthPerHeight;

        protected LayoutElement LayoutElement { get; set; }

        protected override void Awake()
        {
            base.Awake();

            LayoutElement = GetComponent<LayoutElement>();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var rectTransform = (RectTransform)transform;

            var width = WidthPerHeight * rectTransform.rect.height;
            if (LayoutElement != null && !LayoutElement.ignoreLayout)
                LayoutElement.preferredWidth = width;
            else
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
    }
}