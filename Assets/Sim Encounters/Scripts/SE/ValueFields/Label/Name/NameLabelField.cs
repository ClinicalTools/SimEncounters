using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class NameLabelField : BaseEncounterField
    {
        protected virtual EncounterMetadata Metadata { get; set; }

        public override string Name => name;
        public override string Value => Label.text;

        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }
    }
}