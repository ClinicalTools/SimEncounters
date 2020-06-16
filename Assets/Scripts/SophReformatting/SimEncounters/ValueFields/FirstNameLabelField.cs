using ClinicalTools.SimEncounters.Data;
namespace ClinicalTools.ClinicalEncounters
{
    public class FirstNameLabelField : NameLabelField
    {
        public override void Initialize(Encounter encounter)
        {
            base.Initialize(encounter);

            if (Metadata != null)
                Label.text = Metadata.FirstName;
        }
    }
}