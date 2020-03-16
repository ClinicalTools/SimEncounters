namespace ClinicalTools.SimEncounters.Data
{
    public class EncounterData
    {
        public virtual EncounterContent Content { get; }
        public virtual EncounterImageData Images { get; }

        public EncounterData(EncounterContent content, EncounterImageData images)
        {
            Content = content;
            Images = images;
        }
    }
}