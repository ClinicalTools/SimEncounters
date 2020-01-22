namespace ClinicalTools.Layout
{
    public class AlignToStart : IElementPositioner
    {
        public PositionedDimension PositionElement(PositionedDimension layoutPosition, float size)
        {
            return new PositionedDimension(layoutPosition.StartPosition, size);
        }
    }
}