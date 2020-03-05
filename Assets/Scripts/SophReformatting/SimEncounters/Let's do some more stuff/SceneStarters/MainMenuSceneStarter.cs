using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuSceneStarter
    {
        protected IScenePathData ScenePathData { get; }

        public MainMenuSceneStarter(IScenePathData scenePathData)
        {
            ScenePathData = scenePathData;
        }

        public virtual void StartScene(EncounterSceneManager sceneManager, InfoNeededForMainMenuTohappen data)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePathData.MainMenuPath);
            loading.completed += (asyncOperation) => InitializeScene(sceneManager, data);
        }

        protected virtual void InitializeScene(EncounterSceneManager sceneManager, InfoNeededForMainMenuTohappen data)
        {
            StartMainMenu(sceneManager, data);
        }

        public virtual void StartMainMenu(EncounterSceneManager sceneManager, InfoNeededForMainMenuTohappen data)
        {
            var mainMenuUI = sceneManager.SceneUI as MainMenuUI;
            if (mainMenuUI == null)
            {
                Debug.LogError("Started scene UI is not Main Menu.");
                return;

            }

            mainMenuUI.Display(data);
        }
    }
}
