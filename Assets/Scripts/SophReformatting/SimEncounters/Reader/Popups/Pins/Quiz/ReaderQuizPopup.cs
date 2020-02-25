﻿using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizPopup : ReaderPopup
    {
        protected ReaderScene Reader { get; }
        protected virtual ReaderPanelCreator ReaderPanelCreator { get; }
        protected virtual ReaderQuizPopupUI QuizPopupUI { get; }
        public ReaderQuizPopup(ReaderScene reader, ReaderQuizPopupUI quizPopupUI, QuizPin pin) : base(reader, quizPopupUI)
        {
            Reader = reader;
            QuizPopupUI = quizPopupUI;

            ReaderPanelCreator = new ReaderPanelCreator(reader, quizPopupUI.PanelsParent);

            DeserializeChildren(pin.Questions);
        }

        public void DeserializeChildren(OrderedCollection<Panel> panels)
        {
            foreach (var panel in panels) {
                var panelData = panel.Value.Data;

                ReaderPanelUI panelUI;
                if (panelData.ContainsKey("OptionTypeValue") && panelData["OptionTypeValue"] == "Multiple Choice")
                    panelUI = ReaderPanelCreator.Deserialize(QuizPopupUI.MultipleChoicePanel);
                else
                    panelUI = ReaderPanelCreator.Deserialize(QuizPopupUI.CheckBoxPanel);
                Reader.PanelDisplayFactory.CreatePanel(panelUI, panel);
            }
        }
    }
}