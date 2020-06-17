using ClinicalTools.SimEncounters.Data;
namespace ClinicalTools.ClinicalEncounters
{
    public class LastNameTextField : NameTextField
    {
        public override string Value {
            get {
                if (Metadata != null)
                    Metadata.LastName = base.Value;
                return null;
            }
        }
        public override void Initialize(Encounter encounter)
        {
            base.Initialize(encounter);

            if (Metadata != null)
                InputField.text = Metadata.LastName;
        }
    }
}