using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{

    public class ReaderSectionButton : EncounterToggle<Section>
    {
        protected virtual EncounterReader Reader { get; }
        protected virtual ReaderSectionToggleUI SectionToggleUI { get; }
        protected virtual Section Section { get; }
        public ReaderSectionButton(EncounterReader reader, ReaderSectionToggleUI sectionToggleUI, Section section) : base(sectionToggleUI.SelectToggle, section)
        {
            Reader = reader;
            Section = section;
            SectionToggleUI = sectionToggleUI;

            sectionToggleUI.NameLabel.text = section.Name;

            SetColor(sectionToggleUI, section.Color, false);
            var icons = reader.Encounter.Images.Icons;
            if (icons.ContainsKey(section.IconKey))
                sectionToggleUI.Icon.sprite = icons[section.IconKey];
        }

        public override void Select() => SectionToggleUI.SelectToggle.isOn = true;

        protected override void ToggleChanged(bool isOn)
        {
            base.ToggleChanged(isOn);
            SetColor(SectionToggleUI, Section.Color, isOn);
        }

        protected virtual void SetColor(ReaderSectionToggleUI sectionToggleUI, Color color, bool isOn)
        {
            if (isOn) {
                sectionToggleUI.Image.color = color;
                sectionToggleUI.Icon.color = Color.white;
                sectionToggleUI.NameLabel.color = Color.white;
            } else {
                sectionToggleUI.Image.color = Color.white;
                sectionToggleUI.Icon.color = color;
                sectionToggleUI.NameLabel.color = color;
            }
        }
    }
}