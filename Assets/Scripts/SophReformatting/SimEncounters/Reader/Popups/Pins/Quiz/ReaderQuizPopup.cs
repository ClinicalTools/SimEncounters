using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizPopup : ReaderPopup
    {
        protected virtual ReaderPanelCreator ReaderPanelCreator { get; }
        protected virtual ReaderQuizPopupUI QuizPopupUI { get; }
        public ReaderQuizPopup(ReaderScene reader, ReaderQuizPopupUI quizPopupUI, QuizPin pin) : base(reader, quizPopupUI)
        {
            QuizPopupUI = quizPopupUI;

            ReaderPanelCreator = new ReaderPanelCreator(reader, quizPopupUI.PanelsParent);

            DeserializeChildren(pin.Questions);
        }

        public void DeserializeChildren(OrderedCollection<Panel> panels)
        {
            foreach (var panel in panels) {
                var panelData = panel.Value.Data;
                if (panelData.ContainsKey("OptionTypeValue") && panelData["OptionTypeValue"] == "Multiple Choice")
                    ReaderPanelCreator.Deserialize(panel, QuizPopupUI.MultipleChoicePanel);
                else
                    ReaderPanelCreator.Deserialize(panel, QuizPopupUI.CheckBoxPanel);
            }
        }
    }
}