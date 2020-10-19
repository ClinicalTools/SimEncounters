namespace ClinicalTools.SimEncounters
{
    public class EncounterSelector : Selector<Encounter>
    {
        protected ISelector<EncounterMetadata> MetadataSelector { get; }
        public EncounterSelector(ISelector<EncounterMetadata> metadataSelector)
            => MetadataSelector = metadataSelector;

        public override void Select(object sender, Encounter value)
        {
            base.Select(sender, value);
            MetadataSelector.Select(this, value.Metadata);
        }
    }
}