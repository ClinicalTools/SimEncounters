using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTabButton : EncounterButton<Tab>
    {
        public WriterTabButton(EncounterWriter writer, TabButtonUI tabButtonUI, Tab tab) 
            : base(tabButtonUI.SelectButton, tab)
        {
            tabButtonUI.NameLabel.text = tab.Name;
        }
    }
}