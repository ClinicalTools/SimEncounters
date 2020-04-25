using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTab : UserTabDrawer
    {
        [SerializeField] private BaseChildPanelsDrawer panelCreator;
        public BaseChildPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }

        public override void Display(UserTab tab)
        {
            PanelCreator.DrawChildPanels(tab.GetPanels());
        }
    }
}