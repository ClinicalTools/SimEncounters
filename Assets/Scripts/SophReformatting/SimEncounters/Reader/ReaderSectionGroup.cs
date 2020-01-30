using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionGroup : ButtonGroup<Section>
    {
        protected virtual ReaderSectionsUI SectionsUI { get; }

        protected virtual ReaderTabsGroup Tabs { get; set; }
        protected virtual EncounterReader Reader { get; set; }

        public ReaderSectionGroup(EncounterReader reader, ReaderSectionsUI sectionsUI, OrderedCollection<Section> sections) 
            : base(sections)
        {
            Reader = reader;
            SectionsUI = sectionsUI;

            CreateInitialButtons(sections);
        }

        protected override ISelectable<Section> AddButton(Section section)
        {
            var sectionButtonUI = Object.Instantiate(SectionsUI.SectionButtonPrefab, SectionsUI.SectionButtonsParent);
            ISelectable<Section> sectionButton = new ReaderSectionButton(Reader, sectionButtonUI, section);
            return sectionButton;
        }

        protected override void Select(Section section)
        {
            Tabs?.Delete();
            Tabs = new ReaderTabsGroup(Reader, SectionsUI.Tabs, section.Tabs);
        }
    }
}