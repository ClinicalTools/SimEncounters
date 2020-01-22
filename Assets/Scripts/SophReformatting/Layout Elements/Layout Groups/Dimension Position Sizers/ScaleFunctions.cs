using System.Collections.Generic;

namespace ClinicalTools.Layout
{
    public class ScaleFunctions : PositionFunctions
    {
        public void SizePreferencePositions(IEnumerable<SizedDimension> sizedElements, float preferredScale)
        {
            foreach (var sizedElement in sizedElements) {
                if (sizedElement.Size != null)
                    continue;

                sizedElement.Size = sizedElement.Dimension.Preferred * preferredScale;
            }
        }

        public float SizePositionsOutsidePreferredScale(SizedDimension[] sizedDimensions, float size)
        {
            var preferredSize = GetPreferredSize(sizedDimensions);
            var remainingSize = GetRemainingSize(sizedDimensions, size);
            while (true) {
                if (remainingSize == 0)
                    return 0; 

                var preferredScale = remainingSize / preferredSize;
                SizedDimension furthestDimension = GetFurthestRemainingDimension(sizedDimensions, preferredScale);
                if (furthestDimension == null)
                    return preferredScale;

                var preferredDimensionSize = (float)furthestDimension.Dimension.Preferred * preferredScale;
                SizeOutsideDimension(furthestDimension, preferredDimensionSize);

                preferredSize -= (float)furthestDimension.Dimension.Preferred;
                remainingSize -= (float)furthestDimension.Size;
            }
        }

        private SizedDimension GetFurthestRemainingDimension(IEnumerable<SizedDimension> sizedDimensions, float preferredScale)
        {
            SizedDimension furthestDimension = null;
            var furthestDistance = 0f;
            foreach (var sizedDimension in sizedDimensions) {
                var dimension = sizedDimension.Dimension;
                if (sizedDimension.Size != null || dimension.Preferred == null)
                    continue;

                var preferredSize = preferredScale * (float)dimension.Preferred;
                var distance = DistanceFromPreferred(dimension, preferredSize);

                if (distance < furthestDistance)
                    continue;

                furthestDistance = distance;
                furthestDimension = sizedDimension;
            }

            return furthestDimension;
        }

        private float GetPreferredSize(SizedDimension[] sizedDimensions)
        {
            var size = 0f;
            foreach (var sizedDimension in sizedDimensions) {
                if (sizedDimension.Size == null && sizedDimension.Dimension.Preferred != null)
                    size += (float)sizedDimension.Dimension.Preferred;
            }

            return size;
        }
    }
}