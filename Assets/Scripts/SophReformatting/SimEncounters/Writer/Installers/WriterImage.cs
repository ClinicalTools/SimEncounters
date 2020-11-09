using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterImage : BaseEncounterField
    {
        public override string Name => name;
        public override string Value => value;
        private string value = null;

        public Button SelectImageButton { get => selectImageButton; set => selectImageButton = value; }
        [SerializeField] private Button selectImageButton;

        protected Image Image {
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

            var sprite = GetSprite(imageKey);
            Color imageColor;
            if (sprite != null) {
                Image.sprite = sprite;
                imageColor = Color.white;
            } else {
                imageColor = Color.clear;
            }

            Image.color = imageColor;
            Image.enabled = imageKey != null;
        }

        private const string PatientImageKey = "patientImage";
        protected virtual Sprite GetSprite(string imageKey)
        {
            if (imageKey == null)
                return null;

            if (imageKey.Equals(PatientImageKey, StringComparison.InvariantCultureIgnoreCase))
                return CurrentEncounter.Metadata.Sprite;

            var sprites = CurrentEncounter.Content.ImageContent.Sprites;
            if (sprites.ContainsKey(imageKey))
                return sprites[imageKey];

            return null;
        }

        protected virtual void SelectImage()
        {
            var newImageKey = SpritePopup.SelectSprite(CurrentEncounter.Content.ImageContent.Sprites, Value);
            newImageKey.AddOnCompletedListener(ImageSelected);
        }

        protected virtual void ImageSelected(TaskResult<string> imageKey)
        {
            if (!imageKey.IsError())
                SetSprite(imageKey.Value);
        }
    }
}