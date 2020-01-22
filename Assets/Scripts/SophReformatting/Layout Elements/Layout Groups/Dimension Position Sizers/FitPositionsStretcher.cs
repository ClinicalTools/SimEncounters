namespace ClinicalTools.Layout
{
    public class FitPositionsStretcher : IPositionsStretcher
    {
        readonly IPositionsStretcher stretchToPreferred = new PreferredPositionsStretcher();
        readonly IPositionsStretcher stretchPastPreferred = new StretchPastPreferredPositionsStretcher();

        public float[] SizeElementPositions(IDimensionLayout[] dimensions, float size)
        {
            var maxPreferredSize = MaxPreferredSize(dimensions);
            if (maxPreferredSize > size)
                return stretchToPreferred.SizeElementPositions(dimensions, size);
            else
                return stretchPastPreferred.SizeElementPositions(dimensions, size);
        }

        private float MaxPreferredSize(IDimensionLayout[] dimensions)
        {
            var size = 0f;
            foreach (var dimension in dimensions) {
                if (dimension.Preferred != null) {
                    size += (float)dimension.Preferred;
                    continue;
                }

                if (dimension.Max == null)
                    return float.MaxValue;

                size += (float)dimension.Max;
            }

            return size;
        }
    }
}