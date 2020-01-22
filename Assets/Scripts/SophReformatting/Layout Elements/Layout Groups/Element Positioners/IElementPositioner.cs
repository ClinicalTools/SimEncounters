namespace ClinicalTools.Layout
{
    public interface IElementPositioner
    {
        PositionedDimension PositionElement(PositionedDimension layoutPosition, float size);
    }
}