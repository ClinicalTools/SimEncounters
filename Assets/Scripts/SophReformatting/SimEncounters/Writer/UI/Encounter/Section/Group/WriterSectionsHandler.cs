using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class WriterSectionsHandler : BaseWriterSectionsHandler
    {
        public virtual BaseRearrangeableGroup RearrangeableGroup { get => rearrangeableGroup; set => rearrangeableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup rearrangeableGroup;
        public virtual BaseWriterSectionToggle SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }
        [SerializeField] private BaseWriterSectionToggle sectionButtonPrefab;
        public virtual ToggleGroup SectionsToggleGroup { get => sectionsToggleGroup; set => sectionsToggleGroup = value; }
        [SerializeField] private ToggleGroup sectionsToggleGroup;
        public virtual Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;
        public SectionCreatorPopup AddSectionPopup { get => addSectionPopup; set => addSectionPopup = value; }
        [SerializeField] private SectionCreatorPopup addSectionPopup;

        public override event SectionSelectedHandler SectionSelected;
        public override event Action<Section> SectionEdited;
        public override event Action<Section> SectionDeleted;

        protected Encounter CurrentEncounter { get; set; }
        protected Dictionary<Section, BaseWriterSectionToggle> SectionButtons { get; } = new Dictionary<Section, BaseWriterSectionToggle>();
        
        protected virtual void Awake()
        {
            AddButton.onClick.AddListener(AddSection);
            RearrangeableGroup.Rearranged += SectionsRearranged;
        }

        public override void Display(Encounter encounter)
        {
            RearrangeableGroup.Clear();
            foreach (var sectionButton in SectionButtons)
                Destroy(sectionButton.Value.gameObject);
            SectionButtons.Clear();

            CurrentEncounter = encounter;
            foreach (var section in encounter.Content.NonImageContent.Sections)
                AddSectionButton(encounter, section.Value);
        }
        protected virtual void AddSection()
        {
            var newSection = AddSectionPopup.CreateSection(CurrentEncounter);
            newSection.AddOnCompletedListener(AddNewSection);
        }

        protected virtual void AddNewSection(WaitedResult<Section> section)
        {
            if (section.IsError() || section.Value == null)
                return;

            CurrentEncounter.Content.NonImageContent.Sections.Add(section.Value);
            AddSectionButton(CurrentEncounter, section.Value);

            var sectionSelectedArgs = new SectionSelectedEventArgs(section.Value);
            SectionSelected?.Invoke(this, sectionSelectedArgs);
        }

        protected void AddSectionButton(Encounter encounter, Section section)
        {
            var sectionButton = RearrangeableGroup.AddFromPrefab(SectionButtonPrefab);
            sectionButton.SetToggleGroup(SectionsToggleGroup);
            sectionButton.Display(encounter, section);
            sectionButton.Selected += () => OnSelected(section);
            sectionButton.Edited += (editedSection) => SectionEdited?.Invoke(editedSection);
            sectionButton.Deleted += OnDeleted;
            SectionButtons.Add(section, sectionButton);
        }

        protected Section CurrentSection { get; set; }
        protected void OnSelected(Section section)
        {
            var selectedArgs = new SectionSelectedEventArgs(section);
            CurrentSection = section;
            SectionSelected?.Invoke(this, selectedArgs);
        }
        protected void OnEdited(Section section)
        {
            var selectedArgs = new SectionSelectedEventArgs(section);
            CurrentSection = section;
            SectionSelected?.Invoke(this, selectedArgs);
        }
        protected void OnDeleted(Section section)
        {
            var button = SectionButtons[section];
            RearrangeableGroup.Remove(button);
            SectionButtons.Remove(section);
            CurrentEncounter.Content.NonImageContent.Sections.Remove(section);

            CurrentSection = section;
            SectionDeleted?.Invoke(section);
        }

        public override void SelectSection(Section section)
        {
            if (CurrentSection == section)
                return;

            SectionButtons[section].Select();
        }

        private void SectionsRearranged(object sender, RearrangedEventArgs2 e)
        {
            var sections = CurrentEncounter.Content.NonImageContent.Sections;
            sections.MoveValue(e.NewIndex, e.OldIndex);
        }
    }
}