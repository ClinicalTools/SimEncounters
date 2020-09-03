using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class PaddingByProportion : UIBehaviour
    {
        public virtual float Left { get => left; set => left = value; }
        [SerializeField] private float left;
        public virtual float Right { get => right; set => right = value; }
        [SerializeField] private float right;
        public virtual float Top { get => top; set => top = value; }
        [SerializeField] private float top;
        public virtual float Bottom { get => bottom; set => bottom = value; }
        [SerializeField] private float bottom;
        public virtual float Spacing { get => spacing; set => spacing = value; }
        [SerializeField] private float spacing;

        private LayoutGroup group;
        protected LayoutGroup Group
        {
            get {
                if (group == null)
                    group = GetComponent<LayoutGroup>();
                return group;
            }
        }


        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            var rect = ((RectTransform)transform).rect;
            Group.padding.left = Mathf.RoundToInt(left * rect.width);
            Group.padding.right = Mathf.RoundToInt(right * rect.width);
            Group.padding.top = Mathf.RoundToInt(top * rect.height);
            Group.padding.bottom = Mathf.RoundToInt(bottom * rect.height);
            if (Group is VerticalLayoutGroup verticalLayoutGroup)
                verticalLayoutGroup.spacing = GetHeightSpacing(rect);
            if (Group is HorizontalLayoutGroup horizontalLayoutGroup)
                horizontalLayoutGroup.spacing = GetWidthSpacing(rect);
            if (Group is GridLayoutGroup gridLayoutGroup)
                gridLayoutGroup.spacing = new Vector2(GetWidthSpacing(rect), GetHeightSpacing(rect));
        }

        protected virtual float GetWidthSpacing(Rect rect) => spacing * rect.width;
        protected virtual float GetHeightSpacing(Rect rect) => spacing * rect.height;
    }
}
