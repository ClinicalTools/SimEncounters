using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class DropdownOption : MonoBehaviour
    {
        public TextMeshProUGUI OptionText { get => optionText; set => optionText = value; }
        [SerializeField] private TextMeshProUGUI optionText;

        public Button SelectButton { get => selectButton; set => selectButton = value; }
        [SerializeField] private Button selectButton;


        public event Action<string> Selected;
        public string Value { get; protected set; }

        protected virtual void Awake() => SelectButton.onClick.AddListener(SelectButtonPressed);
        public virtual void Display(string value)
        {
            Value = value;
            OptionText.text = value;
        }
        public virtual void SetActive(bool active) => gameObject.SetActive(active);
        public virtual void Highlight() => SelectButton.image.color = new Color(.8f, 1f, 1f);
        public virtual void RemoveHighlight() => SelectButton.image.color = new Color(1f, 1f, 1f);
        protected virtual void SelectButtonPressed() => Selected?.Invoke(Value);

    }
}