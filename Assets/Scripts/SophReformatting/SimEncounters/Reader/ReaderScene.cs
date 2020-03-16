using ClinicalTools.SimEncounters.Data;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderScene : EncounterScene
    {
        protected virtual ILoadingScreen LoadingScreen { get; }
        public virtual InfoNeededForReaderToHappen Data { get; }
        protected virtual User User { get; }
        public virtual EncounterData EncounterData { get; }
        public virtual ReaderSectionsGroup SectionsGroup { get; }
        protected ReaderUI ReaderUI { get; }
        public virtual ReaderPinManager Pins { get; }
        public virtual ReaderPopupManager Popups { get; }
        public virtual ReaderFooter Footer { get; }
        public MouseInput Mouse { get; internal set; }
        public ReaderTabDisplayFactory TabDisplayFactory { get; }
        public ReaderPanelDisplayFactory PanelDisplayFactory { get; }
        public ReaderValueFieldInitializer ValueFieldInitializer { get; }

        public HashSet<string> ReadTabs { get; } = new HashSet<string>();


        // combine user/loading screen and maybe encounter?
        public ReaderScene(InfoNeededForReaderToHappen data, ReaderUI readerUI)
            : base(readerUI)
        {
            Data = data;
            User = data.User;
            ReaderUI = readerUI;
            EncounterData = data.Encounter.Data;
            LoadingScreen = data.LoadingScreen;
            LoadingScreen?.Stop();

            TabDisplayFactory = CreateTabDisplayFactory();
            PanelDisplayFactory = CreatePanelDisplayFactory();
            ValueFieldInitializer = CreateValueFieldInitializer();

            Pins = CreatePinManager();
            Popups = CreatePopupManager();
            Footer = CreateFooter();
            SectionsGroup = CreateSectionsGroup();
            Mouse = readerUI.Mouse;
            var encounterInfo = CreateEncounterInfo();

            AddListeners(ReaderUI);
        }
        // combine user/loading screen and maybe encounter?
        public ReaderScene(User user, ILoadingScreen loadingScreen, EncounterData encounter, ReaderUI readerUI)
            : base(readerUI)
        {
            User = user;
            ReaderUI = readerUI;
            EncounterData = encounter;
            LoadingScreen = loadingScreen;
            LoadingScreen?.Stop();

            TabDisplayFactory = CreateTabDisplayFactory();
            PanelDisplayFactory = CreatePanelDisplayFactory();
            ValueFieldInitializer = CreateValueFieldInitializer();

            Pins = CreatePinManager();
            Popups = CreatePopupManager();
            Footer = CreateFooter();
            SectionsGroup = CreateSectionsGroup();
            Mouse = readerUI.Mouse;
            var encounterInfo = CreateEncounterInfo();

            AddListeners(ReaderUI);
        }

        protected virtual ReaderTabDisplayFactory CreateTabDisplayFactory() => new ReaderTabDisplayFactory(this);
        protected virtual ReaderPanelDisplayFactory CreatePanelDisplayFactory() => new ReaderPanelDisplayFactory(this);
        protected virtual ReaderValueFieldInitializer CreateValueFieldInitializer() => new ReaderValueFieldInitializer(this);
        protected virtual ReaderEncounterInfo CreateEncounterInfo() => new ReaderEncounterInfo(this, ReaderUI.EncounterInfo, Data.Encounter.Info.MetaGroup.CurrentInfo);
        protected virtual ReaderSectionsGroup CreateSectionsGroup() => new ReaderSectionsGroup(this, ReaderUI.Sections, EncounterData.Content);
        protected virtual ReaderPinManager CreatePinManager() => new ReaderPinManager(this, ReaderUI.Pins);
        protected virtual ReaderPopupManager CreatePopupManager() => new ReaderPopupManager(this, ReaderUI.Popups);
        protected virtual ReaderFooter CreateFooter() => new ReaderFooter(this, ReaderUI.Footer, EncounterData.Content.Sections);


        protected virtual void AddListeners(ReaderUI readerUI)
        {
            readerUI.MainMenuButton.onClick.AddListener(ShowMainMenu);
        }

        public void Help()
        {

        }

        public void ShowMainMenu()
        {
            EncounterSceneManager.EncounterInstance.StartMainMenuScene(User);
        }
    }
}