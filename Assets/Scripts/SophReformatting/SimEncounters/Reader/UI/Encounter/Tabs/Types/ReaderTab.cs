
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTab : BaseUserTabDrawer
    {
        public BaseReaderPanelsCreator PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseReaderPanelsCreator panelCreator;

        public override void Display(UserTabSelectedEventArgs eventArgs)
        {
            if (PanelCreator != null)
                PanelCreator.DrawChildPanels(eventArgs.SelectedTab.GetPanels());
        }
    }
}