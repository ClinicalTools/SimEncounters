using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionButton : EncounterButton<Section>
    {
        public SectionButton(SectionButtonUI sectionButtonUI, Section section, EncounterWriter writer) : base(sectionButtonUI.SelectButton, section)
        {
            sectionButtonUI.NameLabel.text = section.Name;
        }
    }
}