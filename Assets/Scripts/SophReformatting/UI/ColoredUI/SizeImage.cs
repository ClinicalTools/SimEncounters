using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.UI
{
    [ExecuteAlways]
    public class SizeImage : MonoBehaviour
    {
        private const float DefaultScreenHeight = 1030;

        public float Scale { get => scale; set => scale = value; }
        [SerializeField] private float scale = .5f;

        protected RectTransform RectTransform => (RectTransform)transform;

        private Image image;
        protected Image Image {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        protected Texture2D Texture { get; set; }
        protected int ScreenHeight { get; set; }
        protected float LastScale { get; set; }
        protected virtual void Awake() => UpdateSize();
        protected virtual void Update()
        {
            if (Texture != Image.sprite.texture || ScreenHeight != Screen.height || LastScale != Scale)
                UpdateSize();
        }

        protected virtual void UpdateSize()
        {
            LastScale = Scale;
            ScreenHeight = Screen.height;
            Texture = Image.sprite.texture;
            if (Texture == null)
                return;
            var heightProportion = Screen.height / DefaultScreenHeight;
            heightProportion *= Scale;
            var height = heightProportion * Texture.height;
            var width = heightProportion * Texture.width;
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}