using System;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class EncounterToggle<T> : ISelectable<T>
    {
        public event Action<T> Selected;
        public event Action<T> Unselected;
        protected Toggle Toggle { get; }
        protected T Value { get; }

        public EncounterToggle(Toggle toggle, T value)
        {
            Toggle = toggle;
            Value = value;
            toggle.onValueChanged.AddListener(ToggleChanged);
        }

        public virtual void Select()
        {
            // changing to selected color should be instantaneous here, so fade duration needs to be temporarily changed
            var fadeDuration = Toggle.colors.fadeDuration;
            SetToggleFadeDuration(Toggle, 0);
            Toggle.isOn = true;
            SetToggleFadeDuration(Toggle, fadeDuration);
        }

        protected virtual void ToggleChanged(bool isOn)
        {
            Toggle.interactable = !isOn;
            if (isOn)
                Selected?.Invoke(Value);
            else
                Unselected?.Invoke(Value);
        }
        protected virtual void SetToggleFadeDuration(Toggle toggle, float fadeDuration)
        {
            var colorGroup = toggle.colors;
            colorGroup.fadeDuration = fadeDuration;
            toggle.colors = colorGroup;
        }
    }
}