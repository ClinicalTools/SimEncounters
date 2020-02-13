namespace ClinicalTools.SimEncounters.Data
{
    public class Encounter
    {
        public virtual EncounterInfo Info { get; }
        public virtual SectionsData Content { get; }
        public virtual ImagesData Images { get; }

        public Encounter(EncounterInfo info, SectionsData content, ImagesData images)
        {
            Info = info;
            Content = content;
            Images = images;
        }
    }
}