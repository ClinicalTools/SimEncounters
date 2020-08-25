﻿using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class MenuSceneStarter : IMenuSceneStarter
    {
        protected string ScenePath { get; }
        public MenuSceneStarter(string scenePath) => ScenePath = scenePath;

        public virtual void StartScene(LoadingMenuSceneInfo data)
        {
            data.LoadingScreen?.Show();
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePath);
            loading.completed += (asyncOperation) => StartMainMenu(data);
        }

        public virtual void StartMainMenu(LoadingMenuSceneInfo data)
        {
            if (!(SceneManager.Instance is IMenuSceneDrawer menuScene)) {
                Debug.LogError("Started scene UI is not Menu scene.");
                return;
            }

            menuScene.Display(data);
        }
    }
}
