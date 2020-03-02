using System.Collections;
using UnityEngine;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.Writer;
using ClinicalTools.SimEncounters.Data;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneLoader
    {
        protected virtual string ReaderScenePath => "";

        public ReaderSceneLoader(LoadingScreen loadingScreen)
        {

        }

        public void StartReaderScene(User user, IEncounterXml encounterXml)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ReaderScenePath);
            loading.completed += (asyncOperation) => InitializeReaderScene(user, encounterXml);
        }
        protected virtual void InitializeReaderScene(User user, IEncounterXml encounterXml)
        {
            //encounterXml.Completed += (dataXml, imagesXml) => EncounterXml_Completed(user, dataXml, imagesXml);
            encounterXml.GetEncounterXml(user, new EncounterInfoGroup());
        }

    }

    public class EncounterSceneManager : SceneManager
    {
        public static EncounterSceneManager EncounterInstance => (EncounterSceneManager)Instance;
        [field: SerializeField] public LoadingScreenUI LoadingScreenPrefab { get; set; }

        protected virtual string WriterScenePath => "";
        protected virtual string ReaderScenePath => "";
        protected virtual string MainMenuScenePath => "";

        public static void StartReader(User user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
            => EncounterInstance.StartReaderScene(user, loadingScreen, encounterXml);
        protected virtual void StartReaderScene(User user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ReaderScenePath);
            loading.completed += (asyncOperation) => InitializeReaderScene(user, loadingScreen, encounterXml);
        }
        protected virtual void InitializeReaderScene(User user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
        {
            //encounterXml.Completed += (dataXml, imagesXml) => EncounterXml_Completed(user, dataXml, imagesXml);
            encounterXml.GetEncounterXml(user, new EncounterInfoGroup());
        }

        public static void StartWriter(User user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
            => EncounterInstance.StartWriterScene(user, loadingScreen, encounterXml);
        protected virtual void StartWriterScene(User user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ReaderScenePath);
            loading.completed += (asyncOperation) => InitializeWriterScene(user, loadingScreen, encounterXml);
        }
        protected virtual void InitializeWriterScene(User user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
        {
            //encounterXml.Completed += (dataXml, imagesXml) => EncounterXml_Completed(user, dataXml, imagesXml);
            encounterXml.GetEncounterXml(user, new EncounterInfoGroup());
        }

        private void EncounterXml_Completed(User user, XmlDocument dataXml, XmlDocument imagesXml)
        {
            var loader = new EncounterLoader();
            var encounterInfo = new EncounterInfoGroup();
            var encounter = loader.ReadEncounter(encounterInfo, dataXml, imagesXml);

            new EncounterWriter(user, null, encounter, (WriterUI)SceneUI);
        }
    }
}