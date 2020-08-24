
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTab : BaseUserTabDrawer
    {
        [SerializeField] private BaseReaderPanelsCreator panelCreator;
        public BaseReaderPanelsCreator PanelCreator { get => panelCreator; set => panelCreator = value; }

        public override void Display(UserTab tab)
        {
            PanelCreator.DrawChildPanels(tab.GetPanels());
        }
    }
}