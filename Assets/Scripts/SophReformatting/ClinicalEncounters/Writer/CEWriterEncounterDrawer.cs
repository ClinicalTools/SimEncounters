﻿using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class CEWriterEncounterDrawer : WriterEncounterDrawer
    {
        public Button PatientImageButton { get => patientImageButton; set => patientImageButton = value; }
        [SerializeField] private Button patientImageButton;
        public Image PatientImage { get => patientImage; set => patientImage = value; }
        [SerializeField] private Image patientImage;

        protected string PatientImageKey { get; } = "patientImage";
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

            PatientImageSet();
        }

        protected virtual void SetPatientImage()
        {
            if (Encounter.Metadata.Sprite != null)
                Encounter.Content.ImageContent.Sprites.AddKeyedValue(PatientImageKey, Encounter.Metadata.Sprite);
            var spriteKey = PatientSpriteSelector.SelectSprite(Encounter.Content.ImageContent.Sprites, PatientImageKey);
            spriteKey.AddOnCompletedListener((key) => PatientImageSet());
        }

        protected virtual void PatientImageSet()
        {
            Sprite sprite;
            if (Encounter.Content.ImageContent.Sprites.ContainsKey(PatientImageKey)) {
                sprite = Encounter.Content.ImageContent.Sprites[PatientImageKey];
                Encounter.Content.ImageContent.Sprites.Remove(PatientImageKey);
            } else {
                sprite = Encounter.Metadata.Sprite;
            }
            Encounter.Metadata.Sprite = sprite;
            PatientImage.sprite = sprite;
        }
    }
}