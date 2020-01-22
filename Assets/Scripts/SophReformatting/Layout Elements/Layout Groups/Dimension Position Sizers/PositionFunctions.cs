using System;
using System.Collections.Generic;

namespace ClinicalTools.Layout
{
    public class PositionFunctions
    {
        public float[] GetSizes(SizedDimension[] sizedDimensions)
        {
            float[] sizes = new float[sizedDimensions.Length];
            for (var i = 0; i < sizedDimensions.Length; i++)
                sizes[i] = (float)sizedDimensions[i].Size;

            return sizes;
        }

        public SizedDimension[] GetSizedDimensions(IDimensionLayout[] sizeFitters)
        {
            var count = sizeFitters.Length;
            SizedDimension[] childWidths = new SizedDimension[count];
            for (int i = 0; i < count; i++)
                childWidths[i] = new SizedDimension(sizeFitters[i]);

            return childWidths;
        }

        public void SizeRemainingPositions(IEnumerable<SizedDimension> sizedElements, float preferredSize)
        {
            foreach (var sizedElement in sizedElements) {
                if (sizedElement.Size != null)
                    continue;

                sizedElement.Size = preferredSize;
            }
        }

        protected void SizeOutsideDimension(SizedDimension sizedDimension, float preferredSize)
        {
            var dimension = sizedDimension.Dimension;
            if (dimension.Min > preferredSize)
                sizedDimension.Size = dimension.Min;
            else if (dimension.Max < preferredSize)
                sizedDimension.Size = dimension.Max;
        }

        protected float DistanceFromPreferred(IDimensionLayout dimension, float preferredSize)
        {
            var minDist = DistanceOfMinFromPreferred(dimension, preferredSize);
            var maxDist = DistanceOfMaxFromPreferred(dimension, preferredSize);
            return Math.Max(minDist, maxDist);
        }

        protected float DistanceOfMinFromPreferred(IDimensionLayout dimension, float preferredSize)
        {
            if (dimension.Min > preferredSize)
                return (float)dimension.Min - preferredSize;

            return -1;
        }
        protected float DistanceOfMaxFromPreferred(IDimensionLayout dimension, float preferredSize)
        {
            if (dimension.Max < preferredSize)
                return preferredSize - (float)dimension.Max;

            return -1;
        }

        protected float GetRemainingSize(SizedDimension[] sizedDimensions, float size)
        {
            foreach (var sizedDimension in sizedDimensions) {
                if (sizedDimension.Size != null)
                    size -= (float)sizedDimension.Size;
            }

            return size;
        }
    }
}