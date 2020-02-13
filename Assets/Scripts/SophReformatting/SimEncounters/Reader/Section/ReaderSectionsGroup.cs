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
        protected virtual SectionsData SectionsData { get; set; }

        protected override int FirstButtonIndex => SectionsData.CurrentSectionIndex;

        public ReaderSectionsGroup(EncounterReader reader, ReaderSectionsUI sectionsUI, SectionsData sectionsData) 
            : base(sectionsData.Sections)
        {
            Reader = reader;
            SectionsUI = sectionsUI;
            SectionsData = sectionsData;
            Reader.Footer.NextSection += MoveToNextSection;

            CreateInitialButtons(sectionsData.Sections);
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

            SectionsData.CurrentSectionIndex = SectionsData.Sections.IndexOf(section);

            Tabs?.Delete();
            Tabs = new ReaderTabsGroup(Reader, SectionsUI.Tabs, section);
        }

        protected virtual void MoveToNextSection()
        {
            var sectionIndex = SectionsData.MoveToNextSection();
            SelectButtons[sectionIndex].Select();
        }
        protected virtual void MoveToPreviousSection()
        {
            var sectionIndex = SectionsData.MoveToPreviousSection();
            SelectButtons[sectionIndex].Select();
        }
    }
}