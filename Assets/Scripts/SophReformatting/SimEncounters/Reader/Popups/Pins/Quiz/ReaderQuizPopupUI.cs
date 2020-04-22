using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizPopupUI : UserQuizPinDrawer
    {
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }

        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private ReaderPanelUI multipleChoicePanel;
        public ReaderPanelUI MultipleChoicePanel { get => multipleChoicePanel; set => multipleChoicePanel = value; }
        [SerializeField] private ReaderPanelUI checkBoxPanel;
        public ReaderPanelUI CheckBoxPanel { get => checkBoxPanel; set => checkBoxPanel = value; }

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public override void Display(UserQuizPin quizPin)
        {
            gameObject.SetActive(true);
            DeserializeChildren(quizPin.GetPanels());
        }

        public void DeserializeChildren(List<UserPanel> panels)
        {
            foreach (var panel in panels) {
                var panelData = panel.Data.Data;

                ReaderPanelUI panelPrefab;
                if (panelData.ContainsKey("OptionTypeValue") && panelData["OptionTypeValue"] == "Multiple Choice")
                    panelPrefab = MultipleChoicePanel;
                else
                    panelPrefab = CheckBoxPanel;
                var panelUI = Instantiate(panelPrefab, PanelsParent);
                panelUI.Display(panel);
            }
        }
    }
}