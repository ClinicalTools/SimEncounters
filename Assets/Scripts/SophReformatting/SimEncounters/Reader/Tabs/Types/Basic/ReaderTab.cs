using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTab : UserTabDrawer
    {
        [SerializeField] private ChildPanelsDrawer panelCreator;
        public ChildPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }

        public override void Display(UserTab tab)
        {
            if (PanelCreator != null)
                PanelCreator.DrawChildPanels(tab.GetPanels());
        }
    }
}