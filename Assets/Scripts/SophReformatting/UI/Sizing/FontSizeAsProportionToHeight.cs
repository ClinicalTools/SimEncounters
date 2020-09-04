using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class FontSizeAsProportionToHeight : UIBehaviour
    {
        public float FontSizePerHeight { get => fontSizePerHeight; set => fontSizePerHeight = value; }
        [SerializeField] private float fontSizePerHeight = 1;

        private TMP_Text text;
        protected TMP_Text Text
        {
            get {
                if (text == null)
                    text = GetComponent<TMP_Text>();
                return text;
            }
        }

        private const float Tolerance = .0001f;
        private float height;

        protected override void Awake()
        {
            base.Awake();
            UpdateFontSize();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var currentHeight = ((RectTransform)transform).rect.height;
            if (Mathf.Abs(currentHeight - height) < Tolerance)
                return;

            height = currentHeight;
            UpdateFontSize();
        }

        private float lastFontSizePerHeight;
        protected void Update()
        {
            if (lastFontSizePerHeight != FontSizePerHeight) 
                UpdateFontSize();
        }

        protected virtual void UpdateFontSize()
        {
            if (height < Tolerance)
                height = ((RectTransform)transform).rect.height;

            lastFontSizePerHeight = FontSizePerHeight;
            Text.fontSize = FontSizePerHeight * height;
        }

    }
}