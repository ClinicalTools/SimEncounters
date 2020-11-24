using ClinicalTools.SimEncounters;
using ClinicalTools.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEImageUploaderUI : KeyedSpriteUploader
    {
        public virtual Toggle PatientImageToggle { get => patientImageToggle; set => patientImageToggle = value; }
        [SerializeField] private Toggle patientImageToggle;

        protected string PatientImageKey { get; } = "patientImage";

        protected override void Awake()
        {
            base.Awake();

            patientImageToggle.onValueChanged.AddListener(ToggleValueChanged);
        }

        protected virtual void ToggleValueChanged(bool isOn) => UploadImageButton.interactable = !isOn;

        public override WaitableTask<string> SelectSprite(KeyedCollection<Sprite> sprites, string spriteKey)
        {
            gameObject.SetActive(true);
            var waitableSprite = base.SelectSprite(sprites, spriteKey);

            var isPatientImage = CurrentKey == PatientImageKey;
            if (isPatientImage)
                CurrentKey = null;
            PatientImageToggle.isOn = isPatientImage;
            ToggleValueChanged(isPatientImage);

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
