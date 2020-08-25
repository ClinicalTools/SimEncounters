using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseConfirmationPopup : MonoBehaviour
    {
        public abstract void ShowConfirmation(Action confirmAction, string title, string description);
    }
}