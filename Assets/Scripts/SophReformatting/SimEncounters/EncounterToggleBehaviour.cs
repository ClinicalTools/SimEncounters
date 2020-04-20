using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class EncounterToggleBehaviour : MonoBehaviour
    {
        public event Action Selected;
        public event Action Unselected;

        [SerializeField] private Toggle toggle;
        public Toggle Toggle { get => toggle; set => toggle = value; }

        protected void Awake()
        {
            toggle.onValueChanged.AddListener(ToggleChanged);
        }

        public void Select()
        {
            // changing to selected color should be instantaneous here, so fade duration needs to be temporarily changed
            var fadeDuration = Toggle.colors.fadeDuration;
            SetToggleFadeDuration(0);
            Toggle.isOn = true;
            SetToggleFadeDuration(fadeDuration);
        }

        public void SetToggleGroup(ToggleGroup group) => Toggle.group = group;

        protected virtual void ToggleChanged(bool isOn)
        {
            Toggle.interactable = !isOn;
            if (isOn)
                Selected?.Invoke();
            else
                Unselected?.Invoke();
        }
        protected virtual void SetToggleFadeDuration(float fadeDuration)
        {
            var colorGroup = Toggle.colors;
            colorGroup.fadeDuration = fadeDuration;
            Toggle.colors = colorGroup;
        }
    }
}