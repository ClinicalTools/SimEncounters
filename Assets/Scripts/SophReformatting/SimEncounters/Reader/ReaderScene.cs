using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.MainMenu;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderScene : EncounterScene
    {
        protected virtual ILoadingScreen LoadingScreen { get; }
        public virtual EncounterSceneInfo SceneInfo { get; }
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

        //public HashSet<string> ReadTabs { get; } = new HashSet<string>();
        public bool IsTabRead(string tabKey)
        {
            return SceneInfo.Encounter.Status.ReadTabs.Contains(tabKey);
        }
        public void ReadTab(string tabKey)
        {
            if (!SceneInfo.Encounter.Status.ReadTabs.Contains(tabKey))
                SceneInfo.Encounter.Status.ReadTabs.Add(tabKey);
        }


        // combine user/loading screen and maybe encounter?
        public ReaderScene(EncounterSceneInfo sceneInfo, ReaderUI readerUI)
            : base(readerUI)
        {
            SceneInfo = sceneInfo;
            User = sceneInfo.User;
            ReaderUI = readerUI;
            EncounterData = sceneInfo.Encounter.Data;
            LoadingScreen = sceneInfo.LoadingScreen;
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
        protected virtual ReaderEncounterInfo CreateEncounterInfo() => new ReaderEncounterInfo(this, ReaderUI.EncounterInfo, SceneInfo.Encounter.Metadata);
        protected virtual ReaderSectionsGroup CreateSectionsGroup() => new ReaderSectionsGroup(this, ReaderUI.Sections, EncounterData.Content);
        protected virtual ReaderPinManager CreatePinManager() => new ReaderPinManager(this, ReaderUI.Pins);
        protected virtual ReaderPopupManager CreatePopupManager() => new ReaderPopupManager(this, ReaderUI.Popups);
        protected virtual ReaderFooter CreateFooter() => new ReaderFooter(this, ReaderUI.Footer, EncounterData.Content.Sections);


        protected virtual void AddListeners(ReaderUI readerUI)
        {
            foreach (var mainMenuButton in readerUI.MainMenuButtons)
                mainMenuButton.onClick.AddListener(ShowMainMenu);

            readerUI.Rating.SubmitRating += Rating_SubmitRating;
            readerUI.GameClosed += Quitting;
            readerUI.Rating.Rating = SceneInfo.Encounter.Status.BasicStatus.Rating;
        }

        private void Rating_SubmitRating(int rating)
        {
            if (rating < 1 || rating > 5)
                return;

            SceneInfo.Encounter.Status.BasicStatus.Rating = rating;

        }

        public void Help()
        {

        }
        
        public void Quitting()
        {
            SetCompleted();
            var serverDetailedStatusWriter = new ServerDetailedStatusWriter(new UrlBuilder(), new ServerReader());
            var fileDetailedStatusWriter = new FileDetailedStatusWriter(new UserFileManager(new FileExtensionManager()));
            var detailedStatusWriter = new DetailedStatusWriter(serverDetailedStatusWriter, fileDetailedStatusWriter);
            detailedStatusWriter.DoStuff(SceneInfo.User, SceneInfo.Encounter);
        }
        public void ShowMainMenu()
        {
            Quitting();
            //var menuSceneInfo = new LoadingMenuSceneInfo(user, loadingScreen, )
            //EncounterSceneManager.EncounterInstance.StartMainMenuScene(User);
        }

        protected void SetCompleted()
        {
            foreach (var section in SceneInfo.Encounter.Data.Content.Sections.Values) {
                foreach (var tab in section.Tabs) {
                    if (!SceneInfo.Encounter.Status.ReadTabs.Contains(tab.Key))
                        return;
                }
            }

            SceneInfo.Encounter.Status.BasicStatus.Completed = true;
        }
    }
}