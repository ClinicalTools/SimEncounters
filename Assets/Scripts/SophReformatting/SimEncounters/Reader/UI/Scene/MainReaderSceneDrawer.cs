using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{

    public class MainReaderSceneDrawer : BaseReaderSceneDrawer
    {
        public List<MonoBehaviour> ReaderObjects { get => readerObjects; }
        [SerializeField] private List<MonoBehaviour> readerObjects = new List<MonoBehaviour>();

        protected List<IReaderSceneDrawer> SceneDrawers { get; } = new List<IReaderSceneDrawer>();
        protected List<ICompletable> Completables { get; } = new List<ICompletable>();
        protected List<ICompletionDrawer> CompleteDrawers { get; } = new List<ICompletionDrawer>();

        protected ReaderEncounterDrawManger EncounterDrawManger { get; } = new ReaderEncounterDrawManger();
        protected IStatusWriter StatusWriter { get; set; }
        [Inject] public virtual void Inject(IStatusWriter statusWriter) => StatusWriter = statusWriter;



        protected virtual void Awake()
        {
            foreach (var readerObject in ReaderObjects)
                AddReaderObject(readerObject);

            AddListeners();
        }

        protected virtual void AddReaderObject(MonoBehaviour readerObject)
        {
            if (readerObject is IReaderSceneDrawer sceneDrawer)
                SceneDrawers.Add(sceneDrawer);

            if (readerObject is ICompletable completable)
                Completables.Add(completable);
            if (readerObject is ICompletionDrawer completeDrawer)
                CompleteDrawers.Add(completeDrawer);
        }

        protected virtual void AddListeners()
        {
            foreach (var completable in Completables)
                completable.Completed += CompletedEncounter;
        }


        protected LoadingReaderSceneInfo LoadingSceneInfo { get; set; }
        public override void Display(LoadingReaderSceneInfo loadingSceneInfo)
        {
            LoadingSceneInfo = loadingSceneInfo;

            foreach (var sceneDrawer in SceneDrawers)
                sceneDrawer.Display(loadingSceneInfo);

            loadingSceneInfo.Result.AddOnCompletedListener(OnSceneLoaded);
        }

        protected ReaderSceneInfo SceneInfo { get; set; }
        protected virtual void OnSceneLoaded(TaskResult<ReaderSceneInfo> sceneInfo)
        {
            if (!sceneInfo.HasValue())
                return;

            SceneInfo = sceneInfo.Value;
            if (started)
                StartEncounter();
        }

        private bool started;
        protected virtual void Start()
        {
            started = true;
            if (SceneInfo != null)
                StartEncounter();
        }


        protected virtual void StartEncounter()
        {
            if (SceneInfo.LoadingScreen != null)
                SceneInfo.LoadingScreen.Stop();
            EncounterDrawManger.DrawEncounter(ReaderObjects, SceneInfo.Encounter);
        }

        protected virtual void CompletedEncounter()
        {
            foreach (var completeDrawer in CompleteDrawers)
                completeDrawer.CompletionDraw(SceneInfo);
        }

        protected virtual void OnDestroy()
        {
            if (SceneInfo != null)
                StatusWriter.WriteStatus(SceneInfo.Encounter);
        }
        protected void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveStatus();
        }

        protected virtual void SaveStatus()
        {
            if (SceneInfo?.Encounter == null)
                return;

            var status = SceneInfo.Encounter.Status;
            status.BasicStatus.Completed = status.ContentStatus.Read;
            StatusWriter.WriteStatus(SceneInfo.Encounter);
        }
    }
}