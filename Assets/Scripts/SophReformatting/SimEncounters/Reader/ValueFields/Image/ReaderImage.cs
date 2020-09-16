using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{

    public class ReaderImage : BaseEncounterField
    {
        public override string Name => name;
        public override string Value => value;
        private string value = null;

        public GameObject ImageGroup { get => imageGroup; set => imageGroup = value; }
        [SerializeField] private GameObject imageGroup;

        public bool ExpandToMax { get => expandToMax; set => expandToMax = value; }
        [SerializeField] private bool expandToMax;
        public float MaxWidth { get => maxWidth; set => maxWidth = value; }
        [SerializeField] private float maxWidth = -1;
        public float MaxHeight { get => maxHeight; set => maxHeight = value; }
        [SerializeField] private float maxHeight = -1;
        protected float MaxRatio {
            get {
                if (MaxHeight <= 0)
                    return 0;
                else if (MaxWidth <= 0)
                    return float.MaxValue;
                else
                    return MaxHeight / MaxWidth;
            }
        }

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

        public override void Initialize(Encounter encounter) => HideImage();
        public override void Initialize(Encounter encounter, string value)
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

        public virtual void SetSprite(Sprite sprite)
        {
            if (sprite == null) {
                Debug.LogError("Sprite is null");
                return;
            }

            var spriteHeight = sprite.rect.height;
            var spriteWidth = sprite.rect.width;
            var spriteRatio = spriteHeight / spriteWidth;

            Image.sprite = sprite;
            if (EnlargeImageButton != null) {
                EnlargeImageButton.onClick.RemoveAllListeners();
                EnlargeImageButton.onClick.AddListener(() => SpritePopup.Display(sprite));
            }

            if (LayoutElement == null)
                return;

            if (spriteRatio > MaxRatio && MaxHeight > 0) {
                LayoutElement.preferredHeight = GetSideLength(spriteHeight, MaxHeight);
                LayoutElement.preferredWidth = LayoutElement.preferredHeight * spriteRatio;
            } else {
                LayoutElement.preferredWidth = GetSideLength(spriteWidth, MaxWidth);
                LayoutElement.preferredHeight = LayoutElement.preferredWidth * spriteRatio;
            }

            LayoutElement.minWidth = LayoutElement.preferredWidth;
            LayoutElement.minHeight = LayoutElement.preferredHeight;
        }

        protected float GetSideLength(float preferredLength, float maxLength)
        {
            if (maxLength < 0)
                return preferredLength;
            else if (ExpandToMax)
                return maxLength;
            else
                return Mathf.Min(preferredLength, maxLength);
        }
    }
}