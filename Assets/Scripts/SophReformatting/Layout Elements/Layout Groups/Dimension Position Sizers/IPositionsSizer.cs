namespace ClinicalTools.Layout
{
    public interface IPositionsSizer
    {
        float[] SizeElementPositions(IDimensionLayout[] dimensions, float width);
    }
    public interface IPositionsStretcher : IPositionsSizer { }
    public interface IPositionsShrinker : IPositionsSizer { }
}