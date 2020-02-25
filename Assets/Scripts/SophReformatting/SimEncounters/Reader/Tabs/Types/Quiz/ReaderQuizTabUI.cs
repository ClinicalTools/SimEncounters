using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizTabUI : MonoBehaviour, IReaderTabUI
    {
        public GameObject GameObject => gameObject;

        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private BaseReaderPanelUI multipleChoicePanel;
        public BaseReaderPanelUI MultipleChoicePanel { get => multipleChoicePanel; set => multipleChoicePanel = value; }
        [SerializeField] private BaseReaderPanelUI checkBoxPanel;
        public BaseReaderPanelUI CheckBoxPanel { get => checkBoxPanel; set => checkBoxPanel = value; }
    }
}