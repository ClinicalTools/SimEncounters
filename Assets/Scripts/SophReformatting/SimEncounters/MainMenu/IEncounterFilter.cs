using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{

    public delegate bool Filter<in T>(T x);
    public abstract class EncounterFilterBehaviour : MonoBehaviour
    {
        public abstract Filter<EncounterDetail> EncounterFilter { get; }
        
        public abstract event Action<Filter<EncounterDetail>> FilterChanged;

        public abstract void Clear();
    }
}