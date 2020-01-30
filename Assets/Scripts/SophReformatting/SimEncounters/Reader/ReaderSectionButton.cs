using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionButton : EncounterButton<Section>
    {
        public ReaderSectionButton(EncounterReader reader, ReaderSectionButtonUI sectionButtonUI, Section section) : base(sectionButtonUI.SelectButton, section)
        {
            sectionButtonUI.NameLabel.text = section.Name;
        }
    }
}