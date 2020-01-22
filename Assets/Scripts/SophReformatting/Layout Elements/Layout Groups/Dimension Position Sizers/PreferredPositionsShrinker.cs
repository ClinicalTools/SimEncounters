namespace ClinicalTools.Layout
{
    public class PreferredPositionsShrinker : IPositionsShrinker
    {
        public float[] SizeElementPositions(IDimensionLayout[] dimensions, float size)
        {
            float[] sizes = new float[dimensions.Length];
            for (var i = 0; i < dimensions.Length; i++)
                sizes[i] = GetPreferredSize(dimensions[i]);

            return sizes;
        }

        private float GetPreferredSize(IDimensionLayout dimension)
        {
            if (dimension.Preferred != null)
                return (float)dimension.Preferred;
            else if (dimension.Min != null)
                return (float)dimension.Min;
            else if (dimension.Max != null)
                return (float)dimension.Max;
            else
                return 0;
        }
    }
}