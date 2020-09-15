using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderCompletableEncounterHandler : ReaderFullEncounterHandler, ICompletable, IReaderSceneDrawer
    {
        protected List<ICompletable> Completables { get; } = new List<ICompletable>();
        protected List<IReaderSceneDrawer> SceneDrawers { get; } = new List<IReaderSceneDrawer>();

        public event Action Completed;

        protected override void AddReaderObject(MonoBehaviour readerObject)
        {
            base.AddReaderObject(readerObject);

            if (readerObject is ICompletable completable)
                Completables.Add(completable);
            if (readerObject is IReaderSceneDrawer sceneDrawer)
                SceneDrawers.Add(sceneDrawer);
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            foreach (var completable in Completables)
                completable.Completed += OnCompleted;
        }

        protected virtual void OnCompleted() => Completed?.Invoke();

        public void Display(LoadingReaderSceneInfo sceneInfo)
        {
            foreach (var sceneDrawer in SceneDrawers)
                sceneDrawer.Display(sceneInfo);
        }
    }
}
