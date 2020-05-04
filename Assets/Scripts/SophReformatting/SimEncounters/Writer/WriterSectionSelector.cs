using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSectionSelector : BaseSectionSelector
    {
        [SerializeField] private Transform sectionButtonsParent;
        public virtual Transform SectionButtonsParent { get => sectionButtonsParent; set => sectionButtonsParent = value; }

        [SerializeField] private WriterSectionToggle sectionButtonPrefab;
        public virtual WriterSectionToggle SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }

        [SerializeField] private ToggleGroup sectionsToggleGroup;
        public virtual ToggleGroup SectionsToggleGroup { get => sectionsToggleGroup; set => sectionsToggleGroup = value; }


        public override event SectionSelectedHandler SectionSelected;

        protected Encounter Encounter { get; set; }
        protected Dictionary<Section, WriterSectionToggle> SectionButtons { get; } = new Dictionary<Section, WriterSectionToggle>();
        public override void Display(Encounter encounter)
        {
            foreach (var sectionButton in SectionButtons)
                Destroy(sectionButton.Value.gameObject);
            SectionButtons.Clear();

            Encounter = encounter;
            foreach (var section in encounter.Content.Sections) {
                AddButton(encounter, section.Value);
            }
        }

        protected void AddButton(Encounter encounter, Section section)
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