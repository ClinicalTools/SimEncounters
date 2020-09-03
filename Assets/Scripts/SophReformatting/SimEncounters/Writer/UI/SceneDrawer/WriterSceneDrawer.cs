using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterSceneDrawer : BaseLoadingWriterSceneDrawer
    {
        public BaseWriterSceneDrawer EncounterSaver { get => encounterSaver; set => encounterSaver = value; }
        [SerializeField] private BaseWriterSceneDrawer encounterSaver;
        public virtual List<Button> MainMenuButtons { get => mainMenuButtons; set => mainMenuButtons = value; }
        [SerializeField] private List<Button> mainMenuButtons;

        protected IMenuSceneStarter MenuSceneStarter { get; set; }
        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject]
        public virtual void Inject(
            IMenuSceneStarter menuSceneStarter, IMenuEncountersInfoReader menuInfoReader, BaseConfirmationPopup confirmationPopup)
        {
            MenuSceneStarter = menuSceneStarter;
            MenuInfoReader = menuInfoReader;
            ConfirmationPopup = confirmationPopup;
        }
        protected virtual void Awake()
        {
            foreach (var mainMenuButton in MainMenuButtons)
                mainMenuButton.onClick.AddListener(ConfirmReturnToMainMenu);
        }

        public override void Display(LoadingWriterSceneInfo sceneInfo)
            => sceneInfo.Result.AddOnCompletedListener(EncounterLoaded);

        protected WriterSceneInfo SceneInfo { get; set; }
        protected virtual void EncounterLoaded(TaskResult<WriterSceneInfo> sceneInfo)
        {
            SceneInfo = sceneInfo.Value;
            if (Started)
                ProcessSceneInfo(sceneInfo.Value);
        }

        protected bool Started { get; set; }
        protected virtual void Start()
        {
            Started = true;
            if (SceneInfo != null)
                ProcessSceneInfo(SceneInfo);
        }

        protected virtual void ProcessSceneInfo(WriterSceneInfo sceneInfo)
            => EncounterSaver.Display(sceneInfo);

        protected virtual void ConfirmReturnToMainMenu()
            => ConfirmationPopup.ShowConfirmation(ReturnToMainMenu, "CONFIRMATION", 
                "Are you sure you want to exit?\nAny unsaved changes will be lost");
        
        protected virtual void ReturnToMainMenu()
        {
            var categories = MenuInfoReader.GetMenuEncountersInfo(SceneInfo.User);
            var menuSceneInfo = new LoadingMenuSceneInfo(SceneInfo.User, SceneInfo.LoadingScreen, categories);

            MenuSceneStarter.StartScene(menuSceneInfo);
        }
    }
}