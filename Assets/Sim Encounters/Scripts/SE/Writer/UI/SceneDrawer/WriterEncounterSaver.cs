using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterSaver : BaseWriterSceneDrawer
    {
        public BaseSaveEncounterDisplay SavePopup { get => savePopup; set => savePopup = value; }
        [SerializeField] private BaseSaveEncounterDisplay savePopup;
        public Button SaveButton { get => saveButton; set => saveButton = value; }
        [SerializeField] private Button saveButton;
        public virtual Button ReaderButton { get => readerButton; set => readerButton = value; }
        [SerializeField] private Button readerButton;

        protected virtual void Awake()
        {
            SaveButton.onClick.AddListener(SaveEncounter);
            ReaderButton.onClick.AddListener(ShowInReader);
            StartCoroutine(AutosaveCoroutine());
        }

        protected ISelector<EncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected IReaderSceneStarter ReaderSceneStarter { get; set; }
        protected IEncounterWriter EncounterWriter { get; set; }
        protected IEncounterWriter AutosaveWriter { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<EncounterSelectedEventArgs> encounterSelector,
            IReaderSceneStarter sceneStarter,
            [Inject(Id = SaveType.Local)] IEncounterWriter encounterWriter,
            [Inject(Id = SaveType.Autosave)] IEncounterWriter autosaveWriter)
        {
            EncounterSelector = encounterSelector;
            ReaderSceneStarter = sceneStarter;
            EncounterWriter = encounterWriter;
            AutosaveWriter = autosaveWriter;
        }

        protected WriterSceneInfo SceneInfo { get; set; }
        public override void Display(WriterSceneInfo sceneInfo)
        {
            ReaderButton.interactable = true;
            SaveButton.interactable = true;
            SceneInfo = sceneInfo;
            EncounterSelector.Select(this, new EncounterSelectedEventArgs(sceneInfo.Encounter));
        }
        protected virtual void SaveEncounter()
        {
            if (SceneInfo == null)
                return;

            SavePopup.Display(SceneInfo.User, SceneInfo.Encounter);
        }

        private const float AUTOSAVE_INTERVAL_SECONDS = 3 * 60; // In seconds
        protected virtual IEnumerator AutosaveCoroutine()
        {
            yield return new WaitForSeconds(AUTOSAVE_INTERVAL_SECONDS);
            AutosaveEncounter();

            yield return AutosaveCoroutine();
        }

        protected virtual void AutosaveEncounter()
        {
            if (SceneInfo == null)
                return;

            AutosaveWriter.Save(SceneInfo.User, SceneInfo.Encounter);
        }

        protected virtual void ShowInReader()
        {
            if (SceneInfo == null)
                return;

            EncounterWriter.Save(SceneInfo.User, SceneInfo.Encounter);

            var encounter = new UserEncounter(SceneInfo.User, SceneInfo.Encounter, new EncounterStatus(new EncounterBasicStatus(), new EncounterContentStatus()));
            var encounterResult = new WaitableTask<UserEncounter>(encounter);
            var loadingInfo = new LoadingReaderSceneInfo(SceneInfo.User, SceneInfo.LoadingScreen, encounterResult);
            ReaderSceneStarter.StartScene(loadingInfo);
        }
    }

    [RequireComponent(typeof(Button))]
    public class WriterSaveAndViewInReaderButton : MonoBehaviour
    {
        public virtual Button ReaderButton { get => readerButton; set => readerButton = value; }
        [SerializeField] private Button readerButton;

        protected SignalBus SignalBus { get; set; }
        protected ISelector<WriterSceneInfoSelectedEventArgs> SceneInfoSelector { get; set; }
        protected IReaderSceneStarter ReaderSceneStarter { get; set; }
        protected IEncounterWriter EncounterWriter { get; set; }
        [Inject]
        public virtual void Inject(
            SignalBus signalBus,
            ISelector<WriterSceneInfoSelectedEventArgs> sceneInfoSelector,
            IReaderSceneStarter sceneStarter,
            [Inject(Id = SaveType.Local)] IEncounterWriter encounterWriter)
        {
            SignalBus = signalBus;
            SceneInfoSelector = sceneInfoSelector;
            ReaderSceneStarter = sceneStarter;
            EncounterWriter = encounterWriter;
        }

        protected virtual Button Button { get; set; }
        protected virtual void Start()
        {
            Button = GetComponent<Button>();
            Button.interactable = false;
            Button.onClick.AddListener(ShowInReader);

            SceneInfoSelector.Selected += SceneLoaded;
            if (SceneInfoSelector.CurrentValue == null)
                SceneLoaded(this, SceneInfoSelector.CurrentValue);
        }

        private void SceneLoaded(object sender, WriterSceneInfoSelectedEventArgs e)
        {
            Button.interactable = true;
            Button.onClick.AddListener(ShowInReader);
        }

        protected virtual void ShowInReader()
        {
            var sceneInfo = SceneInfoSelector.CurrentValue.SceneInfo;
            EncounterWriter.Save(sceneInfo.User, sceneInfo.Encounter);

            var encounter = new UserEncounter(sceneInfo.User, sceneInfo.Encounter, new EncounterStatus(new EncounterBasicStatus(), new EncounterContentStatus()));
            var encounterResult = new WaitableTask<UserEncounter>(encounter);
            var loadingInfo = new LoadingReaderSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounterResult);
            ReaderSceneStarter.StartScene(loadingInfo);
        }
    }
}