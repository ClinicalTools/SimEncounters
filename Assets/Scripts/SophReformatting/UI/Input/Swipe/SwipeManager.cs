using Boo.Lang;
using System;
using UnityEngine;

namespace ClinicalTools.UI
{

    /// <summary>
    /// Angles making up the range, with the angles being listed in counter-clockwise order.
    /// </summary>
    public class AngleRange
    {
        private const float DegreesInCircle = 360;

        private float start;
        public float Start {
            get => start;
            set => start = GetAngleValue(value);
        }
        private float end;
        public float End {
            get => end;
            set => end = GetAngleValue(value);
        }

        public AngleRange(float start, float end)
        {
            Start = start;
            End = end;
        }

        protected float GetAngleValue(float angle)
        {
            angle %= DegreesInCircle;
            if (angle < 0)
                angle += DegreesInCircle;
            return angle;
        }

        public bool ContainsAngle(float angle)
        {
            angle = GetAngleValue(angle);

            if (End > Start)
                return angle >= Start && angle <= End;
            else
                return angle >= Start || angle <= End;
        }

        public bool ContainsAngleRange(AngleRange angleRange)
        {
            if (End > Start) {
                if (angleRange.Start < Start || angleRange.Start > End)
                    return false;

                return angleRange.End >= angleRange.Start && angleRange.Start <= End;
            }

            if (angleRange.Start >= Start)
                return angleRange.End >= angleRange.Start || angleRange.End <= End;

            if (angleRange.Start <= End)
                return angleRange.End >= angleRange.Start && angleRange.End <= End;

            return false;
        }

        /// <summary>
        /// Adds an angle range to this angle range. Either the start or the end must be within the angle range.
        /// </summary>
        /// <param name="angleRange">Angle range to add</param>
        public void AddAngleRange(AngleRange angleRange)
        {
            var containsStart = ContainsAngle(angleRange.Start);
            var containsEnd = ContainsAngle(angleRange.End);
            if (containsStart == containsEnd)
                return;

            if (containsStart)
                End = angleRange.end;
            else
                Start = angleRange.Start;
        }

        public float GetRangeDistance() => GetAngleValue(End - Start);
        public void FlipStartAndEnd()
        {
            var oldStart = Start;
            Start = End;
            End = oldStart;
        }
    }

    public class SwipeParameters
    {
        public Rect? StartPositionRange { get; set; }
        public AngleRange AngleRange { get; set; }
    }
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
    public class SwipeManager : MonoBehaviour
    {
        // Amount angle can be from straight to count as a swipe
        private const float ANGLE_TOLERANCE = 15f;
        private const float ANGLE_MIN_DIST = 100f;

        private Vector2?[] touchPos = new Vector2?[15];
        private float timeSinceLastPress;

        private Swipe currentSwipe;

        private List<SwipeParameters> SwipeParameters { get; } = new List<SwipeParameters>();

        public void AddSwipeAction(SwipeParameters swipeParameters, Action<Swipe> action)
        {
            SwipeParameters.Add(swipeParameters);
        }

        private void Update()
        {
            if (Input.touches.Length != 1 && !Input.GetMouseButton(0)) {
                startPosition = null;
                if (currentSwipe != null)
                    FinishSwipe();
                return;
            }

            Vector2 newPos = (Input.touches.Length == 1) ? Input.touches[0].position : (Vector2)Input.mousePosition;
            TouchPosition(newPos);
        }

        private Vector2? startPosition;
        public void TouchPosition(Vector2 position)
        {
            if (currentSwipe != null) {
                currentSwipe.LastPosition = position;
                return;
            }

            if (startPosition == null) {
                startPosition = position;
                return;
            }

            var currentDistance = ((Vector2)startPosition) - position;
            if (Mathf.Abs(currentDistance.x) < (Screen.width * .01f) && Mathf.Abs(currentDistance.y) < (Screen.height * .01f))
                return;

            currentSwipe = new Swipe((Vector2)startPosition, position);
            Debug.Log(currentSwipe.InitialAngle);
            foreach (var swipeParameter in SwipeParameters) {
                if (!swipeParameter.AngleRange.ContainsAngle(currentSwipe.InitialAngle))
                    continue;
                if (swipeParameter.StartPositionRange?.Contains(position) != false) {
                    return;
                }
            }
        }

        public void FinishSwipe()
        {
            currentSwipe = null;
            return;

            Vector2 firstPoint = (Vector2)touchPos[0];
            Vector2 lastPoint = firstPoint;
            for (int i = touchPos.Length - 1; i >= 0; i--) {
                if (touchPos[i] != null) {
                    lastPoint = (Vector2)touchPos[i];
                    break;
                }
            }

            if (Mathf.Abs(firstPoint.x - lastPoint.x) > ANGLE_MIN_DIST) {
                var angle = Vector2.Angle(Vector2.left, firstPoint - lastPoint);
                if (angle < ANGLE_TOLERANCE) {
                    SwipeRight();
                } else if (180 - angle < ANGLE_TOLERANCE) {
                    SwipeLeft();
                }
            }

            touchPos = new Vector2?[touchPos.Length];
        }

        public void SwipeRight()
        {
            /*
            if (!OnValidScreen())
                return;

            TabInfoScript currentTab = ds.GetData(tm.getCurrentSection()).GetTabInfo(tm.getCurrentTab());

            if (currentTab.position > 0)
            {
                //This is the name of the next tab.
                string newTabName = ds.GetData(tm.getCurrentSection()).GetTabList()[currentTab.position - 1];
                tm.setTabName(newTabName);
                tm.SwitchTab(newTabName);
            }
            else
            {
                var sectionList = ds.GetSectionsList();
                var nextSectionIndex = sectionList.FindIndex((string obj) => obj.Equals(tm.getCurrentSection())) - 1;
                if (nextSectionIndex >= 0)
                {
                    string lastSection = sectionList[nextSectionIndex];
                    tm.SwitchSection(lastSection, true);
                }
            }*/
        }

        public void SwipeLeft()
        {
            /*
            if (!OnValidScreen())
                return; 

            TabInfoScript currentTab = ds.GetData(tm.getCurrentSection()).GetTabInfo(tm.getCurrentTab());

            if (ds.GetData(tm.getCurrentSection()).GetTabList().Count > currentTab.position + 1)
            {
                //This is the name of the next tab.
                string newTabName = ds.GetData(tm.getCurrentSection()).GetTabList()[currentTab.position + 1];
                tm.setTabName(newTabName);
                tm.SwitchTab(newTabName);
            }
            else if (!ds.forceInOrder || ds.GetData(tm.getCurrentSection()).AllTabsVisited())
            {
                var nextSectionIndex = ds.GetSectionsList().FindIndex((string obj) => obj.Equals(tm.getCurrentSection())) + 1;
                if (ds.GetSectionsList().Count > nextSectionIndex)
                {
                    string nextSection = ds.GetSectionsList()[nextSectionIndex];
                    tm.SwitchSection(nextSection);
                }
            }*/
        }
    }
}
