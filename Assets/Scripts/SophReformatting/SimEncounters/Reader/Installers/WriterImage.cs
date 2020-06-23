using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterImage : BaseEncounterField
    {
        public override string Name => name;
        public override string Value => value;
        private string value = null;

        public Button SelectImageButton { get => selectImageButton; set => selectImageButton = value; }
        [SerializeField] private Button selectImageButton;

        protected Image Image
        {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }
        private Image image;

        protected BaseSpriteSelector SpritePopup { get; set; }
        [Inject] public virtual void Inject(BaseSpriteSelector spritePopup) => SpritePopup = spritePopup;

        protected Encounter CurrentEncounter { get; set; }

        protected virtual void Awake() => SelectImageButton.onClick.AddListener(SelectImage);

        public override void Initialize(Encounter encounter) => CurrentEncounter = encounter;
        public override void Initialize(Encounter encounter, string value)
        {
            CurrentEncounter = encounter;
            SetSprite(value);
        }

        public virtual void SetSprite(string imageKey)
        {
            value = imageKey;

            var sprites = CurrentEncounter.Images.Sprites;
            Color imageColor;
            if (imageKey != null && sprites.ContainsKey(imageKey)) {
                Image.sprite = sprites[imageKey];
                imageColor = Color.white;
            } else {
                imageColor = Color.clear;
            }

            Image.color = imageColor;
            Image.enabled = imageKey != null;
        }

        protected virtual void SelectImage()
        {
            var newImageKey = SpritePopup.SelectSprite(CurrentEncounter.Images.Sprites, Value);
            newImageKey.AddOnCompletedListener((result) => ImageSelected(newImageKey));
        }

        protected virtual void ImageSelected(WaitableResult<string> imageKey)
        {
            if (!imageKey.IsError)
                SetSprite(imageKey.Result);
        }
    }
}