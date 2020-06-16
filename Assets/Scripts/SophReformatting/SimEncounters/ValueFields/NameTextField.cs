using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Data;
using TMPro;

namespace ClinicalTools.ClinicalEncounters
{
    public abstract class NameTextField : BaseEncounterField
    {
        protected virtual CEEncounterMetadata Metadata { get; set; }

        public override string Name => name;
        public override string Value => InputField.text;

        private TMP_InputField inputField;
        protected TMP_InputField InputField {
            get {
                if (inputField == null)
                    inputField = GetComponent<TMP_InputField>();
                return inputField;
            }
        }

        public override void Initialize(Encounter encounter) => SetMetadata(encounter);

        public override void Initialize(Encounter encounter, string value)
        {
            SetMetadata(encounter);
            InputField.text = value;
        }

        protected virtual void SetMetadata(Encounter encounter)
        {
            if (encounter.Metadata is CEEncounterMetadata ceMetadat)
                Metadata = ceMetadat;
        }
    }
}