using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.Layout
{
    public interface IPrimaryDimensionPositioner
    {
        PositionedDimension[] PositionDimensions(float[] sizes, SpacedPadding padding, float size);
    }
}