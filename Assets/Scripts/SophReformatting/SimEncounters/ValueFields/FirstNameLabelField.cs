using ClinicalTools.SimEncounters.Data;
namespace ClinicalTools.ClinicalEncounters
{
    public class FirstNameLabelField : NameLabelField
    {
        public override void Initialize(Encounter encounter, string value) => Initialize(encounter);
        public override void Initialize(Encounter encounter)
        {
            base.Initialize(encounter);

            if (Metadata?.Name != null)
                Label.text = Metadata.Name.FirstName;
        }
    }
}