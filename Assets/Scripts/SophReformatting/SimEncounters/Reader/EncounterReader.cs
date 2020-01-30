using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class EncounterReader : EncounterScene
    {
        protected virtual object User { get; }
        public virtual Encounter Encounter { get; }
        public virtual ReaderSectionGroup SectionsGroup { get; }
        protected ReaderUI ReaderUI { get; }

        // combine user/loading screen and maybe encounter?
        public EncounterReader(object user, LoadingScreen loadingScreen, Encounter encounter, ReaderUI readerUI)
            : base(readerUI)
        {
            User = user;
            ReaderUI = readerUI;

            Encounter = encounter;

            SectionsGroup = new ReaderSectionGroup(this, ReaderUI.Sections, Encounter.Content.Sections);

            AddListeners(ReaderUI);

            loadingScreen.Stop();
        }


        protected virtual void AddListeners(ReaderUI readerUI)
        {
            //writerUI.HelpButton.onClick.AddListener(Help);
            readerUI.MainMenuButton.onClick.AddListener(ShowMainMenu);
        }

        public void Help()
        {

        }

        public void ShowMainMenu()
        {

        }
    }
}