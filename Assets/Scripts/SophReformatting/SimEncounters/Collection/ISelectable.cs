using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{

    public abstract class SelectableBehaviour2 : MonoBehaviour
    {
        public abstract event Action Selected;
        public abstract event Action Unselected;
        public abstract void Select();
    }

    public class SelectableBehaviour2<T> : SelectableBehaviour2, INewSelectable<T>
    {
        public override event Action Selected;
        public override event Action Unselected;
        public override void Select() { }
    }
    public abstract class SelectableBehaviour<T> : MonoBehaviour, INewSelectable<T>
    {
        public abstract event Action Selected;
        public abstract event Action Unselected;
        public abstract void Select();
    }

    public interface ISelectable<T>
    {
        event Action<T> Selected;
        void Select();
    }
    public interface INewSelectable<T>
    {
        event Action Selected;
        event Action Unselected;
        void Select();
    }
}