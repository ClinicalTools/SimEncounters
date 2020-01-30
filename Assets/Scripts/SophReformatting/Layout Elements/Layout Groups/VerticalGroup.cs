using UnityEngine;

namespace ClinicalTools.Layout
{
    [ExecuteAlways]
    public class VerticalGroup : LayoutGroup
    {
        [field: SerializeField] public virtual Padding HorizontalPadding { get; set; } = new Padding();
        [field: SerializeField] public virtual SpacedPadding VerticalPadding { get; set; } = new SpacedPadding();

        protected IPositionsSizer PositionsSizer => GetPositionsSizer(GroupHeight);
        protected ISecondaryDimensionPositioner HorizontalPositioner => new SecondaryPositioner();
        protected IPrimaryDimensionPositioner VerticalPositioner => GetPrimaryPositioner();

        protected override void Initialize()
        {
            HorizontalPadding.ValueChanged += InvokeValueChanged;
            VerticalPadding.ValueChanged += InvokeValueChanged;

            base.Initialize();
        }

        protected virtual IPrimaryDimensionPositioner GetPrimaryPositioner()
        {
            if (IsUpper(ChildAnchor))
                return new StartAlignedPrimaryPositioner();
            else if (IsLower(ChildAnchor))
                return new EndAlignedPrimaryPositioner();
            else
                return new CenterAlignedPrimaryPositioner();
        }

        public override void UpdateSize(float width, float height)
        {
            SortElements();

            var elemSize = GetSizeForElements(height, VerticalPadding, ChildElements.Count);
            
            var heights = PositionsSizer.SizeElementPositions(ChildHeights, elemSize);
            var positionedYs = VerticalPositioner.PositionDimensions(heights, VerticalPadding, height);

            var positionedX = HorizontalPositioner.PositionDimension(HorizontalPadding, width);
            for (int i = 0; i < ChildElements.Count; i++) {
                PlaceElement(ChildElements[i], positionedX, positionedYs[i]);
            }
        }

        protected override IDimensionLayout GetWidthDimensionLayout()
        {
            if (GroupWidth.FitChild)
                return new FitterSecondaryDimension(GroupWidth.DimensionLayout, HorizontalPadding, ChildWidths);
            else
                return GroupWidth.DimensionLayout;
        }

        protected override IDimensionLayout GetHeightDimensionLayout()
        {
            if (GroupHeight.FitChild)
                return new FitterPrimaryDimension(GroupHeight.DimensionLayout, VerticalPadding, ChildHeights);
            else
                return GroupHeight.DimensionLayout;
        }
    }
}