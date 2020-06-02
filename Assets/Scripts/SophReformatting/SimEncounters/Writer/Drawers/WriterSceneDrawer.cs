using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.MainMenu;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSceneDrawer : BaseWriterSceneDrawer
    {
        public BaseWriterMetadataDisplay SavePopup { get => savePopup; set => savePopup = value; }
        [SerializeField] private BaseWriterMetadataDisplay savePopup;
        public Button SaveButton { get => saveButton; set => saveButton = value; }
        [SerializeField] private Button saveButton;
        public virtual List<Button> MainMenuButtons { get => mainMenuButtons; set => mainMenuButtons = value; }
        [SerializeField] private List<Button> mainMenuButtons;
        public virtual Button ReaderButton { get => readerButton; set => readerButton = value; }
        [SerializeField] private Button readerButton;
        public BaseEncounterDrawer EncounterDrawer { get => encounterDrawer; set => encounterDrawer = value; }
        [SerializeField] private BaseEncounterDrawer encounterDrawer;

        protected IReaderSceneStarter ReaderSceneStarter { get; set; }
        protected IMenuSceneStarter MenuSceneStarter { get; set; }
        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        [Inject]
        public virtual void Inject(
            IMenuSceneStarter menuSceneStarter, IMenuEncountersInfoReader menuInfoReader, IReaderSceneStarter sceneStarter)
        {
            ReaderSceneStarter = sceneStarter;
            MenuSceneStarter = menuSceneStarter;
            MenuInfoReader = menuInfoReader;
        }
        protected virtual void Awake()
        {
            SaveButton.onClick.AddListener(SaveEncounter);
            ReaderButton.onClick.AddListener(ShowInReader);
            foreach (var mainMenuButton in MainMenuButtons)
                mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }

        public override void Display(LoadingWriterSceneInfo sceneInfo)
        {
            sceneInfo.Result.AddOnCompletedListener(EncounterLoaded);
        }

        protected WriterSceneInfo SceneInfo { get; set; }
        protected virtual void EncounterLoaded(WriterSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;
            if (Started)
                ProcessSceneInfo(sceneInfo);
        }

        protected bool Started { get; set; }
        protected virtual void Start()
        {
            Started = true;
            if (SceneInfo != null)
                ProcessSceneInfo(SceneInfo);
        }

        protected virtual void ProcessSceneInfo(WriterSceneInfo sceneInfo)
        {
            SaveButton.interactable = true;
            EncounterDrawer.Display(sceneInfo.Encounter);
        }

        protected virtual void SaveEncounter() => SavePopup.Display(SceneInfo.User, SceneInfo.Encounter);
        protected virtual void ShowInReader() {
            if (SceneInfo == null)
                return;

            var encounter = new UserEncounter(SceneInfo.User, SceneInfo.Encounter.Metadata, SceneInfo.Encounter, new EncounterStatus(new EncounterBasicStatus(), new EncounterContentStatus()));
            var encounterResult = new WaitableResult<UserEncounter>(encounter);
            var loadingInfo = new LoadingReaderSceneInfo(SceneInfo.User, SceneInfo.LoadingScreen, encounterResult);
            ReaderSceneStarter.StartScene(loadingInfo);
        }
        protected virtual void ReturnToMainMenu()
        {
            var categories = MenuInfoReader.GetMenuEncountersInfo(SceneInfo.User);
            var menuSceneInfo = new LoadingMenuSceneInfo(SceneInfo.User, SceneInfo.LoadingScreen, categories);

            MenuSceneStarter.StartScene(menuSceneInfo);
        }
    }
}