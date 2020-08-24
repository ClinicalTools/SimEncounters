
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterEncounterSaver : BaseWriterSceneDrawer
    {
        public BaseWriterDisplay SavePopup { get => savePopup; set => savePopup = value; }
        [SerializeField] private BaseWriterDisplay savePopup;
        public Button SaveButton { get => saveButton; set => saveButton = value; }
        [SerializeField] private Button saveButton;
        public virtual Button ReaderButton { get => readerButton; set => readerButton = value; }
        [SerializeField] private Button readerButton;
        public BaseSerializableEncounterDrawer EncounterDrawer { get => encounterDrawer; set => encounterDrawer = value; }
        [SerializeField] private BaseSerializableEncounterDrawer encounterDrawer;

        protected virtual void Awake()
        {
            SaveButton.onClick.AddListener(SaveEncounter);
            ReaderButton.onClick.AddListener(ShowInReader);
            StartCoroutine(AutosaveCoroutine());
        }

        protected IReaderSceneStarter ReaderSceneStarter { get; set; }
        protected IEncounterWriter EncounterWriter { get; set; }
        protected IEncounterWriter AutosaveWriter { get; set; }
        [Inject]
        public virtual void Inject(IReaderSceneStarter sceneStarter,
            [Inject(Id = SaveType.Local)] IEncounterWriter encounterWriter,
            [Inject(Id = SaveType.Autosave)] IEncounterWriter autosaveWriter)
        {
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
            EncounterDrawer.Display(sceneInfo.Encounter);
        }
        protected virtual void SaveEncounter()
        {
            if (SceneInfo == null)
                return;

            EncounterDrawer.Serialize();
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

            EncounterDrawer.Serialize();
            AutosaveWriter.Save(SceneInfo.User, SceneInfo.Encounter);
        }

        protected virtual void ShowInReader()
        {
            if (SceneInfo == null)
                return;

            EncounterDrawer.Serialize();
            EncounterWriter.Save(SceneInfo.User, SceneInfo.Encounter);

            var encounter = new UserEncounter(SceneInfo.User, SceneInfo.Encounter, new EncounterStatus(new EncounterBasicStatus(), new EncounterContentStatus()));
            var encounterResult = new WaitableResult<UserEncounter>(encounter);
            var loadingInfo = new LoadingReaderSceneInfo(SceneInfo.User, SceneInfo.LoadingScreen, encounterResult);
            ReaderSceneStarter.StartScene(loadingInfo);
        }
    }
}