using ClinicalTools.SimEncounters.Loader;
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
        public virtual LoadingScreen LoadingScreen { get; } //= new LoadingScreen();
    }
}