using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseRearrangeableGroup : MonoBehaviour
    {
        public abstract event Action<List<IDraggable>> OrderChanged;
        public abstract T AddFromPrefab<T>(T draggablePrefab)
            where T : MonoBehaviour, IDraggable;
        public abstract void Add(IDraggable draggable);
        public abstract void Remove(IDraggable draggable);
        public abstract void Clear();
    }
}