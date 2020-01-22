namespace ClinicalTools.Layout
{
    public class PreferredElementSizer : IElementSizer
    {
        public float SizeElement(IDimensionLayout dimension, float size)
        {
            if (dimension.Preferred != null)
                return (float)dimension.Preferred;
            else if (dimension.Max < size)
                return (float)dimension.Max;
            else if (dimension.Min > size)
                return (float)dimension.Min;
            else
                return size;
        }
    }
}