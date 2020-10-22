using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserDialoguePinDrawer : MonoBehaviour
    {
        public abstract void Display(UserDialoguePin dialoguePin);

        public class Pool : MonoMemoryPool<BaseUserDialoguePinDrawer> { }
    }
}