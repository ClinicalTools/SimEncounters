using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Extensions
{
    public static class Extensions
    {
        private static readonly System.Random rng = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void AddOnSelectListener(this Toggle toggle, UnityAction action)
        {
            toggle.onValueChanged.AddListener(
                (selected) => {
                    if (selected) action();
                }
            );
        }


        public static void EnsureChildIsShowing(this ScrollRect scrollRect, RectTransform childTransform)
        {
            if (scrollRect.horizontal)
                EnsureHorizontalChildIsShowing(scrollRect, childTransform);
            if (scrollRect.vertical)
                EnsureVerticalChildIsShowing(scrollRect, childTransform);
        }

        private static void EnsureHorizontalChildIsShowing(ScrollRect scrollRect, RectTransform childTransform)
        {
            scrollRect.horizontalNormalizedPosition =
                GetNewNormalizedPosition(scrollRect.viewport.rect.width, scrollRect.content.rect.width,
                scrollRect.content.pivot.x, childTransform.localPosition.x, childTransform.rect.width,
                scrollRect.horizontalNormalizedPosition);
        }
        private static void EnsureVerticalChildIsShowing(ScrollRect scrollRect, RectTransform childTransform)
        {
            scrollRect.verticalNormalizedPosition = 
                GetNewNormalizedPosition(scrollRect.viewport.rect.height, scrollRect.content.rect.height,
                scrollRect.content.pivot.y, childTransform.localPosition.y, childTransform.rect.height, 
                scrollRect.verticalNormalizedPosition);
        }

        private static float GetNewNormalizedPosition(float viewportLength, float contentLength,
            float contentPivot, float childLocalPosition, float childLength, float currentNormalizedPosition)
        {
            if (viewportLength < 0)
                return currentNormalizedPosition;

            var scrollableDistance = contentLength - viewportLength;
            if (scrollableDistance < 0)
                return currentNormalizedPosition;

            var contentEnd = contentLength * contentPivot;
            var childPositionFromEnd = contentEnd + childLocalPosition;
            var childStart = (childPositionFromEnd - childLength) / scrollableDistance;
            var childEnd = (childPositionFromEnd - viewportLength) / scrollableDistance;

            return Mathf.Clamp(currentNormalizedPosition, childEnd, childStart);

        }
    }
}