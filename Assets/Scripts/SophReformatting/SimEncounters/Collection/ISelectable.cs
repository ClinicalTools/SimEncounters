using System;

namespace ClinicalTools.SimEncounters
{
    public interface ISelectable<T>
    {
        event Action<T> Selected;
        void Select();
    }
}