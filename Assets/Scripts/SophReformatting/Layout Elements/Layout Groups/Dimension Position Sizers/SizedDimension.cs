namespace ClinicalTools.Layout
{
    public class SizedDimension
    {
        public float? Size { get; set; }
        public IDimensionLayout Dimension { get; }

        public SizedDimension(IDimensionLayout dimension)
        {
            Dimension = dimension;
        }
    }
}