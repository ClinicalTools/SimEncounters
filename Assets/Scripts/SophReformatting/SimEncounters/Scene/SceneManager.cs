using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class SceneManager : MonoBehaviour
    {
        protected static bool FirstScene { get; set; } = true;
        public static SceneManager Instance { get; protected set; }

        public virtual void Awake()
        {
            Instance = this;

            if (FirstScene)
                StartAsInitialScene();
            else
                StartAsLaterScene();

            FirstScene = false;
        }

        protected abstract void StartAsInitialScene();
        protected abstract void StartAsLaterScene();
    }
}