﻿using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Writer
{
    public class EncounterWriter : EncounterScene
    {
        protected virtual object User { get; }
        public virtual Encounter Encounter { get; }
        public virtual SectionsGroup SectionsGroup { get; }
        protected WriterUI WriterUI { get; }
        public TabTypesInfo TabTypes { get; } = new TabTypesInfo();

        // combine user/loading screen and maybe encounter?
        public EncounterWriter(object user, LoadingScreen loadingScreen, Encounter encounter, WriterUI writerUI) 
            : base(writerUI)
        {
            User = user;
            WriterUI = writerUI;

            Encounter = encounter;

            SectionsGroup = new SectionsGroup(WriterUI.Sections, Encounter.Content.Sections, this);

            AddListeners(WriterUI);

            loadingScreen.Stop();
        }


        protected virtual void AddListeners(WriterUI writerUI)
        {
            writerUI.ExitButton.onClick.AddListener(Exit);
            //writerUI.HelpButton.onClick.AddListener(Help);
            writerUI.MainMenuButton.onClick.AddListener(ShowMainMenu);
            writerUI.SaveAndViewButton.onClick.AddListener(SaveAndView);
            writerUI.SaveButton.onClick.AddListener(Save);
            writerUI.VariablesButton.onClick.AddListener(EditVariables);
        }

        public void Exit()
        {

        }

        public void Help()
        {

        }

        public void ShowMainMenu()
        {

        }

        public void SaveAndView()
        {

        }

        public void Save()
        {

        }

        public void EditVariables()
        {

        }
    }
}