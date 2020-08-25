using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseCompletionPopup : MonoBehaviour
    {
        public abstract event Action ExitScene;

        public abstract void Display(Encounter encounter);
    }
}