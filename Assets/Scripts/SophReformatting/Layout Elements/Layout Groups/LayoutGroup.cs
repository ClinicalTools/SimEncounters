using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ClinicalTools.Layout
{
    [ExecuteAlways]
    public abstract class LayoutGroup : UIElement, ILayoutElement
    {
        public event Action ValueChanged;
        protected void InvokeValueChanged() => ValueChanged?.Invoke();

        protected DrivenRectTransformTracker ChildController { get; }

        protected bool Dirty { get; set; } = true;

        [field: SerializeField] public virtual LayoutGroupDimension GroupWidth { get; set; } = new LayoutGroupDimension();
        [field: SerializeField] public virtual LayoutGroupDimension GroupHeight { get; set; } = new LayoutGroupDimension();

        private IDimensionLayout width;
        public IDimensionLayout Width {
            get {
                if (width == null)
                    UpdateWidth();
                return width;
            }
        }
        protected void UpdateWidth() => width = GetWidthDimensionLayout();

        private IDimensionLayout height;
        public IDimensionLayout Height {
            get {
                if (height == null)
                    UpdateHeight();
                return height;
            }
        }
        protected void UpdateHeight() => height = GetHeightDimensionLayout();

        [SerializeField] private TextAnchor childAnchor;
        public virtual TextAnchor ChildAnchor {
            get { return childAnchor; }
            set {
                childAnchor = value;
                ValueChanged?.Invoke();
            }
        }

        private LayoutGroup parent;

        protected IElementSizer ElementWidthSizer => GetElementSizer(GroupWidth);
        protected IElementSizer ElementHeightSizer => GetElementSizer(GroupHeight);
        protected IElementPositioner ElementHorizontalPositioner => GetHorizontalElementPositioner(ChildAnchor);
        protected IElementPositioner ElementVerticalPositioner => GetVerticalElementPositioner(ChildAnchor);

        protected List<ILayoutElement> ChildElements { get; } = new List<ILayoutElement>();
        protected IDimensionLayout[] ChildWidths { get; set; } = new IDimensionLayout[0];
        protected IDimensionLayout[] ChildHeights { get; set; } = new IDimensionLayout[0];

        private readonly Vector2 topLeft = new Vector2(0, 1);
        public void AddChild(ILayoutElement layoutElement)
        {
            ChildElements.Add(layoutElement);
            var childRect = layoutElement.RectTransform;
            ChildController.Add(this, childRect, DrivenTransformProperties.All);
            childRect.anchorMin = topLeft;
            childRect.anchorMax = topLeft;
            childRect.pivot = topLeft;
            childRect.rotation = new Quaternion();
            childRect.localScale = new Vector3(1, 1, 1);

            layoutElement.ValueChanged += SortElements;
            layoutElement.ValueChanged += () => ValueChanged?.Invoke();

            SortElements();
            ValueChanged?.Invoke();
        }

        public void RemoveChild(ILayoutElement layoutElement)
        {
            ChildElements.Remove(layoutElement);
            ValueChanged?.Invoke();
            SortElements();
        }

        protected override void Awake()
        {
            LateUpdate();

            base.Awake();
        }
        protected override void Start()
        {
            Initialize();

            base.Start();
        }

        private bool initialized;
        private int siblingIndex;
        protected virtual void Update()
        {
            // Awake/start aren't always ran in editor mode (particularly upon code recompiling), so this ensures values are still initialized
            if (!initialized)
                Initialize();
            var newSiblingIndex = transform.GetSiblingIndex();
            if (newSiblingIndex != siblingIndex) {
                siblingIndex = newSiblingIndex;
                ValueChanged?.Invoke();
            }
        }

        protected override void OnDestroy()
        {
            if (parent != null)
                parent.RemoveChild(this);
        }

        protected virtual void SortElements()
        {
            ChildElements.Sort(
                (ILayoutElement e1, ILayoutElement e2) => e1.RectTransform.GetSiblingIndex().CompareTo(e2.RectTransform.GetSiblingIndex())
            );

            ChildWidths = new IDimensionLayout[ChildElements.Count];
            ChildHeights = new IDimensionLayout[ChildElements.Count];
            for (var i = 0; i < ChildElements.Count; i++) {
                ChildWidths[i] = ChildElements[i].Width;
                ChildHeights[i] = ChildElements[i].Height;
            }
            UpdateWidth();
            UpdateHeight();
        }

        protected virtual void Initialize()
        {
            parent = transform.parent.GetComponent<LayoutGroup>();
            if (parent != null) {
                parent.AddChild(this);
            }

            RectTransform.pivot = topLeft;

            UpdateWidth();
            UpdateHeight();

            GroupWidth.ValueChanged += InvokeValueChanged;
            GroupWidth.ValueChanged += UpdateWidth;

            GroupHeight.ValueChanged += InvokeValueChanged;
            GroupHeight.ValueChanged += UpdateHeight;
            ValueChanged += () => Dirty = true;


            initialized = true;
        }

        protected abstract IDimensionLayout GetWidthDimensionLayout();
        protected abstract IDimensionLayout GetHeightDimensionLayout();

        protected virtual void LateUpdate()
        {
            if (parent == null && Dirty) {
                if (Width.Preferred != null)
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)Width.Preferred);
                if (Height.Preferred != null)
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)Height.Preferred);
                UpdateSize(RectTransform.rect.width, RectTransform.rect.height);
                Dirty = false;
            }
        }

        public abstract void UpdateSize(float width, float height);

        protected void PlaceElement(ILayoutElement layoutElement, PositionedDimension positionedX, PositionedDimension positionedY)
        {
            var elementWidth = ElementWidthSizer.SizeElement(layoutElement.Width, positionedX.Size);
            var elementHorizontalPosition = ElementHorizontalPositioner.PositionElement(positionedX, elementWidth);

            var elementHeight = ElementHeightSizer.SizeElement(layoutElement.Height, positionedY.Size);
            var elementVerticalPosition = ElementVerticalPositioner.PositionElement(positionedY, elementHeight);

            SetElementPositions(layoutElement, elementHorizontalPosition, elementVerticalPosition);
        }

        private void SetElementPositions(ILayoutElement layoutElement, PositionedDimension horizontalPosition, PositionedDimension verticalPosition)
        {
            layoutElement.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalPosition.Size);
            layoutElement.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalPosition.Size);
            layoutElement.RectTransform.localPosition = new Vector3(horizontalPosition.StartPosition, -verticalPosition.StartPosition);
            layoutElement.UpdateSize(horizontalPosition.Size, verticalPosition.Size);
        }

        protected float GetSizeForElements(float size, SpacedPadding padding, float elementCount)
        {
            size -= padding.Start + padding.End;
            if (elementCount > 1)
                size -= padding.Spacing * (elementCount - 1);
            return size;
        }

        protected bool IsLeft(TextAnchor textAnchor) => textAnchor == TextAnchor.UpperLeft || textAnchor == TextAnchor.MiddleLeft || textAnchor == TextAnchor.LowerLeft;
        protected bool IsCenter(TextAnchor textAnchor) => textAnchor == TextAnchor.UpperCenter || textAnchor == TextAnchor.MiddleCenter || textAnchor == TextAnchor.LowerCenter;
        protected bool IsRight(TextAnchor textAnchor) => textAnchor == TextAnchor.UpperRight || textAnchor == TextAnchor.MiddleRight || textAnchor == TextAnchor.LowerRight;
        protected bool IsUpper(TextAnchor textAnchor) => textAnchor == TextAnchor.UpperLeft || textAnchor == TextAnchor.UpperCenter || textAnchor == TextAnchor.UpperRight;
        protected bool IsMiddle(TextAnchor textAnchor) => textAnchor == TextAnchor.MiddleLeft || textAnchor == TextAnchor.MiddleCenter || textAnchor == TextAnchor.MiddleRight;
        protected bool IsLower(TextAnchor textAnchor) => textAnchor == TextAnchor.LowerLeft || textAnchor == TextAnchor.LowerCenter || textAnchor == TextAnchor.LowerRight;

        public IPositionsSizer GetPositionsSizer(LayoutGroupDimension dimension)
        {
            IPositionsShrinker shrinker;
            if (dimension.ControlChild)
                shrinker = new FitPositionsShrinker();
            else
                shrinker = new PreferredPositionsShrinker();

            IPositionsStretcher stretcher;
            if (dimension.ExpandChild)
                stretcher = new FitPositionsStretcher();
            else
                stretcher = new PreferredPositionsStretcher();

            return new PositionsSizer(shrinker, stretcher);
        }

        public IElementPositioner GetHorizontalElementPositioner(TextAnchor elementAnchor)
        {
            if (IsLeft(elementAnchor))
                return new AlignToStart();
            else if (IsRight(elementAnchor))
                return new AlignToEnd();
            else
                return new AlignToCenter();
        }
        public IElementPositioner GetVerticalElementPositioner(TextAnchor elementAnchor)
        {
            if (IsUpper(elementAnchor))
                return new AlignToStart();
            else if (IsLower(elementAnchor))
                return new AlignToEnd();
            else
                return new AlignToCenter();
        }

        public IElementSizer GetElementSizer(LayoutGroupDimension dimension)
        {
            if (!dimension.ControlChild)
                return new PreferredElementSizer();
            else if (dimension.ExpandChild)
                return new FitElementSizer();
            else
                return new ShrinkElementSizer();
        }
    }
}