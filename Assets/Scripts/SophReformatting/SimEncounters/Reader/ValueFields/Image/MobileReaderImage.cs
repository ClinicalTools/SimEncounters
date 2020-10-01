using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MobileReaderImage : UIBehaviour, IEncounterPanelField
    {
        private float width;

        private const float Tolerance = .0001f;
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var currentWidth = ((RectTransform)transform).rect.width;
            if (Mathf.Abs(currentWidth - width) < Tolerance)
                return;

            width = currentWidth;
            UpdateHeight();
        }

        public string Name => name;
        public string Value => value;
        private string value = null;

        public GameObject ImageGroup { get => imageGroup; set => imageGroup = value; }
        [SerializeField] private GameObject imageGroup;
        public float MaxHeight { get => maxHeight; set => maxHeight = value; }
        [SerializeField] private float maxHeight = -1;
        public LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }
        [SerializeField] private LayoutElement layoutElement;
        public Button EnlargeImageButton { get => enlargeImageButton; set => enlargeImageButton = value; }
        [SerializeField] private Button enlargeImageButton;

        private Image image;
        protected Image Image {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        protected SpriteDrawer SpritePopup { get; set; }
        [Inject] public virtual void Inject(SpriteDrawer spritePopup) => SpritePopup = spritePopup;

        public virtual void Initialize(Encounter encounter) => HideImage();
        public virtual void Initialize(Encounter encounter, string value)
        {
            this.value = value;
            var sprites = encounter.Content.ImageContent.Sprites;
            if (value != null && sprites.ContainsKey(value))
                SetSprite(sprites[value]);
            else
                HideImage();
        }

        public virtual void HideImage()
        {
            if (ImageGroup != null)
                ImageGroup.SetActive(false);
        }

        protected Sprite Sprite { get; set; }
        public virtual void SetSprite(Sprite sprite)
        {
            if (sprite == null) {
                Debug.LogError("Sprite is null");
                return;
            }

            Image.sprite = sprite;
            Sprite = sprite;

            UpdateHeight();
        }

        protected virtual void UpdateHeight()
        {
            if (Sprite == null)
                return;

            if (width == 0)
                width = ((RectTransform)transform).rect.width;

            var spriteHeight = Sprite.rect.height;
            var spriteWidth = Sprite.rect.width;
            var spriteRatio = spriteHeight / spriteWidth;

            if (EnlargeImageButton != null) {
                EnlargeImageButton.onClick.RemoveAllListeners();
                EnlargeImageButton.onClick.AddListener(() => SpritePopup.Display(Image.sprite));
            }

            if (LayoutElement == null)
                return;

            var height = spriteRatio * width;
            if (MaxHeight > 0)
                height = Mathf.Min(height, MaxHeight);
            LayoutElement.preferredHeight = height;

        }
    }
}