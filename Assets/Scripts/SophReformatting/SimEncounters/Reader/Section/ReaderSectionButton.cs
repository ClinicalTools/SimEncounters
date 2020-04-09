using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionButton : EncounterToggle<KeyValuePair<string, Section>>
    {
        protected virtual ReaderScene Reader { get; }
        protected virtual ReaderSectionToggleUI SectionToggleUI { get; }
        protected virtual Section Section { get; }
        public ReaderSectionButton(ReaderScene reader, ReaderSectionToggleUI sectionToggleUI, KeyValuePair<string, Section> keyedSection) : base(sectionToggleUI.SelectToggle, keyedSection)
        {
            Reader = reader;
            Section = keyedSection.Value;
            SectionToggleUI = sectionToggleUI;

            sectionToggleUI.NameLabel.text = Section.Name;

            SetColor(sectionToggleUI, Section.Color, false);
            var icons = reader.EncounterData.Images.Icons;
            if (icons.ContainsKey(Section.IconKey))
                sectionToggleUI.Icon.sprite = icons[Section.IconKey];

            Unselected += CheckRead;
            CheckRead(keyedSection);
        }

        public virtual void CheckRead(KeyValuePair<string, Section> keyedSection)
        {
            var isRead = true;
            foreach (var tab in keyedSection.Value.Tabs) {
                if (Reader.SceneInfo.Encounter.Status.ReadTabs.Contains(tab.Key))
                    continue;
                isRead = false;
                break;
            }
            SectionToggleUI.Visited.SetActive(isRead);
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