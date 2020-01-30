using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizTabUI : MonoBehaviour, IReaderTabUI
    {
        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private ReaderPanelUI multipleChoicePanel;
        protected ReaderPanelUI MultipleChoicePanel { get => multipleChoicePanel; set => multipleChoicePanel = value; }
        [SerializeField] private ReaderPanelUI checkBoxPanel;
        protected ReaderPanelUI CheckBoxPanel { get => checkBoxPanel; set => checkBoxPanel = value; }

        protected Tab Tab { get; private set; }
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }


        public void Initialize(EncounterReader reader, string tabFolder, Tab tab)
        {
            Tab = tab;
            ReaderPanelCreator = new ReaderPanelCreator(reader, PanelsParent);

            DeserializeChildren(Tab.Panels);
        }

        public void DeserializeChildren(OrderedCollection<Panel> panels)
        {
            foreach (var panel in panels) {
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