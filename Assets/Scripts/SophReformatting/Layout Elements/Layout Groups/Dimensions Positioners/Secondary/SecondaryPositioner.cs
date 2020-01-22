namespace ClinicalTools.Layout
{
    public class SecondaryPositioner : ISecondaryDimensionPositioner
    {
        public PositionedDimension PositionDimension(Padding padding, float size)
        {
            var start = padding.Start;
            var end = size - start - padding.End;
            return new PositionedDimension(start, end);
        }
    }
}