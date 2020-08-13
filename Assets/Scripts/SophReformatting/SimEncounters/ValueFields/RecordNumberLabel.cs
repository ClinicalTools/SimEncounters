using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Data;
using TMPro;

namespace ClinicalTools.ClinicalEncounters
{
    public class RecordNumberLabel : BaseEncounterField
    {
        protected virtual CEEncounterMetadata Metadata { get; set; }

        public override string Name => name;
        public override string Value => null;

        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

        public override void Initialize(Encounter encounter) => Label.text = encounter.Metadata.RecordNumber.ToString();
        public override void Initialize(Encounter encounter, string value) => Initialize(encounter);
    }
}