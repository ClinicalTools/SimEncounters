using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionsGroup
    {
        protected virtual SectionsUI SectionsUI { get; }
        protected virtual OrderedCollection<Section> Sections { get; }

        protected virtual TabsGroup Tabs { get; set; }
        protected virtual EncounterWriter Writer { get; set; }

        public SectionsGroup(SectionsUI sectionsUI, OrderedCollection<Section> sections, EncounterWriter writer)
        {
            SectionsUI = sectionsUI;
            Sections = Sections;
            Writer = writer;

            AddListeners();
            foreach (var section in sections) {
                AddSectionButton(section.Value);
            }
            if (sections.Count > 0)
                Select(sections[0].Value);
        }

        protected virtual void AddListeners()
        {
            SectionsUI.AddButton.onClick.RemoveAllListeners();
            SectionsUI.AddButton.onClick.AddListener(OpenAddSectionPopup);
        }

        protected virtual void OpenAddSectionPopup()
        {
            var sectionCreatorUI = Writer.OpenPanel(SectionsUI.AddSectionPrefab);
            IApply<Section> sectionCreator = new SectionCreator(sectionCreatorUI, Writer);
            sectionCreator.Apply += Add;
        }

        protected virtual void Add(Section section)
        {
            Sections.Add(section);
            AddSectionButton(section);
        }

        protected virtual ISelectable<Section> AddSectionButton(Section section)
        {
            var sectionButtonUI = Object.Instantiate(SectionsUI.SectionButtonPrefab, SectionsUI.SectionButtonsParent);
            ISelectable<Section> sectionButton = new SectionButton(sectionButtonUI, section, Writer);
            //sectionButton.Selected += Select();
            // section changed call Select
            return sectionButton;
        }


        protected virtual void Select(Section section)
        {
            Tabs = new TabsGroup(SectionsUI.Tabs, section.Tabs, Writer);
            // section changed event
        }

    }
}