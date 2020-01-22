namespace ClinicalTools.Layout
{
    public class ShrinkElementSizer : IElementSizer
    {
        public float SizeElement(IDimensionLayout dimension, float size)
        {
            if (dimension.Min > size)
                return (float)dimension.Min;
            else if (dimension.Preferred < size)
                return (float)dimension.Preferred;
            else if (dimension.Max < size)
                return (float)dimension.Max;
            else
                return size;
        }
    }
}