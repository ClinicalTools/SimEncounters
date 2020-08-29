﻿#if !STANDALONE_SCENE
#endif

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneDrawer : BaseReaderSceneDrawer
    {
        public virtual List<Button> MainMenuButtons { get => mainMenuButtons; set => mainMenuButtons = value; }
        [SerializeField] private List<Button> mainMenuButtons;
        public virtual Button ExitButton { get => exitButton; set => exitButton = value; }
        [SerializeField] private Button exitButton;
        public BaseUserEncounterDrawer EncounterDrawer { get => encounterDrawer; set => encounterDrawer = value; }
        [SerializeField] private BaseUserEncounterDrawer encounterDrawer;
        public BaseCompletionPopup CompletionPopup { get => completionPopup; set => completionPopup = value; }
        [SerializeField] private BaseCompletionPopup completionPopup;
        public BaseCompletionPopup WebGLCompletionPopup { get => webGLCompletionPopup; set => webGLCompletionPopup = value; }
        [SerializeField] private BaseCompletionPopup webGLCompletionPopup;

        public bool StandaloneScene { get; set; }

        protected LoadingReaderSceneInfo LoadingSceneInfo { get; set; }

#if !STANDALONE_SCENE
        protected IWriterSceneStarter WriterSceneStarter { get; set; }
        protected IMenuSceneStarter MenuSceneStarter { get; set; }
        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
#endif
        protected IDetailedStatusWriter StatusWriter { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        protected AndroidBackButton BackButton { get; set; }

        [Inject]
        public virtual void Inject(
#if !STANDALONE_SCENE
            IMenuSceneStarter menuSceneStarter,
            IMenuEncountersInfoReader menuInfoReader,
            IWriterSceneStarter writerSceneStarter,
#endif
            IDetailedStatusWriter statusWriter,
            BaseConfirmationPopup confirmationPopup,
            AndroidBackButton backButton)
        {
#if !STANDALONE_SCENE
            WriterSceneStarter = writerSceneStarter;
            MenuSceneStarter = menuSceneStarter;
            MenuInfoReader = menuInfoReader;
#endif
            StatusWriter = statusWriter;
            ConfirmationPopup = confirmationPopup;
            BackButton = backButton;
        }

        protected virtual void Awake()
        {
            if (EncounterDrawer is ICompletable completable) {
#if STANDALONE_SCENE
                completable.Finish += () => WebGLCompletionPopup.Display(userEncounter.Data);
#else
                completable.Finish += () => CompletionPopup.Display(userEncounter.Data);
#endif
            }
            CompletionPopup.ExitScene += ExitScene;
        }

        private bool started = false;
        protected virtual void Start()
        {
            started = true;
            if (userEncounter != null)
                EncounterDrawer.Display(userEncounter);

            foreach (var mainMenuButton in MainMenuButtons)
                mainMenuButton.onClick.AddListener(ConfirmExitScene);
            if (BackButton != null)
                BackButton.Register(ConfirmExitScene);
#if !STANDALONE_SCENE
            if (ExitButton != null)
                ExitButton.onClick.AddListener(OpenWriter);
#endif
        }

        public override void Display(LoadingReaderSceneInfo loadingSceneInfo)
        {
            LoadingSceneInfo = loadingSceneInfo;
            loadingSceneInfo.Result.AddOnCompletedListener(EncounterLoaded);
        }

        private UserEncounter userEncounter;
        protected virtual void EncounterLoaded(WaitedResult<ReaderSceneInfo> sceneInfo)
        {
            sceneInfo.Value.LoadingScreen?.Stop();
            userEncounter = sceneInfo.Value.Encounter;
            if (started)
                EncounterDrawer.Display(sceneInfo.Value.Encounter);
        }

#if !STANDALONE_SCENE
        private const string EXIT_CONFIRMATION_TITLE = "RETURN TO MAIN MENU";
#else
        private const string EXIT_CONFIRMATION_TITLE = "EXIT APPLICATION";
#endif
        protected virtual void ConfirmExitScene()
            => ConfirmationPopup.ShowConfirmation(ExitScene, CancelExitScene,
                EXIT_CONFIRMATION_TITLE, "Are you sure you want to exit?");

        protected virtual void ExitScene()
        {
            SaveStatus();

#if !STANDALONE_SCENE
            var categories = MenuInfoReader.GetMenuEncountersInfo(LoadingSceneInfo.User);
            var menuSceneInfo = new LoadingMenuSceneInfo(LoadingSceneInfo.User, LoadingSceneInfo.LoadingScreen, categories);
            MenuSceneStarter.StartScene(menuSceneInfo);
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        protected virtual void CancelExitScene()
        {
            if (BackButton != null)
                BackButton.Register(ConfirmExitScene);
        }

#if !STANDALONE_SCENE
        protected virtual void OpenWriter()
        {
            var encounter = new WaitableResult<Encounter>(userEncounter.Data);
            var writerSceneInfo = new LoadingWriterSceneInfo(LoadingSceneInfo.User, LoadingSceneInfo.LoadingScreen, encounter);
            WriterSceneStarter.StartScene(writerSceneInfo);
        }
#endif

        protected void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveStatus();
        }
        protected void OnApplicationQuit() => SaveStatus();

        protected virtual void SaveStatus()
        {
            var status = userEncounter.Status;
            status.BasicStatus.Completed = status.ContentStatus.Read;
            StatusWriter.WriteStatus(userEncounter);
        }
    }
}