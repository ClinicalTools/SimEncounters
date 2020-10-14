using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IUserDialoguePinDrawer
    {
        void Display(UserDialoguePin dialoguePin);
    }

    public abstract class UserDialoguePinDrawer : MonoBehaviour, IUserDialoguePinDrawer
    {
        public abstract void Display(UserDialoguePin dialoguePin);
    }
}