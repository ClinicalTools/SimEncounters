using System.Collections;
using UnityEngine;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.Writer;
using ClinicalTools.SimEncounters.Data;
using System.Xml;
using ClinicalTools.SimEncounters.Reader;

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
        protected virtual string ReaderScenePath => "MobileCassReader";
        protected virtual string MainMenuScenePath => "";

        public static void StartReader(User user, IEncounterGetter encounterGetter)
            => EncounterInstance.StartReaderScene(user, encounterGetter);
        protected virtual void StartReaderScene(User user, IEncounterGetter encounterGetter)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ReaderScenePath);
            loading.completed += (asyncOperation) => InitializeReaderScene(user, encounterGetter);
        }

        protected virtual void InitializeReaderScene(User user, IEncounterGetter encounterGetter)
        {
            if (encounterGetter.IsDone)
                StartReader(user, encounterGetter.Encounter);
            else
                encounterGetter.Completed += (encounter) => StartReader(user, encounter);
        }

        protected virtual void StartReader(User user, Encounter encounter)
        {
            new ReaderScene(User.Guest, null, encounter, (ReaderUI)SceneUI);
        }


    }
}