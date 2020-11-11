using UnityEngine;

namespace ClinicalTools.UI
{
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