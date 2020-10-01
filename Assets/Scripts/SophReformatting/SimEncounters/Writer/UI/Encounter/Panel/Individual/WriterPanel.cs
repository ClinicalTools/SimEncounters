using UnityEngine;

namespace ClinicalTools.SimEncounters
{

    public class WriterPanel : BaseWriterPanel
    {
        public BaseWriterPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseWriterPanelsDrawer childPanelCreator;

        public BaseWriterPinsDrawer PinsDrawer { get => pinsDrawer; set => pinsDrawer = value; }
        [SerializeField] private BaseWriterPinsDrawer pinsDrawer;

        protected Encounter CurrentEncounter { get; set; }
        protected Panel CurrentPanel { get; set; }
        public override void Display(Encounter encounter)
        {
            CurrentEncounter = encounter;
            CurrentPanel = new Panel(Type);
            var panelDisplay = new WriterPanelValueDisplay();
            Fields = panelDisplay.Display(encounter, CurrentPanel, transform);
            if (PinsDrawer != null)
                PinsDrawer.Display(encounter, new PinData());
        }
        protected IPanelField[] Fields { get; set; }
        public override void Display(Encounter encounter, Panel panel)
        {
            CurrentEncounter = encounter;
            CurrentPanel = new Panel(Type);
            var panelDisplay = new WriterPanelValueDisplay();
            Fields = panelDisplay.Display(encounter, panel, transform);

            if (panel.ChildPanels.Count > 0)
                ChildPanelCreator.DrawChildPanels(encounter, panel.ChildPanels);
            if (PinsDrawer != null) {
                if (panel.Pins == null)
                    panel.Pins = new PinData();
                PinsDrawer.Display(encounter, panel.Pins);
            }
        }

        public override Panel Serialize()
        {
            CurrentPanel.Type = Type;

            var panelDisplay = new WriterPanelValueDisplay();
            CurrentPanel.Values = panelDisplay.Serialize(Fields);
            if (ChildPanelCreator != null)
                CurrentPanel.ChildPanels = ChildPanelCreator.SerializeChildren();
            if (PinsDrawer != null)
                CurrentPanel.Pins = PinsDrawer.Serialize();

            return CurrentPanel;
        }
    }
}