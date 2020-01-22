using UnityEngine;

namespace ClinicalTools.Layout
{
    [ExecuteAlways]
    public class HorizontalGroup : LayoutGroup
    {
        [field: SerializeField] public virtual SpacedPadding HorizontalPadding { get; set; } = new SpacedPadding();
        [field: SerializeField] public virtual Padding VerticalPadding { get; set; } = new Padding();

        protected IPositionsSizer PositionsSizer => GetPositionsSizer(GroupWidth);
        protected IPrimaryDimensionPositioner HorizontalPositioner => GetPrimaryPositioner();
        protected ISecondaryDimensionPositioner VerticalPositioner => new SecondaryPositioner();

        protected override void Initialize()
        {
            HorizontalPadding.ValueChanged += InvokeValueChanged;
            VerticalPadding.ValueChanged += InvokeValueChanged;

            base.Initialize();
        }

        public IPrimaryDimensionPositioner GetPrimaryPositioner()
        {
            if (IsLeft(ChildAnchor))
                return new StartAlignedPrimaryPositioner();
            else if (IsRight(ChildAnchor))
                return new EndAlignedPrimaryPositioner();
            else
                return new CenterAlignedPrimaryPositioner();
        }

        public override void UpdateSize(float width, float height)
        {
            SortElements();

            var elemSize = GetSizeForElements(width, HorizontalPadding, ChildElements.Count);

            var widths = PositionsSizer.SizeElementPositions(ChildWidths, elemSize);
            var positionedXs = HorizontalPositioner.PositionDimensions(widths, HorizontalPadding, width);

            var positionedY = VerticalPositioner.PositionDimension(VerticalPadding, height);
            for (int i = 0; i < ChildElements.Count; i++)
                PlaceElement(ChildElements[i], positionedXs[i], positionedY);
        }

        protected override IDimensionLayout GetWidthDimensionLayout()
        {
            if (GroupWidth.FitChild)
                return new FitterPrimaryDimension(GroupWidth.DimensionLayout, HorizontalPadding, ChildWidths);
            else
                return GroupWidth.DimensionLayout;
        }

        protected override IDimensionLayout GetHeightDimensionLayout()
        {
            if (GroupHeight.FitChild)
                return new FitterSecondaryDimension(GroupHeight.DimensionLayout, VerticalPadding, ChildHeights);
            else
                return GroupHeight.DimensionLayout;
        }
    }
}