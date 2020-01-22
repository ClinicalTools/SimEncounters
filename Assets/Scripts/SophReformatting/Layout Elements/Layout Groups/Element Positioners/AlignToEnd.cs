namespace ClinicalTools.Layout
{
    public class AlignToEnd : IElementPositioner
    {
        public PositionedDimension PositionElement(PositionedDimension layoutPosition, float size)
        {
            var startPosition = layoutPosition.StartPosition + layoutPosition.Size - size;
            return new PositionedDimension(startPosition, size);
        }
    }
}