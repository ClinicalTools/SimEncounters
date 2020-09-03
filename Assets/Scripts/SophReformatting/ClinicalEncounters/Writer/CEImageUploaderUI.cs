﻿using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEImageUploaderUI : ImageUploaderUI
    {
        public virtual Toggle PatientImageToggle { get => patientImageToggle; set => patientImageToggle = value; }
        [SerializeField] private Toggle patientImageToggle;

        protected string PatientImageKey { get; } = "patientImage";

        public override WaitableTask<string> SelectSprite(KeyedCollection<Sprite> sprites, string spriteKey)
        {
            var waitableSprite = base.SelectSprite(sprites, spriteKey);

            var isPatientImage = CurrentKey == PatientImageKey;
            if (isPatientImage)
                CurrentKey = null;
            PatientImageToggle.isOn = isPatientImage;

            return waitableSprite;
        }

        protected override void ApplyClicked()
        {
            if (!PatientImageToggle.isOn) {
                base.ApplyClicked();
                return;
            }

            if (CurrentKey != null && SpriteCollection.ContainsKey(CurrentKey))
                SpriteCollection.Remove(CurrentKey);
            CurrentWaitableSpriteKey.SetResult(PatientImageKey);
            Close();
        }
    }
}
