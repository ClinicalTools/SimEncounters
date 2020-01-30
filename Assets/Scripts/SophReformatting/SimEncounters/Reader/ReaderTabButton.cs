using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabButton : EncounterButton<Tab>
    {
        public ReaderTabButton(EncounterReader reader, ReaderTabButtonUI tabButtonUI, Tab tab) 
            : base(tabButtonUI.SelectButton, tab)
        {
            tabButtonUI.NameLabel.text = tab.Name;
        }
    }
}