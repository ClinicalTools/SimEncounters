﻿using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
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
            var spriteKey = PatientSpriteSelector.SelectSprite(Encounter.Images.Sprites, PatientImageKey);
            spriteKey.AddOnCompletedListener((key) => PatientImageSet());
        }

        protected virtual void PatientImageSet()
        {
            Sprite sprite;
            if (Encounter.Images.Sprites.ContainsKey(PatientImageKey))
                sprite = Encounter.Images.Sprites[PatientImageKey];
            else
                sprite = null;
            PatientImage.sprite = sprite;
        }
    }
}