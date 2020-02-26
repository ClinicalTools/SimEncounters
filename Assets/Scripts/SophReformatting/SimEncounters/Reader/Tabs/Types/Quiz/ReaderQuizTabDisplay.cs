using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizTabDisplay : IReaderTabDisplay
    {
        protected ReaderScene Reader { get; }
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected ReaderQuizTabUI QuizTabUI { get; }

        public ReaderQuizTabDisplay(ReaderScene reader, ReaderQuizTabUI quizTabUI)
        {
            Reader = reader;
            QuizTabUI = quizTabUI;
            ReaderPanelCreator = new ReaderPanelCreator(reader, quizTabUI.PanelsParent);
        }

        public virtual void Display(KeyValuePair<string, Tab> keyedTab)
        {
            DeserializeChildren(keyedTab.Value.Panels);
        }

        protected virtual void DeserializeChildren(IEnumerable<KeyValuePair<string, Panel>> panels)
        {
            foreach (var keyedPanel in panels)
                DeserializeChild(keyedPanel);
        }

        protected virtual void DeserializeChild(KeyValuePair<string, Panel> keyedPanel)
        {
            var panelData = keyedPanel.Value.Data;
            BaseReaderPanelUI panelUI;
            if (panelData.ContainsKey("OptionTypeValue") && panelData["OptionTypeValue"] == "Multiple Choice")
                panelUI = ReaderPanelCreator.Deserialize(QuizTabUI.MultipleChoicePanel);
            else
                panelUI = ReaderPanelCreator.Deserialize(QuizTabUI.CheckBoxPanel);
            var panelDisplay = Reader.PanelDisplayFactory.CreatePanel(panelUI);
            panelDisplay.Display(keyedPanel);
        }

        public void Destroy() => Object.Destroy(QuizTabUI.GameObject);
    }
}