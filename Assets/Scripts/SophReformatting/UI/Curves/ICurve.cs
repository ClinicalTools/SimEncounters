using System.Collections;
using System.Collections.Generic;
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
        public virtual float GetCurveY(float curveX) => Mathf.Clamp01(1 - Mathf.Pow(1 - curveX, 2));
        public virtual float GetCurveX(float curveY) => Mathf.Clamp01(1 - Mathf.Sqrt(1 - curveY));
    }
}