using ClinicalTools.SimEncounters.Data;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionCreator : IApply<Section>
    {
        protected virtual ColorEditor ColorEditor { get; }

        public event Action<Section> Apply;


        public SectionCreator(SectionCreatorUI sectionCreatorUI, EncounterWriter writer)
        {
            ColorEditor = new ColorEditor(sectionCreatorUI.Color);
            AddListeners(sectionCreatorUI);
        }

        /**
         * Referencing methods in the Unity editor is a mess, so I much prefer adding listeners this way. 
         * It makes it much easier to find what methods are actually used and where, as well as it not 
         * forcing methods to be public.
         */
        protected virtual void AddListeners(SectionCreatorUI sectionCreatorUI)
        {
            sectionCreatorUI.CancelButton.onClick.AddListener(() => Close(sectionCreatorUI));
            sectionCreatorUI.CreateButton.onClick.AddListener(() => AddSection(sectionCreatorUI));
        }

        protected virtual void AddSection(SectionCreatorUI sectionCreatorUI)
        {
            var name = sectionCreatorUI.NameField.text;
            var icon = GetIconReference(sectionCreatorUI.Icons);
            var color = ColorEditor.Value;

            var section = new Section(name, icon, color);

            Apply?.Invoke(section);

            Close(sectionCreatorUI);
        }

        protected virtual string IconName => "Icon";
        /**
         * Gets the icon image based on the selected section icon.
         */
        protected virtual string GetIconReference(Toggle[] icons)
        {
            var selectedIcon = icons.FirstOrDefault(icon => icon != null && icon.isOn);
            if (selectedIcon == null)
                return null;

            return selectedIcon.transform.Find(IconName).GetComponent<Image>().sprite.name;
        }

        protected virtual void Close(SectionCreatorUI sectionCreatorUI)
        {
            UnityEngine.Object.Destroy(sectionCreatorUI.gameObject);
        }
    }
}