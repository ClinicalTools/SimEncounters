namespace ClinicalTools.SimEncounters
{
    public class EncounterSelector : Selector<EncounterSelectedEventArgs>
    {
        protected ISelector<EncounterMetadataSelectedEventArgs> MetadataSelector { get; }
        public EncounterSelector(ISelector<EncounterMetadataSelectedEventArgs> metadataSelector)
            => MetadataSelector = metadataSelector;

        public override void Select(object sender, EncounterSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            MetadataSelector.Select(this, new EncounterMetadataSelectedEventArgs(eventArgs.Encounter.Metadata));
        }
    }
}