using ClinicalTools.SimEncounters.Reader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneStarter : MonoBehaviour
    {
        protected IScenePathData ScenePathData { get; }

        public ReaderSceneStarter(IScenePathData scenePathData)
        {
            ScenePathData = scenePathData;
        }

        public virtual void StartScene(EncounterSceneManager sceneManager, InfoNeededForReaderToHappen data)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePathData.MainMenuPath);
            loading.completed += (asyncOperation) => InitializeScene(sceneManager, data);
        }

        protected virtual void InitializeScene(EncounterSceneManager sceneManager, InfoNeededForReaderToHappen data)
        {
            StartReader(sceneManager, data);
        }

        public virtual void StartReader(EncounterSceneManager sceneManager, InfoNeededForReaderToHappen data)
        {
            var readerUI = sceneManager.SceneUI as ReaderUI;
            if (readerUI == null) {
                Debug.LogError("Started scene UI is not Main Menu.");
                return;
            }

            //readerUI.Display(data);
        }
    }
}