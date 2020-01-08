using System.Collections;
using UnityEngine;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.Writer;


namespace ClinicalTools.SimEncounters
{
    public class EncounterSceneManager : SceneManager
    {
        public static EncounterSceneManager EncounterInstance => (EncounterSceneManager)Instance;
        [field: SerializeField] public LoadingScreenUI LoadingScreenPrefab { get; set; }

        protected virtual string WriterScenePath => "";
        protected virtual string ReaderScenePath => "";
        protected virtual string MainMenuScenePath => "";

        public static void StartWriter(object user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
            => EncounterInstance.StartWriterScene(user, loadingScreen, encounterXml);
        protected virtual void StartWriterScene(object user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ReaderScenePath);
            loading.completed += (asyncOperation) => StartCoroutine(InitializeWriterScene(user, loadingScreen, encounterXml));
        }

        protected virtual IEnumerator InitializeWriterScene(object user, LoadingScreen loadingScreen, IEncounterXml encounterXml)
        {
            while (!encounterXml.ImagesXml.IsCompleted || !encounterXml.DataXml.IsCompleted)
                yield return null;

            var loader = new EncounterLoader();
            var encounter = loader.ReadEncounter(encounterXml.DataXml.Result, encounterXml.ImagesXml.Result);

            new EncounterWriter(user, loadingScreen, encounter, (WriterUI)SceneUI);
        }
    }
}