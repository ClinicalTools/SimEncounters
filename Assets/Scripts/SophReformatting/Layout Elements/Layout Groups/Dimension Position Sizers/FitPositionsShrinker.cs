namespace ClinicalTools.Layout
{
    public class FitPositionsShrinker : IPositionsShrinker
    {
        private readonly ScaleFunctions scaleFunctions = new ScaleFunctions();

        public float[] SizeElementPositions(IDimensionLayout[] dimensions, float size)
        {
            var sizedDimensions = scaleFunctions.GetSizedDimensions(dimensions);
            MinimizeNoPreferenceChildren(sizedDimensions); 
            var preferredScale = scaleFunctions.SizePositionsOutsidePreferredScale(sizedDimensions, size);
            scaleFunctions.SizePreferencePositions(sizedDimensions, preferredScale);

            return scaleFunctions.GetSizes(sizedDimensions);
        }

        private void MinimizeNoPreferenceChildren(SizedDimension[] sizedDimensions)
        {
            foreach (var sizedDimension in sizedDimensions) {
                if (sizedDimension.Dimension.Preferred != null)
                    continue;

                if (sizedDimension.Dimension.Min != null)
                    sizedDimension.Size = sizedDimension.Dimension.Min;
                else
                    sizedDimension.Size = 0;
            }
        }
    }
}