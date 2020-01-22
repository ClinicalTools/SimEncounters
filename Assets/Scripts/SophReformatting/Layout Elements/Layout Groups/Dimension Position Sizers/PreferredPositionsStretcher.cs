namespace ClinicalTools.Layout
{
    public class PreferredPositionsStretcher : IPositionsStretcher
    {
        private readonly SizeFunctions sizeFunctions = new SizeFunctions();

        public float[] SizeElementPositions(IDimensionLayout[] dimensions, float size)
        {
            var sizedDimensions = sizeFunctions.GetSizedDimensions(dimensions);

            SetChildrenToPreferredSize(sizedDimensions);
            var preferredSize = sizeFunctions.SizePositionsOutsideAverageSize(sizedDimensions, size);
            sizeFunctions.SizeRemainingPositions(sizedDimensions, preferredSize);

            return sizeFunctions.GetSizes(sizedDimensions);
        }

        private void SetChildrenToPreferredSize(SizedDimension[] sizedDimensions)
        {
            foreach (var sizedDimension in sizedDimensions)
                sizedDimension.Size = sizedDimension.Dimension.Preferred;
        }
    }
}