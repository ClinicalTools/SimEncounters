using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class UserDialoguePinDrawer : MonoBehaviour
    {
        public abstract void Display(UserDialoguePin dialoguePin);
    }
}