
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class UserQuizPinDrawer : MonoBehaviour
    {
        public abstract void Display(UserQuizPin quizPin);
    }

    public class ReaderPinButtonUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        public virtual Button Button { get => button; set => button = value; }
    }
    public abstract class UserDialoguePinDrawer : MonoBehaviour
    {
        public abstract void Display(UserDialoguePin dialoguePin);
    }
}