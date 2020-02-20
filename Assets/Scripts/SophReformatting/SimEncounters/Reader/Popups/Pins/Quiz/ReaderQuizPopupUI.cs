using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizPopupUI : PopupUI
    {
        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private ReaderPanelUI multipleChoicePanel;
        public ReaderPanelUI MultipleChoicePanel { get => multipleChoicePanel; set => multipleChoicePanel = value; }
        [SerializeField] private ReaderPanelUI checkBoxPanel;
        public ReaderPanelUI CheckBoxPanel { get => checkBoxPanel; set => checkBoxPanel = value; }
    }
}