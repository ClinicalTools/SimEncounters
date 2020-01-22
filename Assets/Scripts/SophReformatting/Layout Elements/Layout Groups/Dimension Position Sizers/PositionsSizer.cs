using System.Collections.Generic;

namespace ClinicalTools.Layout
{
    public class PositionsSizer : IPositionsSizer
    {
        private readonly IPositionsShrinker positionsShrinker;
        private readonly IPositionsStretcher positionsStretcher;

        public PositionsSizer(IPositionsShrinker positionsShrinker, IPositionsStretcher positionsStretcher)
        {
            this.positionsShrinker = positionsShrinker;
            this.positionsStretcher = positionsStretcher;
        }

        public float[] SizeElementPositions(IDimensionLayout[] dimensions, float size)
        {
            float preferredSize = GetPreferredSize(dimensions);
            if (preferredSize < size)
                return positionsStretcher.SizeElementPositions(dimensions, size);
            else 
                return positionsShrinker.SizeElementPositions(dimensions, size);
        }

        private float GetPreferredSize(IEnumerable<IDimensionLayout> layoutDimensions)
        {
            var size = 0f;
            foreach (var layoutDimension in layoutDimensions) {
                if (layoutDimension.Preferred != null)
                    size += (float)layoutDimension.Preferred;
                else if (layoutDimension.Min != null)
                    size += (float)layoutDimension.Min;
            }

            return size;
        }
    }
}