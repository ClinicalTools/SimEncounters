using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// As much as I'd love to escape singletons, there isn't a great way to set variables in the editor and still pass classes between scenes.
    /// </remarks>
    public class LoadingScreen : ILoadingScreen
    {
        public static LoadingScreen Instance { get; protected set; }

        protected virtual void Awake()
        {
            Instance = this;
        }

        public virtual void Show()
        {

        }
        public virtual void Stop()
        {
            Instance = null;
        }
    }
}