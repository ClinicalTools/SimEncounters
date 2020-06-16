using ClinicalTools.SimEncounters.Data;
namespace ClinicalTools.ClinicalEncounters
{
    public class LastNameLabelField : NameLabelField
    {
        public override void Initialize(Encounter encounter)
        {
            base.Initialize(encounter);

            if (Metadata != null)
                Label.text = Metadata.LastName;
        }
    }
}