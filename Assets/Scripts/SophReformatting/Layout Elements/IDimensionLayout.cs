using System;

namespace ClinicalTools.Layout
{
    public interface IDimensionLayout
    {
        float? Max { get; }
        float? Min { get; }
        float? Preferred { get; }
    }
}