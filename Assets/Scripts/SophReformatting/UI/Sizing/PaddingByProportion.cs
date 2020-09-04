using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
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
        protected LayoutGroup Group {
            get {
                if (group == null)
                    group = GetComponent<LayoutGroup>();
                return group;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            UpdateAll();
        }

        private Rect currentRect;
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            UpdateAll();
        }

        protected virtual void UpdateAll()
        {
            currentRect = ((RectTransform)transform).rect;
            UpdateLeft();
            UpdateRight();
            UpdateTop();
            UpdateBottom();
            UpdateSpacing();
        }

        protected virtual void Update()
        {
            if (currentRect == default)
                currentRect = ((RectTransform)transform).rect;

            if (lastLeft != Left)
                UpdateLeft();
            if (lastRight != Right)
                UpdateRight();
            if (lastTop != Top)
                UpdateTop();
            if (lastBottom != Bottom)
                UpdateBottom();
            if (lastSpacing != Spacing)
                UpdateSpacing();
        }


        private float lastLeft;
        protected void UpdateLeft()
        {
            lastLeft = Left;
            Group.padding.left = Mathf.RoundToInt(Left * currentRect.width);
        }

        private float lastRight;
        protected void UpdateRight()
        {
            lastRight = Right;
            Group.padding.right = Mathf.RoundToInt(Right * currentRect.width);
        }

        private float lastTop;
        protected void UpdateTop()
        {
            lastTop = Top;
            Group.padding.top = Mathf.RoundToInt(Top * currentRect.height);
        }

        private float lastBottom;
        protected void UpdateBottom()
        {
            lastBottom = Bottom;
            Group.padding.bottom = Mathf.RoundToInt(Bottom * currentRect.height);
        }

        private float lastSpacing;
        protected void UpdateSpacing()
        {
            lastSpacing = Spacing;
            if (Group is VerticalLayoutGroup verticalLayoutGroup)
                verticalLayoutGroup.spacing = GetHeightSpacing(currentRect);
            if (Group is HorizontalLayoutGroup horizontalLayoutGroup)
                horizontalLayoutGroup.spacing = GetWidthSpacing(currentRect);
            if (Group is GridLayoutGroup gridLayoutGroup)
                gridLayoutGroup.spacing = new Vector2(GetWidthSpacing(currentRect), GetHeightSpacing(currentRect));

        }

        protected virtual float GetWidthSpacing(Rect rect) => Spacing * rect.width;
        protected virtual float GetHeightSpacing(Rect rect) => Spacing * rect.height;
    }
}
