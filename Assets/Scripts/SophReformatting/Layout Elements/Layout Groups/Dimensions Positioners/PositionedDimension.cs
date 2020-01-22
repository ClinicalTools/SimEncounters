using UnityEngine;

namespace ClinicalTools.Layout
{
    public struct PositionedDimension
    {
        public float StartPosition { get; }
        public float Size { get; }

        public PositionedDimension(float startPosition, float size)
        {
            StartPosition = startPosition;
            Size = size;
        }
    }
}