using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabsUI : MonoBehaviour
    {
        [SerializeField] private Transform tabButtonsParent;
        public virtual Transform TabButtonsParent { get => tabButtonsParent; set => tabButtonsParent = value; }

        [SerializeField] private Button addButton;
        public virtual Button AddButton { get => addButton; set => addButton = value; }

        [SerializeField] private Transform tabContent;
        public virtual Transform TabContent { get => tabContent; set => tabContent = value; }

        [SerializeField] private AutofillTMP help;
        public virtual AutofillTMP Help { get => help; set => help = value; }

        [SerializeField] private TabButtonUI tabButtonPrefab;
        public virtual TabButtonUI TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }

        [SerializeField] private TabCreatorUI addTabPopup;
        public virtual TabCreatorUI AddTabPopup { get => addTabPopup; set => addTabPopup = value; }
    }
}