using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public interface IMenuSceneStarter
    {
        void StartScene(LoadingMenuSceneInfo encounterSceneInfo);
    }
    public class MenuSceneStarter : IMenuSceneStarter
    {
        protected IScenePathData ScenePathData { get; }

        public MenuSceneStarter(IScenePathData scenePathData)
        {
            ScenePathData = scenePathData;
        }

        public virtual void StartScene(LoadingMenuSceneInfo data)
        {
            data.LoadingScreen?.Show();
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePathData.MainMenuPath);
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
