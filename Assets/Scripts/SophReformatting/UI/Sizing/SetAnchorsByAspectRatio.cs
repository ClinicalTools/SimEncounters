using System;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class SetAnchorsByAspectRatio : MonoBehaviour
    {
        public Vector2 LandscapeMinAnchor { get => landscapeMinAnchor; set => landscapeMinAnchor = value; }
        [Tooltip("Minimum anchor in a 16:9 resolution")]
        [SerializeField] private Vector2 landscapeMinAnchor;
        public Vector2 LandscapeMaxAnchor { get => landscapeMaxAnchor; set => landscapeMaxAnchor = value; }
        [Tooltip("Maximum anchor in a 16:9 resolution")]
        [SerializeField] private Vector2 landscapeMaxAnchor;
        public Vector2 PortraitMinAnchor { get => portraitMinAnchor; set => portraitMinAnchor = value; }
        [Tooltip("Minimum anchor in a 9:16 resolution")]
        [SerializeField] private Vector2 portraitMinAnchor;
        public Vector2 PortraitMaxAnchor { get => portraitMaxAnchor; set => portraitMaxAnchor = value; }
        [Tooltip("Maximum anchor in a 9:16 resolution")]
        [SerializeField] private Vector2 portraitMaxAnchor;


        private const float LandscapeAspectRatio = 9f / 16;
        private const float PortraitAspectRatio = 16f / 9;

        protected virtual void Awake() => UpdateSize();

        protected virtual void Update() => UpdateSize();

        private const float Tolerance = .00001f;
        private Vector2 canvasSize = new Vector2();
        protected virtual void UpdateSize()
        {
            var currentCanvasSize = new Vector2(Screen.width, Screen.height);
            if (currentCanvasSize == canvasSize)
                return;

            canvasSize = currentCanvasSize;
            var currentAspectRatio = canvasSize.y / canvasSize.x;

            var rectTransform = (RectTransform)transform;
            var anchorMin = rectTransform.anchorMin;
            var anchorMax = rectTransform.anchorMax;

            if (LandscapeMinAnchor.x > Tolerance || PortraitMinAnchor.x > Tolerance)
                anchorMin.x = GetValue(currentAspectRatio, LandscapeMinAnchor.x, PortraitMinAnchor.x);
            if (LandscapeMinAnchor.y > Tolerance || PortraitMinAnchor.y > Tolerance)
                anchorMin.y = GetValue(currentAspectRatio, LandscapeMinAnchor.y, PortraitMinAnchor.y);

            if (LandscapeMaxAnchor.x > Tolerance || PortraitMaxAnchor.x > Tolerance)
                anchorMax.x = GetValue(currentAspectRatio, LandscapeMaxAnchor.x, PortraitMaxAnchor.x);
            if (LandscapeMaxAnchor.y > Tolerance || PortraitMaxAnchor.y > Tolerance)
                anchorMax.y = GetValue(currentAspectRatio, LandscapeMaxAnchor.y, PortraitMaxAnchor.y);

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = new Vector2();
            rectTransform.offsetMax = new Vector2();
        }

        private float GetValue(float currentAspectRatio, float landscapeValue, float portraitValue)
        {
            var slope = (portraitValue - landscapeValue)
                            / (PortraitAspectRatio - LandscapeAspectRatio);
            var y1 = landscapeValue;
            var x1 = LandscapeAspectRatio;
            var x = currentAspectRatio;

            return slope * (x - x1) + y1;
        }
    }
}