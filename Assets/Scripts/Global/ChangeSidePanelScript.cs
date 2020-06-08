using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ChangeSidePanelScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action Selected;

        private Toggle toggle;
        protected Toggle Toggle {
            get {
                if (toggle == null) toggle = GetComponent<Toggle>();
                return toggle;
            }
        }
        public TextMeshProUGUI label;
        public Color onColor;
        public Color offColor;
        public Color hoverColor;

        // Use this for initialization
        protected void Start()
        {
            Toggle.onValueChanged.AddListener(ToggleThis);
            ToggleThis(Toggle.isOn);
        }

        protected void ToggleThis(bool isOn)
        {
            Color textColor;
            // 195 dark and 115 light
            if (isOn) {
                Selected?.Invoke();
                textColor = onColor;
            } else {
                textColor = offColor;
            }
            label.color = textColor;
            Toggle.interactable = !isOn;
        }

        public void OnPointerEnter(PointerEventData data)
        {
            if (!Toggle.isOn)
                label.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData data)
        {
            if (!Toggle.isOn)
                label.color = offColor;
        }

        public void Select() => Toggle.isOn = true;

        public void Show(string text)
        {
            label.text = text;
            gameObject.SetActive(true);
            Select();
        }

        public void Display() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public virtual bool IsOn() => Toggle.isOn;
    }
}