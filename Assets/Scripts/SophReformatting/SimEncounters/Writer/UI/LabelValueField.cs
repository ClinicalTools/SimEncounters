using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class LabelValueField : MonoBehaviour, IValueField
    {
        public string Name => name;
        public string Value => Label.text;

        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

        public void Initialize() { }
        public void Initialize(string value)
        {
            Label.text = value;
        }
    }
}