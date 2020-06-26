using ClinicalTools.SimEncounters.Reader;
using ClinicalTools.SimEncounters.Writer;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IReaderSceneStarter
    {
        void StartScene(LoadingReaderSceneInfo encounterSceneInfo);
    }
    public interface IWriterSceneStarter
    {
        void StartScene(LoadingWriterSceneInfo encounterSceneInfo);
    }

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