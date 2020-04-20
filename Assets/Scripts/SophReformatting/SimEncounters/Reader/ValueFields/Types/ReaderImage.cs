using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class SpriteDrawer : MonoBehaviour
    {
        public abstract void Display(Sprite sprite);
    }

    public class ReaderImage : MonoBehaviour, IUserValueField
    {
        public string Name => name;
        public string Value { get; protected set; }

        [SerializeField] private GameObject imageGroup;
        public GameObject ImageGroup { get => imageGroup; set => imageGroup = value; }

        [SerializeField] private bool expandToMax;
        public bool ExpandToMax { get => expandToMax; set => expandToMax = value; }
        [SerializeField] private float maxWidth = -1;
        public float MaxWidth { get => maxWidth; set => maxWidth = value; }
        [SerializeField] private float maxHeight = -1;
        public float MaxHeight { get => maxHeight; set => maxHeight = value; }
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
        [SerializeField] private LayoutElement layoutElement;
        public LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }

        [SerializeField] private Button enlargeImageButton;
        public Button EnlargeImageButton { get => enlargeImageButton; set => enlargeImageButton = value; }

        private Image image;
        protected Image Image {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        protected SpriteDrawer SpritePopup { get; set; }
        [Inject]
        public virtual void Inject(SpriteDrawer spritePopup)
        {
            SpritePopup = spritePopup;
        }

        public virtual void Initialize(UserPanel userPanel)
        {
            HideImage();
        }
        public virtual void Initialize(UserPanel userPanel, string value)
        {
            var sprites = userPanel.Encounter.Data.Images.Sprites;
            if (sprites.ContainsKey(value))
                SetSprite(userPanel, sprites[value]);
            else
                HideImage();
        }

        public virtual void HideImage()
        {
            if (ImageGroup != null)
                ImageGroup.SetActive(false);
        }

        public virtual void SetSprite(UserPanel userPanel, Sprite sprite)
        {
            if (sprite == null) {
                Debug.LogError("Sprite is null");
                return;
            }

            var spriteHeight = sprite.rect.height;
            var spriteWidth = sprite.rect.width;
            var spriteRatio = spriteHeight / spriteWidth;

            Image.sprite = sprite;
            if (spriteRatio > MaxRatio && MaxHeight > 0) {
                LayoutElement.preferredHeight = GetSideLength(spriteHeight, MaxHeight);
                LayoutElement.preferredWidth = LayoutElement.preferredHeight * spriteRatio;
            } else {
                LayoutElement.preferredWidth = GetSideLength(spriteWidth, MaxWidth);
                LayoutElement.preferredHeight = LayoutElement.preferredWidth * spriteRatio;
            }

            LayoutElement.minWidth = LayoutElement.preferredWidth;
            LayoutElement.minHeight = LayoutElement.preferredHeight;

            if (EnlargeImageButton != null) {
                EnlargeImageButton.onClick.RemoveAllListeners();
                EnlargeImageButton.onClick.AddListener(() => SpritePopup.Display(sprite));
            }
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