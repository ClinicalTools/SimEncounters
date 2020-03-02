namespace ClinicalTools.SimEncounters.Data
{
    public class Encounter
    {
        public virtual EncounterInfoGroup Info { get; }
        public virtual SectionsData Content { get; }
        public virtual ImagesData Images { get; }

        public Encounter(EncounterInfoGroup info, SectionsData content, ImagesData images)
        {
            Info = info;
            Content = content;
            Images = images;
        }
    }
}