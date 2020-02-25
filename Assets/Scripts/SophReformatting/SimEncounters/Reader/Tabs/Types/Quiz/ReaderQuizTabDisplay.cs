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
            foreach (var panel in panels) {
                var panelData = panel.Value.Data;
                BaseReaderPanelUI panelUI;
                if (panelData.ContainsKey("OptionTypeValue") && panelData["OptionTypeValue"] == "Multiple Choice")
                    panelUI = ReaderPanelCreator.Deserialize(QuizTabUI.MultipleChoicePanel);
                else
                    panelUI = ReaderPanelCreator.Deserialize(QuizTabUI.CheckBoxPanel);
                Reader.PanelDisplayFactory.CreatePanel(panelUI, panel);
            }
        }

        public void Destroy() => Object.Destroy(QuizTabUI.GameObject);
    }
}