using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizTabDisplay : IReaderTabDisplay
    {
        protected ReaderScene Reader { get; }
        protected Tab Tab { get; }
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected ReaderQuizTabUI QuizTabUI { get; }

        public ReaderQuizTabDisplay(ReaderScene reader, ReaderQuizTabUI quizTabUI, KeyValuePair<string, Tab> keyedTab)
        {
            Reader = reader;
            Tab = keyedTab.Value;
            QuizTabUI = quizTabUI;
            ReaderPanelCreator = new ReaderPanelCreator(reader, quizTabUI.PanelsParent);

            DeserializeChildren(Tab.Panels);
        }

        public void DeserializeChildren(IEnumerable<KeyValuePair<string,Panel>> panels)
        {
            foreach (var keyedPanel in panels) {
                var panelData = keyedPanel.Value.Data;
                BaseReaderPanelUI panelUI;
                if (panelData.ContainsKey("OptionTypeValue") && panelData["OptionTypeValue"] == "Multiple Choice")
                    panelUI = ReaderPanelCreator.Deserialize(QuizTabUI.MultipleChoicePanel);
                else
                    panelUI = ReaderPanelCreator.Deserialize(QuizTabUI.CheckBoxPanel);
                var panelDisplay = Reader.PanelDisplayFactory.CreatePanel(panelUI);
                panelDisplay.Display(keyedPanel);
            }
        }

        public void Destroy() => Object.Destroy(QuizTabUI.GameObject);
    }
}