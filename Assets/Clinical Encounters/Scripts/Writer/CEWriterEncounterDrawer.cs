using ClinicalTools.SimEncounters;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEWriterEncounterDrawer : WriterEncounterDrawer
    {
        public Button PatientImageButton { get => patientImageButton; set => patientImageButton = value; }
        [SerializeField] private Button patientImageButton;
        public Image PatientImage { get => patientImage; set => patientImage = value; }
        [SerializeField] private Image patientImage;

        protected BaseSpriteSelector PatientSpriteSelector { get; set; }
        [Inject] public virtual void Inject(BaseSpriteSelector patientSpriteSelector) => PatientSpriteSelector = patientSpriteSelector;

        protected override void Awake()
        {
            base.Awake();

            PatientImageButton.onClick.AddListener(SetPatientImage);
        }

        public override void Display(Encounter encounter)
        {
            base.Display(encounter);

            PatientImage.sprite = Encounter.Metadata.Sprite;
        }

        protected virtual void SetPatientImage()
        {
            var sprite = Encounter.Metadata.Sprite;
            var key = (sprite != null) ? Encounter.Content.ImageContent.Sprites.Add(sprite) : null;
            var spriteKey = PatientSpriteSelector.SelectSprite(Encounter.Content.ImageContent.Sprites, key);
            spriteKey.AddOnCompletedListener(PatientImageSet);
        }

        protected virtual void PatientImageSet(TaskResult<string> key)
        {
            if (!key.HasValue())
                return;

            Sprite sprite;
            if (Encounter.Content.ImageContent.Sprites.ContainsKey(key.Value)) {
                sprite = Encounter.Content.ImageContent.Sprites[key.Value];
                Encounter.Content.ImageContent.Sprites.Remove(key.Value);
            } else {
                sprite = Encounter.Metadata.Sprite;
            }
            Encounter.Metadata.Sprite = sprite;
            PatientImage.sprite = sprite;
        }
    }
}