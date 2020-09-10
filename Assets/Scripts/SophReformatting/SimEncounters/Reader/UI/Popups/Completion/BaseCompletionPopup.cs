using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseCompletionPopup : MonoBehaviour, ICompletionDrawer
    {
        public abstract event Action ExitScene;
        public abstract void CompletionDraw(Encounter encounter);
    }
}