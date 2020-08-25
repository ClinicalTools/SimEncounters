using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizPinButton : UserQuizPinDrawer
    {
        [SerializeField] private Button button;
        public virtual Button Button { get => button; set => button = value; }

        protected UserQuizPinDrawer QuizPopup { get; set; }
        [Inject] public virtual void Inject(UserQuizPinDrawer quizPopup)
        {
            QuizPopup = quizPopup;
        }


        public override void Display(UserQuizPin quizPin)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => QuizPopup.Display(quizPin));
        }
    }
}