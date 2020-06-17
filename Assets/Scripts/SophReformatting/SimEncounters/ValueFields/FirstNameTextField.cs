using ClinicalTools.SimEncounters.Data;
namespace ClinicalTools.ClinicalEncounters
{
    public class FirstNameTextField : NameTextField
    {
        public override string Value {
            get {
                if (Metadata != null)
                    Metadata.FirstName = base.Value;
                return null;
            }
        }
        public override void Initialize(Encounter encounter)
        {
            base.Initialize(encounter);

            if (Metadata != null)
                InputField.text = Metadata.FirstName;
        }
    }
}