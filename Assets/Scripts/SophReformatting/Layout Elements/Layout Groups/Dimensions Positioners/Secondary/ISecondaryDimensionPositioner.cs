namespace ClinicalTools.Layout
{
    public interface ISecondaryDimensionPositioner
    {
        PositionedDimension PositionDimension(Padding padding, float size);
    }
}