using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderCompletableEncounterHandler : ReaderFullEncounterHandler, ICompletable
    {
        protected List<ICompletable> Completables { get; } = new List<ICompletable>();

        public event Action Completed;

        protected override void AddReaderObject(MonoBehaviour readerObject)
        {
            base.AddReaderObject(readerObject);

            if (readerObject is ICompletable completable)
                Completables.Add(completable);
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            foreach (var completable in Completables)
                completable.Completed += OnCompleted;
        }

        protected virtual void OnCompleted() => Completed?.Invoke();
    }
}
