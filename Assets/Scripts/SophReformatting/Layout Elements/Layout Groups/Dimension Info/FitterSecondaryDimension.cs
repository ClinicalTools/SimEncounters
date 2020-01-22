using System;
using System.Collections.Generic;

namespace ClinicalTools.Layout
{
    public class FitterSecondaryDimension : IDimensionLayout
    {
        private readonly IDimensionLayout dimensionLayout;
        private readonly Padding padding;
        private readonly IDimensionLayout[] childDimensionLayouts;
        public FitterSecondaryDimension(IDimensionLayout dimensionLayout, Padding padding, IDimensionLayout[] childDimensionLayouts)
        {
            this.dimensionLayout = dimensionLayout;

            this.padding = padding;
            this.childDimensionLayouts = childDimensionLayouts;
        }

        public float? Min {
            get {
                var min = padding.Start + padding.End;
                var childrenMin = 0f;
                foreach (var childDimensionLayout in childDimensionLayouts) {
                    if (childDimensionLayout.Min > childrenMin)
                        childrenMin = (float)childDimensionLayout.Min;
                }
                min += childrenMin;

                if (dimensionLayout.Min > min)
                    return dimensionLayout.Min;
                else if (dimensionLayout.Max < min)
                    return dimensionLayout.Max;
                return min;
            }
        }

        public float? Max {
            get {
                var max = padding.Start + padding.End;
                var childrenMax = float.MaxValue;
                foreach (var childDimensionLayout in childDimensionLayouts) {
                    if (childDimensionLayout.Max < childrenMax)
                        childrenMax = (float)childDimensionLayout.Max;
                }
                max += childrenMax;

                if (dimensionLayout.Min > max)
                    return dimensionLayout.Min;
                else if (dimensionLayout.Max < max)
                    return dimensionLayout.Max;
                return max;
            }
        }

        public float? Preferred {
            get {
                var preferred = padding.Start + padding.End;

                var childrenPreferred = 0f;
                foreach (var childDimensionLayout in childDimensionLayouts) {
                    if (childDimensionLayout.Preferred > childrenPreferred)
                        childrenPreferred = (float)childDimensionLayout.Preferred;
                }

                if (dimensionLayout.Min > preferred)
                    return dimensionLayout.Min;
                else if (dimensionLayout.Max < preferred)
                    return dimensionLayout.Max;
                return preferred;
            }
        }
    }
}