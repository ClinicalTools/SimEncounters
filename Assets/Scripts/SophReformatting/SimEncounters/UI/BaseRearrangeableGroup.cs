using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class RearrangedEventArgs2 : EventArgs
    {
        public int OldIndex { get; }
        public int NewIndex { get; }
        public IDraggable MovedObject { get; }
        public List<IDraggable> CurrentOrder { get; }

        public RearrangedEventArgs2(int oldIndex, int newIndex, IDraggable movedObject, List<IDraggable> draggedObjects)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            MovedObject = movedObject;
            CurrentOrder = draggedObjects;
        }
    }
    public delegate void RearrangedEventHandler(object sender, RearrangedEventArgs2 e);


    public abstract class BaseRearrangeableGroup : MonoBehaviour
    {
        public abstract event RearrangedEventHandler Rearranged;
        public abstract T AddFromPrefab<T>(T draggablePrefab)
            where T : MonoBehaviour, IDraggable;
        public abstract void Add(IDraggable draggable);
        public abstract void Remove(IDraggable draggable);
        public abstract void Clear();
    }
}