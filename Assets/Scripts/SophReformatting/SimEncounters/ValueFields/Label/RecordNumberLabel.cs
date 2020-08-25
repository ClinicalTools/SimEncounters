using ClinicalTools.SimEncounters;

using TMPro;

namespace ClinicalTools.SimEncounters
{
    public class RecordNumberLabel : BaseEncounterField
    {
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