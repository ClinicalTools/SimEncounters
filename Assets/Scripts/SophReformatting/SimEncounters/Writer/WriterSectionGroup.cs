using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSectionGroup : ButtonGroup<Section>
    {
        protected virtual SectionsUI SectionsUI { get; }
        
        protected virtual WriterTabGroup Tabs { get; set; }
        protected virtual EncounterWriter Writer { get; set; }

        public WriterSectionGroup(EncounterWriter writer, SectionsUI sectionsUI, OrderedCollection<Section> sections) : base(sections)
        {
            Writer = writer;
            SectionsUI = sectionsUI;

            CreateInitialButtons(sections);

            AddListeners();
        }

        protected virtual void AddListeners()
        {
            SectionsUI.AddButton.onClick.RemoveAllListeners();
            SectionsUI.AddButton.onClick.AddListener(OpenAddSectionPopup);
        }

        protected virtual void OpenAddSectionPopup()
        {
            var sectionCreatorUI = Writer.OpenPopup(SectionsUI.AddSectionPrefab);
            IApply<Section> sectionCreator = new SectionCreator(sectionCreatorUI, Writer);
            sectionCreator.Apply += Add;
        }

        protected override ISelectable<Section> AddButton(Section section)
        {
            var sectionButtonUI = Object.Instantiate(SectionsUI.SectionButtonPrefab, SectionsUI.SectionButtonsParent);
            ISelectable<Section> sectionButton = new SectionButton(Writer, sectionButtonUI, section);
            return sectionButton;
        }

        protected override void Select(Section section)
        {
            Tabs?.Delete();
            Tabs = new WriterTabGroup(Writer, SectionsUI.Tabs, section.Tabs);
        }
    }
}