using System;
using System.Collections;
using UnityEngine;

namespace ClinicalTools.UI
{
    public interface ICurve
    {
        float GetCurveY(float curveX);
        float GetCurveX(float curveY);
    }

    public class SquaredCurve : ICurve
    {
        private const float Exp = 1.25f;
        public virtual float GetCurveY(float curveX) => Mathf.Clamp01(1 - Mathf.Pow(1 - Mathf.Clamp01(curveX), Exp));
        public virtual float GetCurveX(float curveY) => Mathf.Clamp01(1 - Mathf.Pow(1 - Mathf.Clamp01(curveY), 1 / Exp));
    }
    public class AccCurve : ICurve
    {
        private const float InitialVelocity = 1.5f;
        private const float Acceleration = 1 - InitialVelocity;
        public virtual float GetCurveY(float curveX)
            => Mathf.Clamp01((Acceleration * Mathf.Clamp01(curveX) + InitialVelocity) * Mathf.Clamp01(curveX));
        public virtual float GetCurveX(float curveY)
        {
            var discriminant = InitialVelocity * InitialVelocity + 4 * Acceleration * Mathf.Clamp01(curveY);
            return (-InitialVelocity + Mathf.Sqrt(discriminant)) / (2 * Acceleration);
        }
    }

    public interface IShifter
    {
        IEnumerator ShiftForward(RectTransform leaving, RectTransform current);
        IEnumerator ShiftBackward(RectTransform leaving, RectTransform current);
        void SetPosition(RectTransform current);
        void SetMoveAmountForward(RectTransform leaving, RectTransform current, float moveAmount);
        void SetMoveAmountBackward(RectTransform leaving, RectTransform current, float moveAmount);
    }

    public class Shifter : IShifter
    {
        protected ICurve Curve { get; set; }
        public Shifter(ICurve curve) => Curve = curve;

        public virtual void SetPosition(RectTransform current)
        {
            current.anchorMin = new Vector2(0, current.anchorMin.y);
            current.anchorMax = new Vector2(1, current.anchorMax.y);
        }

        public virtual IEnumerator ShiftForward(RectTransform leaving, RectTransform current)
            => Shift(SetMoveAmountForward, leaving, current);
        public virtual IEnumerator ShiftBackward(RectTransform leaving, RectTransform current)
            => Shift(SetMoveAmountBackward, leaving, current);

        private const float MoveTime = .5f;
        protected virtual IEnumerator Shift(Action<RectTransform, RectTransform, float> moveAction,
            RectTransform leaving, RectTransform current)
        {
            var moveAmount = Mathf.Clamp01(Curve.GetCurveX(Mathf.Abs(leaving.anchorMin.x)));

            while (moveAmount < 1f) {
                moveAmount += Time.deltaTime / MoveTime;
                var position = Curve.GetCurveY(moveAmount);
                moveAction(leaving, current, position);
                yield return null;
            }

            SetPosition(current);
            leaving.gameObject.SetActive(false);
        }

        public virtual void SetMoveAmountForward(RectTransform leaving, RectTransform current, float moveAmount)
        {
            leaving.anchorMin = new Vector2(0 - moveAmount, leaving.anchorMin.y);
            leaving.anchorMax = new Vector2(1 - moveAmount, leaving.anchorMax.y);
            current.anchorMin = new Vector2(1 - moveAmount, leaving.anchorMin.y);
            current.anchorMax = new Vector2(2 - moveAmount, leaving.anchorMax.y);
        }

        public virtual void SetMoveAmountBackward(RectTransform leaving, RectTransform current, float moveAmount)
        {
            leaving.anchorMin = new Vector2(0 + moveAmount, leaving.anchorMin.y);
            leaving.anchorMax = new Vector2(1 + moveAmount, leaving.anchorMax.y);
            current.anchorMin = new Vector2(-1 + moveAmount, leaving.anchorMin.y);
            current.anchorMax = new Vector2(0 + moveAmount, leaving.anchorMax.y);
        }
    }
}