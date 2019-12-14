using ClinicalTools.SimEncounters.EncounterReader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    /// <summary>
    /// Holds an instance of variables used between scenes.
    /// </summary>
    public class GlobalData 
    {
        public static GlobalData Instance;
        public virtual EncounterGetter EncounterGetter { get; protected set; } = new EncounterGetter();
        public virtual LoadingScreen LoadingScreen { get; } //= new LoadingScreen();

    }
}