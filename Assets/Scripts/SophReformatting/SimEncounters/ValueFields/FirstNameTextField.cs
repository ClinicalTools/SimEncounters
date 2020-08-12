using ClinicalTools.SimEncounters.Data;
namespace ClinicalTools.ClinicalEncounters
{
    public class FirstNameTextField : NameTextField
    {
        public override string Value {
            get {
                if (Metadata?.Name != null)
                    Metadata.Name.FirstName = base.Value;
                return null;
            }
        }
        public override void Initialize(Encounter encounter)
        {
            base.Initialize(encounter);

            if (Metadata?.Name != null)
                InputField.text = Metadata.Name.FirstName;
        }
    }
}