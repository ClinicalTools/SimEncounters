using ClinicalTools.Layout;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class BetterCanvasScaler : CanvasScaler
    {
        // Some things only work with ints (like layout group padding),
        // so small width and height numbers don't allow for the precision sometimes needed
        private float scalar = 1f;
        protected override void HandleConstantPhysicalSize()
        {
#if UNITY_EDITOR
            var editorDPI = new NullableFloat(96);
            float currentDpi = (editorDPI.Value != null) ? (float)editorDPI.Value : Screen.dpi;
#else
            float currentDpi = Screen.dpi;

#endif
            float dpi = (currentDpi == 0 ? m_FallbackScreenDPI : currentDpi);
            float targetDPI = 1;
            switch (m_PhysicalUnit) {
                case Unit.Centimeters: targetDPI = 2.54f; break;
                case Unit.Millimeters: targetDPI = 25.4f; break;
                case Unit.Inches: targetDPI = 1; break;
                case Unit.Points: targetDPI = 72; break;
                case Unit.Picas: targetDPI = 6; break;
            }
            targetDPI *= scalar;

            var spriteDpiProportion = dpi / m_DefaultSpriteDPI;

            SetScaleFactor(dpi / targetDPI);
            SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit * targetDPI / m_DefaultSpriteDPI / spriteDpiProportion);
        }
    }
}
