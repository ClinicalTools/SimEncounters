using ClinicalTools.SimEncounters.MainMenu;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuSceneLoader
    {
        protected IScenePathData ScenePathData { get; }

        public MainMenuSceneLoader(IScenePathData scenePathData)
        {
            ScenePathData = scenePathData;
        }

        public virtual void StartScene(EncounterSceneManager sceneManager, User user)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePathData.MainMenuPath);
            loading.completed += (asyncOperation) => InitializeScene(sceneManager, user);
        }

        protected virtual void InitializeScene(EncounterSceneManager sceneManager, User user)
        {
            StartMainMenu(sceneManager, user);
        }

        public virtual void StartMainMenu(EncounterSceneManager sceneManager, User user)
        {
            new MainMenuScene(user, (MainMenuUI)sceneManager.SceneUI);
        }
    }
}