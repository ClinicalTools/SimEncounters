using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class MainReaderSceneDrawer : BaseReaderSceneDrawer
    {
        public List<MonoBehaviour> ReaderObjects { get => readerObjects; }
        [SerializeField] private List<MonoBehaviour> readerObjects = new List<MonoBehaviour>();

        protected List<IReaderSceneDrawer> SceneDrawers { get; } = new List<IReaderSceneDrawer>();
        protected List<ICompletable> Completables { get; } = new List<ICompletable>();
        protected List<ICompletionDrawer> CompleteDrawers { get; } = new List<ICompletionDrawer>();

        protected List<IMainMenuStarter> MainMenuStarters { get; } = new List<IMainMenuStarter>();

        protected ReaderEncounterDrawManger EncounterDrawManger { get; } = new ReaderEncounterDrawManger();


        protected virtual void Awake()
        {
            foreach (var readerObject in ReaderObjects)
                AddReaderObject(readerObject);
        }

        protected virtual void AddReaderObject(MonoBehaviour readerObject)
        {
            if (readerObject is IReaderSceneDrawer sceneDrawer)
                SceneDrawers.Add(sceneDrawer);

            if (readerObject is ICompletable completable)
                Completables.Add(completable);
            if (readerObject is ICompletionDrawer completeDrawer)
                CompleteDrawers.Add(completeDrawer);

            if (readerObject is IMainMenuStarter mainMenuStarter)
                MainMenuStarters.Add(mainMenuStarter);
        }

        protected virtual void AddListeners()
        {
            foreach (var mainMenuStarter in MainMenuStarters)
                mainMenuStarter.StartMainMenu += StartMainMenu;

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

            EncounterDrawManger.DrawEncounter(ReaderObjects, sceneInfo.Value.Encounter);
        }

        protected virtual void StartMainMenu()
        {

        }

        protected virtual void CompletedEncounter()
        {
            foreach (var completeDrawer in CompleteDrawers)
                completeDrawer.CompletionDraw(SceneInfo?.Encounter.Data);
        }
    }
}