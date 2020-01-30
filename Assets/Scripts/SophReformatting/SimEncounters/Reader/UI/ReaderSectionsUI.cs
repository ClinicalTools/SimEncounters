using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionsUI : MonoBehaviour
    {
        [field: SerializeField] private Transform sectionButtonsParent;
        public virtual Transform SectionButtonsParent { get => sectionButtonsParent; set => sectionButtonsParent = value; }

        [field: SerializeField] private ReaderTabsUI tabs;
        public virtual ReaderTabsUI Tabs { get => tabs; set => tabs = value; }

        [field: SerializeField] private ReaderSectionButtonUI sectionButtonPrefab;
        public virtual ReaderSectionButtonUI SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }
    }
}