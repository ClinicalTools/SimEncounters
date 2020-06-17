using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Data;
using TMPro;
namespace ClinicalTools.ClinicalEncounters
{
    public abstract class NameLabelField : BaseEncounterField
    {
        protected virtual CEEncounterMetadata Metadata { get; set; }

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

        public override void Initialize(Encounter encounter) => SetMetadata(encounter);
        public override void Initialize(Encounter encounter, string value)
        {
            SetMetadata(encounter);
            Label.text = value;
        }

        protected virtual void SetMetadata(Encounter encounter)
        {
            if (encounter.Metadata is CEEncounterMetadata ceMetadat)
                Metadata = ceMetadat;
        }
    }
}