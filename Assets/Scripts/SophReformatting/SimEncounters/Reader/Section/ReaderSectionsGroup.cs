using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionsGroup : ButtonGroup<Section>
    {
        protected virtual ReaderSectionsUI SectionsUI { get; }

        protected virtual ReaderTabsGroup Tabs { get; set; }
        protected virtual ReaderScene Reader { get; set; }
        protected virtual EncounterContent SectionsData { get; set; }

        protected override int FirstButtonIndex => SectionsData.CurrentSectionIndex;

        public ReaderSectionsGroup(ReaderScene reader, ReaderSectionsUI sectionsUI, EncounterContent sectionsData) 
            : base(sectionsData.Sections)
        {
            Reader = reader;
            SectionsUI = sectionsUI;
            SectionsData = sectionsData;
            Reader.Footer.NextSection += MoveToNextSection;

            CreateInitialButtons(sectionsData.Sections);
        }

        protected override ISelectable<KeyValuePair<string, Section>> AddButton(KeyValuePair<string, Section> keyedSection)
        {
            var sectionButtonUI = Object.Instantiate(SectionsUI.SectionButtonPrefab, SectionsUI.SectionButtonsParent);
            sectionButtonUI.SelectToggle.group = SectionsUI.SectionsToggleGroup;
            var sectionButton = new ReaderSectionButton(Reader, sectionButtonUI, keyedSection);
            return sectionButton;
        }

        protected override void Select(KeyValuePair<string, Section> keyedSection)
        {
            var section = keyedSection.Value;
            foreach (var sectionBorder in SectionsUI.SectionBorders)
                sectionBorder.color = section.Color;

            SectionsData.CurrentSectionIndex = SectionsData.Sections.IndexOf(section);

            Tabs?.Delete();
            Tabs = new ReaderTabsGroup(Reader, SectionsUI.Tabs, section);
            Tabs.MoveToPreviousSection += MoveToPreviousSection;
            Tabs.MoveToNextSection += MoveToNextSection;
        }

        protected virtual void MoveToNextSection()
        {
            var oldSectionIndex = SectionsData.CurrentSectionIndex;
            var sectionIndex = SectionsData.MoveToNextSection();
            if (oldSectionIndex != sectionIndex)
                SelectButtons[sectionIndex].Select();
        }
        protected virtual void MoveToPreviousSection()
        {
            var oldSectionIndex = SectionsData.CurrentSectionIndex;
            var sectionIndex = SectionsData.MoveToPreviousSection();
            if (oldSectionIndex != sectionIndex)
                SelectButtons[sectionIndex].Select();
        }
    }
}