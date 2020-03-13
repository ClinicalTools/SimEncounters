using System;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{

    public delegate bool Filter<in T>(T x);
    public interface IEncounterFilter
    {
        Filter<EncounterDetail> EncounterFilter { get; }

        event Action<Filter<EncounterDetail>> FilterChanged;
    }
}