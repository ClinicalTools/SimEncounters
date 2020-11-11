using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderCompletableEncounterHandler : ReaderFullEncounterHandler, ICompletable, ICompletionDrawer, IReaderSceneDrawer
    {
        protected List<ICompletable> Completables { get; } = new List<ICompletable>();
        protected List<ICompletionDrawer> CompletionDrawers { get; } = new List<ICompletionDrawer>();
        protected List<IReaderSceneDrawer> SceneDrawers { get; } = new List<IReaderSceneDrawer>();

        public event Action Completed;

        protected override void Register(MonoBehaviour readerObject)
        {
            base.Register(readerObject);

            if (readerObject is ICompletionDrawer completionDrawer)
                CompletionDrawers.Add(completionDrawer);
            if (readerObject is IReaderSceneDrawer sceneDrawer)
                SceneDrawers.Add(sceneDrawer);
            if (readerObject is ICompletable completable) {
                Completables.Add(completable);
                completable.Completed += OnCompleted;
            }
        }

        protected override void Deregister(MonoBehaviour readerObject)
        {
            base.Deregister(readerObject);

            if (readerObject is ICompletable completable) {
                Completables.Remove(completable);
                completable.Completed -= Completed;
            }

            if (readerObject is ICompletionDrawer completionDrawer)
                CompletionDrawers.Remove(completionDrawer);
            if (readerObject is IReaderSceneDrawer sceneDrawer)
                SceneDrawers.Remove(sceneDrawer);
        }

        protected virtual void OnCompleted() => Completed?.Invoke();

        public virtual void Display(LoadingReaderSceneInfo sceneInfo)
        {
            foreach (var sceneDrawer in SceneDrawers)
                sceneDrawer.Display(sceneInfo);
        }

        public virtual void CompletionDraw(ReaderSceneInfo readerSceneInfo)
        {
            foreach (var completionDrawer in CompletionDrawers)
                completionDrawer.CompletionDraw(readerSceneInfo);
        }
    }
}
