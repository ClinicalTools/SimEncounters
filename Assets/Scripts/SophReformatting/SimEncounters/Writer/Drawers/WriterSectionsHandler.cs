using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSectionsHandler : BaseWriterSectionsHandler
    {
        public virtual Transform SectionButtonsParent { get => sectionButtonsParent; set => sectionButtonsParent = value; }
        [SerializeField] private Transform sectionButtonsParent;
        public virtual WriterSectionToggle SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }
        [SerializeField] private WriterSectionToggle sectionButtonPrefab;
        public virtual ToggleGroup SectionsToggleGroup { get => sectionsToggleGroup; set => sectionsToggleGroup = value; }
        [SerializeField] private ToggleGroup sectionsToggleGroup;
        public virtual Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;
        public SectionCreatorPopup AddSectionPopup { get => addSectionPopup; set => addSectionPopup = value; }
        [SerializeField] private SectionCreatorPopup addSectionPopup;

        public override event SectionSelectedHandler SectionSelected;
        public override event RearrangedHandler Rearranged;

        protected Encounter CurrentEncounter { get; set; }
        protected Dictionary<Section, WriterSectionToggle> SectionButtons { get; } = new Dictionary<Section, WriterSectionToggle>();
        public override void Display(Encounter encounter)
        {
            foreach (var sectionButton in SectionButtons)
                Destroy(sectionButton.Value.gameObject);
            SectionButtons.Clear();

            CurrentEncounter = encounter;
            foreach (var section in encounter.Content.Sections)
                AddSectionButton(encounter, section.Value);

            AddButton.onClick.AddListener(AddSection);
        }
        protected virtual void AddSection()
        {
            var newSection = AddSectionPopup.CreateSection();
            newSection.AddOnCompletedListener(AddNewSection);
        }

        protected virtual void AddNewSection(Section section)
        {
            if (section == null)
                return;

            CurrentEncounter.Content.Sections.Add(section);
            AddSectionButton(CurrentEncounter, section);

            var sectionSelectedArgs = new SectionSelectedEventArgs(section);
            SectionSelected?.Invoke(this, sectionSelectedArgs);
        }

        protected void AddSectionButton(Encounter encounter, Section section)
        {
            var sectionButton = Instantiate(SectionButtonPrefab, SectionButtonsParent);
            sectionButton.SetToggleGroup(SectionsToggleGroup);
            sectionButton.Display(encounter, section);
            sectionButton.Selected += () => OnSelected(section);
            SectionButtons.Add(section, sectionButton);
        }

        protected Section CurrentSection { get; set; }
        protected void OnSelected(Section section)
        {
            var selectedArgs = new SectionSelectedEventArgs(section);
            CurrentSection = section;
            SectionSelected?.Invoke(this, selectedArgs);
        }


        public override void SelectSection(Section section)
        {
            if (CurrentSection == section)
                return;

            SectionButtons[section].Select();
        }
    }
}