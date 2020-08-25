using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneStarter : IReaderSceneStarter
    {
        protected string ScenePath { get; }
        public ReaderSceneStarter(string scenePath) => ScenePath = scenePath;

        public virtual void StartScene(LoadingReaderSceneInfo data)
        {
            data.LoadingScreen?.Show();
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePath);
            loading.completed += (asyncOperation) => InitializeScene(data);
        }

        protected virtual void InitializeScene(LoadingReaderSceneInfo data)
        {
            if (!(SceneManager.Instance is IReaderSceneDrawer readerScene)) {
                Debug.LogError("Started scene UI is not Reader.");
                return;
            }

            readerScene.Display(data);
        }
    }
}