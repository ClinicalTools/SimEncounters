using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{

    public delegate bool Filter<in T>(T x);
    public abstract class EncounterFilterBehaviour : MonoBehaviour
    {
        public abstract Filter<EncounterInfo> EncounterFilter { get; }
        
        public abstract event Action<Filter<EncounterInfo>> FilterChanged;

        public abstract void Clear();
    }
}