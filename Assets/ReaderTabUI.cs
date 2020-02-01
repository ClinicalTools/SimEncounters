using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabUI : MonoBehaviour, IReaderTabUI
    {
        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private List<ReaderPanelUI> panelOptions;
        public virtual List<ReaderPanelUI> PanelOptions { get => panelOptions; set => panelOptions = value; }

        protected Tab Tab { get; private set; }
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }
        protected List<ReaderPanelUI> ReaderPanels { get; set; }

        public virtual void Initialize(EncounterReader reader, string tabFolder, Tab tab)
        {
            Tab = tab;
            ReaderPanelCreator = new ReaderPanelCreator(reader, PanelsParent);

            if (tab.Panels.Count > 0)
                ReaderPanels = ReaderPanelCreator.Deserialize(tab.Panels, PanelOptions);
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