using ClinicalTools.SimEncounters.Writer;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SceneManager
    {
        public static SceneManager Instance { get; } = new SceneManager();
        protected virtual WriterManager WriterManager { get; }

        public virtual void StartReader()
        {

        }


        public virtual async Task StartReaderScene(GameObject loadingScreen)
        {
            var loadingScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2);
            loadingScene.completed += ((AsyncOperation obj) => SetReaderValues(obj));
        }

        public Task<WriterManager> GetTheData()
        {
            TaskCompletionSource<WriterManager> tcs = new TaskCompletionSource<WriterManager>();
            var worker = "";// new SomeObject();
            //worker.WorkCompleted += result => tcs.SetResult(result);
            //worker.DoWork();
            return tcs.Task;
        }

        protected virtual void SetReaderValues(AsyncOperation obj)
        {

        }
    }
}