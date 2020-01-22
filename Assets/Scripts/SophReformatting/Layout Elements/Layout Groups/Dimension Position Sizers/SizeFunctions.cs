using System.Collections.Generic;

namespace ClinicalTools.Layout
{
    public class SizeFunctions : PositionFunctions
    {
        public float SizePositionsOutsideAverageSize(SizedDimension[] sizedDimensions, float size)
        {
            size = GetRemainingSize(sizedDimensions, size);
            var unsizedDimensions = UnsizedDimensionsCount(sizedDimensions);
            while (true) {
                if (unsizedDimensions == 0)
                    return 0;

                var preferredSize = size / unsizedDimensions;
                SizedDimension sizedDimension = GetFurthestRemainingDimension(sizedDimensions, preferredSize);
                if (sizedDimension == null)
                    return preferredSize;

                SizeOutsideDimension(sizedDimension, preferredSize);
                size -= (float)sizedDimension.Size;
                unsizedDimensions--;
            }
        }

        private int UnsizedDimensionsCount(SizedDimension[] dimensions)
        {
            var count = 0;
            foreach (var dimension in dimensions) {
                if (dimension.Size == null)
                    count++;
            }

            return count;
        }

        private SizedDimension GetFurthestRemainingDimension(IEnumerable<SizedDimension> sizedDimensions, float preferredSize)
        {
            SizedDimension furthestDimension = null;
            var furthestDistance = 0f;
            foreach (var sizedDimension in sizedDimensions) {
                if (sizedDimension.Size != null)
                    continue;

                var distance = DistanceFromPreferred(sizedDimension.Dimension, preferredSize);

                if (distance < furthestDistance)
                    continue;

                furthestDistance = distance;
                furthestDimension = sizedDimension;
            }

            return furthestDimension;
        }
    }
}