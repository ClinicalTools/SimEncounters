namespace ClinicalTools.Layout
{
    public class AlignToCenter : IElementPositioner
    {
        public PositionedDimension PositionElement(PositionedDimension layoutPosition, float size)
        {
            var offset = (layoutPosition.Size - size) / 2;
            var startPosition = layoutPosition.StartPosition + offset;
            return new PositionedDimension(startPosition, size);
        }
    }
}