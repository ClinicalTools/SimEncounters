using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseCompletionPopup : MonoBehaviour
    {
        public abstract event Action ExitScene;

        public abstract void Display(Encounter encounter);
    }
}