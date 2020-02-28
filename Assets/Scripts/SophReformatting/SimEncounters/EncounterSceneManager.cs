using System.Collections;
using UnityEngine;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.Writer;
using ClinicalTools.SimEncounters.Data;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class EncounterSceneManager : SceneManager
    {
        public static EncounterSceneManager EncounterInstance => (EncounterSceneManager)Instance;
        [field: SerializeField] public LoadingScreenUI LoadingScreenPrefab { get; set; }

        protected virtual string WriterScenePath => "";
        protected virtual string ReaderScenePath => "";
        protected virtual string MainMenuScenePath => "";

        public static void StartWriter(User user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
            => EncounterInstance.StartWriterScene(user, loadingScreen, encounterXml);
        protected virtual void StartWriterScene(User user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ReaderScenePath);
            loading.completed += (asyncOperation) => InitializeWriterScene(user, loadingScreen, encounterXml);
        }

        private void EncounterXml_Completed(User user, XmlDocument dataXml, XmlDocument imagesXml)
        {
            var loader = new EncounterLoader();
            var encounterInfo = new EncounterInfo();
            var encounter = loader.ReadEncounter(encounterInfo, dataXml, imagesXml);

            new EncounterWriter(user, null, encounter, (WriterUI)SceneUI);
        }

        protected virtual void InitializeWriterScene(User user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
        {
            encounterXml.Completed += (dataXml, imagesXml) => EncounterXml_Completed(user, dataXml, imagesXml);
            encounterXml.GetEncounterXml(user, new EncounterInfoGroup());
        }
    }
}