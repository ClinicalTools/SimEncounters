using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class LabelValueField : MonoBehaviour, IValueField
    {
        public string Name => name;
        public string Value => Label.text;

        protected TextMeshProUGUI Label { get; set; }

        protected virtual void Awake()
        {
            Label = GetComponent<TextMeshProUGUI>();
        }

        public void Initialize() { }
        public void Initialize(string value)
        {
            Label.text = value;
        }
    }
}