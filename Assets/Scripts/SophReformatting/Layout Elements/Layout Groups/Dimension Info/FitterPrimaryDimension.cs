using System;

namespace ClinicalTools.Layout
{
    public class FitterPrimaryDimension : IDimensionLayout
    {
        private readonly IDimensionLayout dimensionLayout;
        private readonly SpacedPadding padding;
        private readonly IDimensionLayout[] childDimensionLayouts;
        public FitterPrimaryDimension(IDimensionLayout dimensionLayout, SpacedPadding padding, IDimensionLayout[] childDimensionLayouts)
        {
            this.dimensionLayout = dimensionLayout;
            this.padding = padding;
            this.childDimensionLayouts = childDimensionLayouts;
        }

        public float? Min {
            get {
                var min = padding.Start + padding.End;
                if (childDimensionLayouts.Length > 1)
                    min += padding.Spacing * (childDimensionLayouts.Length - 1);
                foreach (var childDimensionLayout in childDimensionLayouts) {
                    if (childDimensionLayout.Min != null)
                        min += (float)childDimensionLayout.Min;
                }

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
                if (childDimensionLayouts.Length > 1)
                    max += padding.Spacing * (childDimensionLayouts.Length - 1);
                foreach (var childDimensionLayout in childDimensionLayouts) {
                    if (childDimensionLayout.Max != null)
                        max += (float)childDimensionLayout.Max;
                    else
                        return dimensionLayout.Max;
                }


                if (dimensionLayout.Min > max)
                    return dimensionLayout.Min;
                else if(dimensionLayout.Max < max)
                    return dimensionLayout.Max;
                return max;
            }
        }

        public float? Preferred {
            get {
                var preferred = padding.Start + padding.End;
                if (childDimensionLayouts.Length > 1)
                    preferred += padding.Spacing * (childDimensionLayouts.Length - 1);
                foreach (var childDimensionLayout in childDimensionLayouts) {
                    if (childDimensionLayout.Preferred != null)
                        preferred += (float)childDimensionLayout.Preferred;
                    else if (childDimensionLayout.Min != null)
                        preferred += (float)childDimensionLayout.Min;
                    else if (childDimensionLayout.Max != null)
                        preferred += (float)childDimensionLayout.Max;
                    else
                        return null;
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