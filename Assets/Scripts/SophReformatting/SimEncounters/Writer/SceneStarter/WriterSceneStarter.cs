using ClinicalTools.SimEncounters.Writer;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class WriterSceneStarter : IWriterSceneStarter
    {
        protected string ScenePath { get; }
        public WriterSceneStarter(string scenePath) => ScenePath = scenePath;

        public virtual void StartScene(LoadingWriterSceneInfo data)
        {
            data.LoadingScreen?.Show();
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePath);
            loading.completed += (asyncOperation) => InitializeScene(data);
        }

        protected virtual void InitializeScene(LoadingWriterSceneInfo data)
        {
            if (!(SceneManager.Instance is ILoadingWriterSceneDrawer writerScene)) {
                Debug.LogError("Started scene UI is not Reader.");
                return;
            }

            writerScene.Display(data);
        }
    }
}