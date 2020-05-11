using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterQuizPopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button RemoveButton { get => removeButton; set => removeButton = value; }
        [SerializeField] private Button removeButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;

        public BaseWriterPanelsDrawer QuestionsDrawer { get => questionsDrawer; set => questionsDrawer = value; }
        [SerializeField] private BaseWriterPanelsDrawer questionsDrawer;

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            RemoveButton.onClick.AddListener(Remove);
            ApplyButton.onClick.AddListener(Apply);
        }

        protected WaitableResult<QuizPin> CurrentWaitableQuiz { get; set; }
        protected Encounter CurrentEncounter { get; set; }
        protected QuizPin CurrentQuiz { get; set; }
        public virtual WaitableResult<QuizPin> EditQuiz(Encounter encounter, QuizPin quizPin)
        {
            CurrentEncounter = encounter;
            CurrentQuiz = quizPin;

            if (CurrentWaitableQuiz?.IsCompleted == false)
                CurrentWaitableQuiz.SetError("New popup opened");

            CurrentWaitableQuiz = new WaitableResult<QuizPin>();

            gameObject.SetActive(true);

            QuestionsDrawer.DrawChildPanels(encounter, quizPin.Questions);

            return CurrentWaitableQuiz;
        }

        protected virtual void Apply()
        {
            CurrentQuiz.Questions = QuestionsDrawer.SerializeChildren();
            CurrentWaitableQuiz.SetResult(CurrentQuiz);

            Close();
        }

        protected virtual void Remove()
        {
            CurrentWaitableQuiz.SetResult(null);

            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableQuiz?.IsCompleted == false)
                CurrentWaitableQuiz.SetError("Canceled");

            gameObject.SetActive(false);
        }
    }
}