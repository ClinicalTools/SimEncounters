using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
        public class ReaderSectionsUI : MonoBehaviour
    {
        [field: SerializeField] private Transform sectionButtonsParent;
        public virtual Transform SectionButtonsParent { get => sectionButtonsParent; set => sectionButtonsParent = value; }

        [field: SerializeField] private ReaderTabsUI tabs;
        public virtual ReaderTabsUI Tabs { get => tabs; set => tabs = value; }

        [field: SerializeField] private ReaderSectionToggleUI sectionButtonPrefab;
        public virtual ReaderSectionToggleUI SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }

        [field: SerializeField] private ToggleGroup sectionsToggleGroup;
        public virtual ToggleGroup SectionsToggleGroup { get => sectionsToggleGroup; set => sectionsToggleGroup = value; }

        [field: SerializeField] private List<Image> sectionBorders;
        public virtual List<Image> SectionBorders { get => sectionBorders; set => sectionBorders = value; }

    }
}