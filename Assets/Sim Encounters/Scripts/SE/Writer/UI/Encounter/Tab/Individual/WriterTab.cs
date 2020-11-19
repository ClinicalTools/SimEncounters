﻿using UnityEngine;

namespace ClinicalTools.SimEncounters
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
                PanelCreator.DrawDefaultChildPanels();
            else
                PanelCreator.DrawChildPanels(tab.Panels);
        }

        public override Tab Serialize()
        {
            CurrentTab.Panels = PanelCreator.SerializeChildren();
            return CurrentTab;
        }
    }
}