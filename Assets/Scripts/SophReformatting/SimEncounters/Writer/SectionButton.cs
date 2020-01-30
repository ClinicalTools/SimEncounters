using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionButton : EncounterButton<Section>
    {
        public SectionButton(EncounterWriter writer, SectionButtonUI sectionButtonUI, Section section) : base(sectionButtonUI.SelectButton, section)
        {
            sectionButtonUI.NameLabel.text = section.Name;
        }
    }
}