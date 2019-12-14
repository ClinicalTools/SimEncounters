namespace ClinicalTools.SimEncounters.Data
{
    public class Encounter
    {
        public virtual SectionsData Content { get; }
        public virtual ImagesData Images { get; }

        public Encounter(SectionsData content, ImagesData images)
        {
            Content = content;
            Images = images;
        }
    }
}