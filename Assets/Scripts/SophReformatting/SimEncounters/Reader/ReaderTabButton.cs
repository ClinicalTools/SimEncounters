using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabButton : EncounterToggle<Tab>
    {
        public ReaderTabButton(EncounterReader reader, ReaderTabToggleUI tabToggleUI, Tab tab) 
            : base(tabToggleUI.SelectToggle, tab)
        {
            tabToggleUI.NameLabel.text = tab.Name;
        }
    }
}