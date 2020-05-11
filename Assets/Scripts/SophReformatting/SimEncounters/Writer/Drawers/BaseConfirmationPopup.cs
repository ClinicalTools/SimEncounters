using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseConfirmationPopup : MonoBehaviour
    {
        public abstract void ShowConfirmation(Action confirmAction, string title, string description);
    }
}