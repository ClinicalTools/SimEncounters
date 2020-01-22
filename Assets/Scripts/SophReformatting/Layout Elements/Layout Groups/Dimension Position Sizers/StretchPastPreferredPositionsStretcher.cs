namespace ClinicalTools.Layout
{
    public class StretchPastPreferredPositionsStretcher : IPositionsStretcher
    {
        private readonly ScaleFunctions scaleFunctions = new ScaleFunctions();

        public float[] SizeElementPositions(IDimensionLayout[] dimensions, float size)
        {
            var sizedDimensions = scaleFunctions.GetSizedDimensions(dimensions);
            
            MaximizeNoPreferencePositions(sizedDimensions);
            var preferredScale = scaleFunctions.SizePositionsOutsidePreferredScale(sizedDimensions, size);
            scaleFunctions.SizePreferencePositions(sizedDimensions, preferredScale);
            
            return scaleFunctions.GetSizes(sizedDimensions);
        }

        private void MaximizeNoPreferencePositions(SizedDimension[] sizedDimensions)
        {
            foreach (var sizedDimension in sizedDimensions) {
                if (sizedDimension.Dimension.Preferred == null)
                    sizedDimension.Size = sizedDimension.Dimension.Max;
            }
        }
    }
}