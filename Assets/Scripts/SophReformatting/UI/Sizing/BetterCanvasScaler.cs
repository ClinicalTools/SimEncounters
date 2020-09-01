using ClinicalTools.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class BetterCanvasScaler : CanvasScaler
    {
        protected override void HandleConstantPhysicalSize()
        {
#if UNITY_EDITOR
            var editorDPI = new NullableFloat(72);
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
            //targetDPI *= 2;

            var spriteDpiProportion = dpi / m_DefaultSpriteDPI;

            SetScaleFactor(dpi / targetDPI);
            SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit * targetDPI / m_DefaultSpriteDPI / spriteDpiProportion);
        }


    }
}
