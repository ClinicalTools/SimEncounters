using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizTabDisplay : IReaderTabDisplay
    {
        protected Tab Tab { get; private set; }
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }
        protected List<BaseReaderPanelUI> ReaderPanels { get; set; }
        protected ReaderQuizTabUI QuizTabUI { get; private set; }

        public ReaderQuizTabDisplay(ReaderScene reader, ReaderQuizTabUI quizTabUI, Tab tab)
        {
            Tab = tab;
            QuizTabUI = quizTabUI;
            ReaderPanelCreator = new ReaderPanelCreator(reader, quizTabUI.PanelsParent);

            DeserializeChildren(Tab.Panels);
        }

        public void DeserializeChildren(OrderedCollection<Panel> panels)
        {
            foreach (var panel in panels)
            {
                var panelData = panel.Value.Data;
                if (panelData.ContainsKey("OptionTypeValue") && panelData["OptionTypeValue"] == "Multiple Choice")
                    ReaderPanelCreator.Deserialize(panel, QuizTabUI.MultipleChoicePanel);
                else
                    ReaderPanelCreator.Deserialize(panel, QuizTabUI.CheckBoxPanel);
            }
        }

        public void Destroy()
        {
            Object.Destroy(QuizTabUI.GameObject);
        }
    }

    public class ReaderQuizTabUI : MonoBehaviour, IReaderTabUI
    {
        public GameObject GameObject => gameObject;

        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private BaseReaderPanelUI multipleChoicePanel;
        public BaseReaderPanelUI MultipleChoicePanel { get => multipleChoicePanel; set => multipleChoicePanel = value; }
        [SerializeField] private BaseReaderPanelUI checkBoxPanel;
        public BaseReaderPanelUI CheckBoxPanel { get => checkBoxPanel; set => checkBoxPanel = value; }

        protected Tab Tab { get; private set; }
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }


        public void Initialize(ReaderScene reader, string tabFolder, Tab tab)
        {
            Tab = tab;
            ReaderPanelCreator = new ReaderPanelCreator(reader, PanelsParent);

            DeserializeChildren(Tab.Panels);
        }

        public void DeserializeChildren(OrderedCollection<Panel> panels)
        {
            foreach (var panel in panels)
            {
                var panelData = panel.Value.Data;
                if (panelData.ContainsKey("OptionTypeValue") && panelData["OptionTypeValue"] == "Multiple Choice")
                    ReaderPanelCreator.Deserialize(panel, MultipleChoicePanel);
                else
                    ReaderPanelCreator.Deserialize(panel, CheckBoxPanel);
            }
        }

        public void Serialize()
        {
        }

        public void Destroy()
        {
            Serialize();
            Destroy(gameObject);
        }
    }
}