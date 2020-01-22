namespace ClinicalTools.Layout
{
    public class EndAlignedPrimaryPositioner : IPrimaryDimensionPositioner
    {
        public PositionedDimension[] PositionDimensions(float[] dimensionSizes, SpacedPadding padding, float size)
        {
            var positionedDimensions = new PositionedDimension[dimensionSizes.Length];
            var start = size - padding.End;

            for (int i = dimensionSizes.Length - 1; i >= 0; i--) {
                var dimensionSize = dimensionSizes[i];
                start -= dimensionSize;

                positionedDimensions[i] = new PositionedDimension(start, dimensionSize);

                start -= padding.Spacing;
            }

            return positionedDimensions;
        }
    }
}