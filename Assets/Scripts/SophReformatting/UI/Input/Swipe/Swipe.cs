using UnityEngine;

namespace ClinicalTools.UI
{
    public class Swipe
    {
        public Vector2 StartPosition { get; }
        public Vector2 LastPosition { get; set; }
        public bool Ended { get; set; }

        public float InitialAngle { get; }

        public Swipe(Vector2 startPosition)
        {
            StartPosition = startPosition;
            LastPosition = startPosition;
        }
        public Swipe(Vector2 startPosition, Vector2 endPosition)
        {
            StartPosition = startPosition;
            LastPosition = endPosition;
            InitialAngle = Vector2.Angle(Vector2.left, StartPosition - LastPosition);
            if (LastPosition.y < StartPosition.y)
                InitialAngle = 360 - InitialAngle;
        }
    }
}
