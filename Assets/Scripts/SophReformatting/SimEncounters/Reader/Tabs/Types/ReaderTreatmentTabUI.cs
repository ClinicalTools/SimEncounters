using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTreatmentTabUI : MonoBehaviour, IReaderTabUI
    {
        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private ReaderPanelUI treatmentPanel;
        protected ReaderPanelUI TreatmentPanel { get => treatmentPanel; set => treatmentPanel = value; }

        protected Tab Tab { get; private set; }
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }


        public void Initialize(ReaderScene reader, string tabFolder, Tab tab)
        {
            Tab = tab;
            ReaderPanelCreator = new ReaderPanelCreator(reader, PanelsParent);

            DeserializeChildren(tab);
        }

        public void DeserializeChildren(Tab tab)
        {
            if (tab.Panels.Count == 0)
                return;

            if (tab.Panels[0].Value.Type != TreatmentPanel.Type) {
                var basePanel = new Panel(TreatmentPanel.Type, tab.Panels);
                ReaderPanelCreator.Deserialize(new KeyValuePair<string, Panel>(null, basePanel), TreatmentPanel);
            } else {
                ReaderPanelCreator.Deserialize(tab.Panels, TreatmentPanel);
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