using ClinicalTools.SimEncounters.Reader;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IReaderSceneStarter {
        void StartScene(LoadingReaderSceneInfo encounterSceneInfo);
    }

    public class ReaderSceneStarter : IReaderSceneStarter
    {
        protected IScenePathData ScenePathData { get; }

        public ReaderSceneStarter(IScenePathData scenePathData)
        {
            ScenePathData = scenePathData;
        }

        public virtual void StartScene(LoadingReaderSceneInfo data)
        {
            data.LoadingScreen?.Show();
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePathData.ReaderPath);
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