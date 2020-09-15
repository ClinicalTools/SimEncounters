using System;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class SwipeParameter
    {
        public Rect? StartPositionRange { get; set; }
        public AngleRange AngleRange { get; set; }

        public event Action<Swipe> OnSwipeStart;
        public event Action<Swipe> OnSwipeUpdate;
        public event Action<Swipe> OnSwipeEnd;

        public bool MeetsParamaters(Swipe swipe)
            => StartPositionRange?.Contains(swipe.StartPosition) != false
            && AngleRange.ContainsAngle(swipe.InitialAngle);

        public void SwipeStart(Swipe swipe) => OnSwipeStart?.Invoke(swipe);
        public void SwipeUpdate(Swipe swipe) => OnSwipeUpdate?.Invoke(swipe);
        public void SwipeEnd(Swipe swipe) => OnSwipeEnd?.Invoke(swipe);
    }
}
