using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{

    public class WriterTab : BaseTabDrawer
    {
        public BaseWriterPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseWriterPanelsDrawer panelCreator;

        protected Encounter CurrentEncounter { get; set; }
        protected Tab CurrentTab { get; set; }
        public override void Display(Encounter encounter, Tab tab)
        {
            CurrentEncounter = encounter;
            CurrentTab = tab;
            if (tab.Panels.Count == 0)
                PanelCreator.DrawDefaultChildPanels(encounter);
            else
                PanelCreator.DrawChildPanels(encounter, tab.Panels);
        }

        public override Tab Serialize()
        {
            CurrentTab.Panels = PanelCreator.SerializeChildren();
            return CurrentTab;
        }
    }
}