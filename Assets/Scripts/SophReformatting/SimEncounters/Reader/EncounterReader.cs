using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class EncounterReader : EncounterScene
    {
        protected virtual object User { get; }
        public virtual Encounter Encounter { get; }
        public virtual ReaderSectionsGroup SectionsGroup { get; }
        protected ReaderUI ReaderUI { get; }
        public virtual ReaderPinManager Pins { get; }


        // combine user/loading screen and maybe encounter?
        public EncounterReader(object user, LoadingScreen loadingScreen, Encounter encounter, ReaderUI readerUI)
            : base(readerUI)
        {
            User = user;
            ReaderUI = readerUI;
            Encounter = encounter;

            Pins = CreatePinsManager();
            SectionsGroup = CreateSectionsGroup();

            AddListeners(ReaderUI);

            loadingScreen.Stop();
        }

        protected virtual ReaderSectionsGroup CreateSectionsGroup() => new ReaderSectionsGroup(this, ReaderUI.Sections, Encounter.Content.Sections);
        protected virtual ReaderPinManager CreatePinsManager() => new ReaderPinManager(this, ReaderUI.Pins);


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