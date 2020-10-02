using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class SwipeManager : MonoBehaviour
    {
        private int itemsDisablingSwipe = 0;
        public void ReenableSwipe()
        {
            if (itemsDisablingSwipe > 0)
                itemsDisablingSwipe--;
        }
        public void DisableSwipe() => itemsDisablingSwipe++;
        public bool SwipeAllowed => itemsDisablingSwipe <= 0;

        private HashSet<SwipeParameter> SwipeParameters { get; } = new HashSet<SwipeParameter>();

        public void AddSwipeAction(SwipeParameter swipeParameter)
        {
            if (!SwipeParameters.Contains(swipeParameter))
                SwipeParameters.Add(swipeParameter);
        }
        public void RemoveSwipeAction(SwipeParameter swipeParameter)
        {
            if (SwipeParameters.Contains(swipeParameter))
                SwipeParameters.Remove(swipeParameter);
        }
        private void Update()
        {
            if (SwipeAllowed && (Input.touches.Length == 1 || (Input.touches.Length == 0 && Input.GetMouseButton(0))))
                TouchPosition(GetTouchPosition());
            else if (startPosition != null)
                FinishSwipe();
        }

        protected virtual Vector2 GetTouchPosition()
            => (Input.touches.Length == 1) ? Input.touches[0].position : (Vector2)Input.mousePosition;


        private Vector2? startPosition;
        private Swipe currentSwipe;
        private SwipeParameter currentParameter;
        public void TouchPosition(Vector2 position)
        {
            if (startPosition == null) {
                startPosition = position;
                return;
            }

            if (currentSwipe != null) {
                currentSwipe.LastPosition = position;
                if (currentParameter != null)
                    currentParameter.SwipeUpdate(currentSwipe);
                return;
            }

            var currentDistance = ((Vector2)startPosition) - position;
            currentDistance.x = Mathf.Abs(currentDistance.x);
            currentDistance.y = Mathf.Abs(currentDistance.y);
            if (currentDistance.x < (Screen.width * .01f) && currentDistance.y < (Screen.height * .01f))
                return;

            currentSwipe = new Swipe((Vector2)startPosition, position);
            foreach (var swipeParameter in SwipeParameters) {
                if (!swipeParameter.MeetsParamaters(currentSwipe))
                    continue;

                currentParameter = swipeParameter;
                currentParameter.SwipeStart(currentSwipe);
                return;
            }
        }

        public void FinishSwipe()
        {
            startPosition = null;
            if (currentSwipe == null)
                return;

            currentSwipe.Ended = true;
            if (currentParameter != null) {
                currentParameter.SwipeEnd(currentSwipe);
                currentParameter = null;
            }

            currentSwipe = null;
        }
    }
}
