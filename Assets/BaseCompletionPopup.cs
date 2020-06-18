using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseCompletionPopup : MonoBehaviour
    {
        public abstract event Action ReturnToMenu;

        public abstract void Display(Encounter encounter);
    }
}