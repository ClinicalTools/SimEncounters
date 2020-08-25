using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class UserDialoguePinDrawer : MonoBehaviour
    {
        public abstract void Display(UserDialoguePin dialoguePin);
    }
}