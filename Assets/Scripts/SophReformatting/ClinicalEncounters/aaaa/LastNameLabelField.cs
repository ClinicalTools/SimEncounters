using ClinicalTools.SimEncounters;

namespace ClinicalTools.ClinicalEncounters
{
    public class LastNameLabelField : NameLabelField
    {
        public override void Initialize(Encounter encounter, string value) => Initialize(encounter);
        public override void Initialize(Encounter encounter)
        {
            base.Initialize(encounter);

            if (Metadata?.Name != null)
                Label.text = Metadata.Name.LastName;
        }
    }
}