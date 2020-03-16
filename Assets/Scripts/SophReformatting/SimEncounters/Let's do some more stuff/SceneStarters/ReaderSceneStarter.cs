using ClinicalTools.SimEncounters.Reader;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneStarter
    {
        protected IScenePathData ScenePathData { get; }

        public ReaderSceneStarter(IScenePathData scenePathData)
        {
            ScenePathData = scenePathData;
        }

        public virtual void StartScene(EncounterSceneManager sceneManager, InfoNeededForReaderToHappen data)
        {
            data.LoadingScreen?.Show();
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePathData.ReaderPath);
            loading.completed += (asyncOperation) => InitializeScene(sceneManager, data);
        }

        protected virtual void InitializeScene(EncounterSceneManager sceneManager, InfoNeededForReaderToHappen data)
        {

            if (data.IsDone)
                StartReader(sceneManager, data);
            else
                data.EncounterLoaded += (encounter) => StartReader(sceneManager, data);
        }

        public virtual void StartReader(EncounterSceneManager sceneManager, InfoNeededForReaderToHappen data)
        {
            var readerUI = sceneManager.SceneUI as ReaderUI;
            if (readerUI == null) {
                Debug.LogError("Started scene UI is not Reader.");
                return;
            }

            new ReaderScene(data, (ReaderUI)sceneManager.SceneUI);
            //readerUI.Display(data);
        }
    }
}