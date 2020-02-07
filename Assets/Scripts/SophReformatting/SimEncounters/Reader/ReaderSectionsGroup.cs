using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionsGroup : ButtonGroup<Section>
    {
        protected virtual ReaderSectionsUI SectionsUI { get; }

        protected virtual ReaderTabsGroup Tabs { get; set; }
        protected virtual EncounterReader Reader { get; set; }

        public ReaderSectionsGroup(EncounterReader reader, ReaderSectionsUI sectionsUI, OrderedCollection<Section> sections) 
            : base(sections)
        {
            Reader = reader;
            SectionsUI = sectionsUI;

            CreateInitialButtons(sections);
        }

        protected override ISelectable<Section> AddButton(Section section)
        {
            var sectionButtonUI = Object.Instantiate(SectionsUI.SectionButtonPrefab, SectionsUI.SectionButtonsParent);
            sectionButtonUI.SelectToggle.group = SectionsUI.SectionsToggleGroup;
            var sectionButton = new ReaderSectionButton(Reader, sectionButtonUI, section);
            return sectionButton;
        }

        protected override void Select(Section section)
        {
            foreach (var sectionBorder in SectionsUI.SectionBorders)
                sectionBorder.color = section.Color;
            Tabs?.Delete();
            Tabs = new ReaderTabsGroup(Reader, SectionsUI.Tabs, section.Tabs);
        }
    }
}