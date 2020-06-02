using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.MainMenu;
using ClinicalTools.SimEncounters.Writer;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSceneDrawer : BaseReaderSceneDrawer
    {
        public virtual List<Button> MainMenuButtons { get => mainMenuButtons; set => mainMenuButtons = value; }
        [SerializeField] private List<Button> mainMenuButtons;
        public virtual Button WriterButton { get => writerButton; set => writerButton = value; }
        [SerializeField] private Button writerButton;
        public BaseUserEncounterDrawer EncounterDrawer { get => encounterDrawer; set => encounterDrawer = value; }
        [SerializeField] private BaseUserEncounterDrawer encounterDrawer;

        public event Action GameClosed;

        protected LoadingReaderSceneInfo LoadingSceneInfo { get; set; }

        protected IWriterSceneStarter WriterSceneStarter { get; set; }
        protected IMenuSceneStarter MenuSceneStarter { get; set; }
        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        protected IDetailedStatusWriter StatusWriter { get; set; }
        [Inject]
        public virtual void Inject(
            IMenuSceneStarter menuSceneStarter, IMenuEncountersInfoReader menuInfoReader, IWriterSceneStarter writerSceneStarter, IDetailedStatusWriter statusWriter)
        {
            WriterSceneStarter = writerSceneStarter;
            MenuSceneStarter = menuSceneStarter;
            MenuInfoReader = menuInfoReader;
            StatusWriter = statusWriter;
        }

        private bool started = false;
        protected virtual void Start()
        {
            started = true;
            if (userEncounter != null)
                EncounterDrawer.Display(userEncounter);

            foreach (var mainMenuButton in MainMenuButtons)
                mainMenuButton.onClick.AddListener(ReturnToMainMenu);

            if (WriterButton != null)
                WriterButton.onClick.AddListener(OpenWriter);
        }

        public override void Display(LoadingReaderSceneInfo loadingSceneInfo)
        {
            LoadingSceneInfo = loadingSceneInfo;
            loadingSceneInfo.Result.AddOnCompletedListener(EncounterLoaded);
        }


        private UserEncounter userEncounter;
        protected virtual void EncounterLoaded(ReaderSceneInfo sceneInfo)
        {
            userEncounter = sceneInfo.Encounter;
            if (started)
                EncounterDrawer.Display(userEncounter);
        }

        protected virtual void ReturnToMainMenu()
        {
            var categories = MenuInfoReader.GetMenuEncountersInfo(LoadingSceneInfo.User);
            var menuSceneInfo = new LoadingMenuSceneInfo(LoadingSceneInfo.User, LoadingSceneInfo.LoadingScreen, categories);

            StatusWriter.WriteStatus(userEncounter);

            MenuSceneStarter.StartScene(menuSceneInfo);
        }
        protected virtual void OpenWriter()
        {
            var encounter = new WaitableResult<Encounter>(userEncounter.Data);
            var writerSceneInfo = new LoadingWriterSceneInfo(LoadingSceneInfo.User, LoadingSceneInfo.LoadingScreen, encounter);
            WriterSceneStarter.StartScene(writerSceneInfo);
        }

        protected void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                GameClosed?.Invoke();
        }
        protected void OnApplicationQuit()
        {
            GameClosed?.Invoke();
        }
    }
}