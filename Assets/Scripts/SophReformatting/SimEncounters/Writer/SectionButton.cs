using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionButton : EncounterButton<KeyValuePair<string, Section>>
    {
        protected virtual bool IsRead { get; set; }
        public SectionButton(EncounterWriter writer, SectionButtonUI sectionButtonUI, KeyValuePair<string, Section> keyedSection) : base(sectionButtonUI.SelectButton, keyedSection)
        {
            sectionButtonUI.NameLabel.text = keyedSection.Value.Name;
        }
    }
}