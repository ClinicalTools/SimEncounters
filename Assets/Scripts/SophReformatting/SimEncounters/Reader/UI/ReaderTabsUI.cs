using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabsUI : MonoBehaviour
    {
        [SerializeField] private Transform tabButtonsParent;
        public virtual Transform TabButtonsParent { get => tabButtonsParent; set => tabButtonsParent = value; }

        [SerializeField] private Transform tabContent;
        public virtual Transform TabContent { get => tabContent; set => tabContent = value; }

        [SerializeField] private ReaderTabButtonUI tabButtonPrefab;
        public virtual ReaderTabButtonUI TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
    }
}