namespace ClinicalTools.Layout
{
    public class FitElementSizer : IElementSizer
    {
        public float SizeElement(IDimensionLayout dimension, float size)
        {
            if (dimension.Max < size)
                return (float)dimension.Max;
            else if (dimension.Min > size)
                return (float)dimension.Min;
            else
                return size;
        }
    }
}