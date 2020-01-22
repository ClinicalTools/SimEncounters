namespace ClinicalTools.Layout
{
    public class StartAlignedPrimaryPositioner : IPrimaryDimensionPositioner
    {
        public PositionedDimension[] PositionDimensions(float[] dimensionSizes, SpacedPadding padding, float size)
        {
            var positionedDimensions = new PositionedDimension[dimensionSizes.Length];
            var start = padding.Start;

            for (int i = 0; i < dimensionSizes.Length; i++) {
                var dimensionSize = dimensionSizes[i];
                positionedDimensions[i] = new PositionedDimension(start, dimensionSize);

                start += dimensionSize + padding.Spacing;
            }

            return positionedDimensions;
        }
    }
}