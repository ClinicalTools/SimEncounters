namespace ClinicalTools.SimEncounters.Data
{
    public class Encounter
    {
        public virtual EncounterMetadata Metadata { get; }
        public virtual EncounterContent Content { get; }
        public virtual EncounterImageData Images { get; }

        public Encounter(EncounterMetadata metadata, EncounterContent content, EncounterImageData images)
        {
            Metadata = metadata;
            Content = content;
            Images = images;
        }
    }
}