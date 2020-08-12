using ClinicalTools.SimEncounters.Data;
namespace ClinicalTools.ClinicalEncounters
{
    public class LastNameTextField : NameTextField
    {
        public override string Value {
            get {
                if (Metadata?.Name != null)
                    Metadata.Name.LastName = base.Value;
                return null;
            }
        }
        public override void Initialize(Encounter encounter)
        {
            base.Initialize(encounter);

            if (Metadata?.Name != null)
                InputField.text = Metadata.Name.LastName;
        }
    }
}