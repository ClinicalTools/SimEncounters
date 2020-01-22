using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionsUI : MonoBehaviour
    {
        [field: SerializeField] private Transform sectionButtonsParent;
        public virtual Transform SectionButtonsParent { get => sectionButtonsParent; set => sectionButtonsParent = value; }

        [field: SerializeField] private Button addButton;
        public virtual Button AddButton { get => addButton; set => addButton = value; }

        [field: SerializeField] private TabsUI tabs;
        public virtual TabsUI Tabs { get => tabs; set => tabs = value; }

        [field: SerializeField] private SectionButtonUI sectionButtonPrefab;
        public virtual SectionButtonUI SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }

        [field: SerializeField] private SectionCreatorUI addSectionPrefab;
        public virtual SectionCreatorUI AddSectionPrefab { get => addSectionPrefab; set => addSectionPrefab = value; }
    }
}