namespace ClinicalTools.Layout
{
    public class StretchElementSizer : IElementSizer
    {
        public float SizeElement(IDimensionLayout dimension, float size)
        {
            if (dimension.Max < size)
                return (float)dimension.Max;
            else if (dimension.Preferred > size)
                return (float)dimension.Preferred;
            else if (dimension.Min > size)
                return (float)dimension.Min;
            else
                return size;
        }
    }
}