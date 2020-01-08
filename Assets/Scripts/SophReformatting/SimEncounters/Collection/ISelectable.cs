using System;

namespace ClinicalTools.SimEncounters.Writer
{
    public interface ISelectable<T>
    {
        event Action<T> Selected;
    }
}