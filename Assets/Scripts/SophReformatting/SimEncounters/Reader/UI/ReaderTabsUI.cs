using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabsUI : MonoBehaviour
    {
        [SerializeField] private Transform tabButtonsParent;
        public virtual Transform TabButtonsParent { get => tabButtonsParent; set => tabButtonsParent = value; }

        [SerializeField] private Transform tabContent;
        public virtual Transform TabContent { get => tabContent; set => tabContent = value; }

        [SerializeField] private ReaderTabToggleUI tabButtonPrefab;
        public virtual ReaderTabToggleUI TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }

        [field: SerializeField] private ToggleGroup tabsToggleGroup;
        public virtual ToggleGroup TabsToggleGroup { get => tabsToggleGroup; set => tabsToggleGroup = value; }
    }
}