using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class PaddingByPercent : UIBehaviour
    {
        [SerializeField] private float left;
        [SerializeField] private float right;
        [SerializeField] private float top;
        [SerializeField] private float bottom;
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
            var rect = ((RectTransform)transform.parent).rect;
            Group.padding.left = Mathf.RoundToInt(left * rect.width);
            Group.padding.right = Mathf.RoundToInt(right * rect.width);
            Group.padding.top = Mathf.RoundToInt(top * rect.height);
            Group.padding.bottom = Mathf.RoundToInt(bottom * rect.height);
            if (Group is VerticalLayoutGroup verticalLayoutGroup)
                verticalLayoutGroup.spacing = spacing * rect.height;
            if (Group is HorizontalLayoutGroup horizontalLayoutGroup)
                horizontalLayoutGroup.spacing = spacing * rect.width;
            if (Group is GridLayoutGroup gridLayoutGroup)
                gridLayoutGroup.spacing = new Vector2(spacing * rect.width, spacing * rect.height);
        }
    }
}
