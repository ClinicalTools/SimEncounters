using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class EncounterToggle<T> : EncounterButton<T>
    {
        protected Toggle Toggle { get; }

        public EncounterToggle(Toggle toggle, T value) : base(toggle, value)
        {
            Toggle = toggle;
            toggle.onValueChanged.AddListener(ToggleChanged);
        }
        public override void Select()
        {
            // changing to selected color should be instantaneous here, so fade duration needs to be temporarily changed
            var fadeDuration = Toggle.colors.fadeDuration;
            SetToggleFadeDuration(Toggle, 0);
            Toggle.isOn = true;
            SetToggleFadeDuration(Toggle, fadeDuration);
        }

        protected virtual void ToggleChanged(bool isOn) => Toggle.interactable = !isOn;

        protected virtual void SetToggleFadeDuration(Toggle toggle, float fadeDuration)
        {
            var colorGroup = toggle.colors;
            colorGroup.fadeDuration = fadeDuration;
            toggle.colors = colorGroup;
        }
    }
}