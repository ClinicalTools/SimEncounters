namespace ClinicalTools.SimEncounters
{
    public class FirstNameLabelField : NameLabelField
    {
        public override void Initialize(Encounter encounter, string value) => Initialize(encounter);
        public override void Initialize(Encounter encounter)
        {
            if (encounter.Metadata is INamed named && named?.Name != null)
                Label.text = named.Name.FirstName;
        }
    }
}