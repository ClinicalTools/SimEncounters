namespace ClinicalTools.Layout
{
    public class CenterAlignedPrimaryPositioner : IPrimaryDimensionPositioner
    {
        public PositionedDimension[] PositionDimensions(float[] dimensionSizes, SpacedPadding padding, float size)
        {
            size -= padding.Start + padding.End;
            var actualSize = GetActualSize(dimensionSizes, padding.Spacing);
            var sizeDifference = (size - actualSize) / 2;
            var start = padding.Start + sizeDifference;

            var positionedDimensions = new PositionedDimension[dimensionSizes.Length];
            for (int i = 0; i < dimensionSizes.Length; i++) {
                var dimensionSize = dimensionSizes[i];
                positionedDimensions[i] = new PositionedDimension(start, dimensionSize);

                start += dimensionSize + padding.Spacing;
            }

            return positionedDimensions;
        }

        private float GetActualSize(float[] dimensionSizes, float spacing)
        {
            var size = 0f;
            foreach (var dimensionSize in dimensionSizes)
                size += dimensionSize + spacing;
            if (dimensionSizes.Length != 0)
                size -= spacing;

            return size;
        }
    }
}