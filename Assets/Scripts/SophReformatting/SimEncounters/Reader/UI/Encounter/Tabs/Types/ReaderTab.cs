
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTab : BaseUserTabDrawer
    {
        [SerializeField] private BaseReaderPanelsCreator panelCreator;
        public BaseReaderPanelsCreator PanelCreator { get => panelCreator; set => panelCreator = value; }

        public override void Display(UserTabSelectedEventArgs eventArgs)
        {
            PanelCreator.DrawChildPanels(eventArgs.SelectedTab.GetPanels());
        }
    }
}