using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class FontSizeAsProportionToHeight : UIBehaviour
    {
        public float FontSizePerHeight { get => fontSizePerHeight; set => fontSizePerHeight = value; }
        [SerializeField] private float fontSizePerHeight;

        private TMP_Text text;
        protected TMP_Text Text
        {
            get {
                if (text == null)
                    text = GetComponent<TMP_Text>();
                return text;
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            Text.fontSize = FontSizePerHeight * ((RectTransform)transform).rect.height;
        }
    }
}