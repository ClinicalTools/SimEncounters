using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabsUI : MonoBehaviour
    {
        [field: SerializeField] public virtual Transform TabButtonsParent { get; set; }
        [field: SerializeField] public virtual Button AddButton { get; set; }
        [field: SerializeField] public virtual Transform TabContent { get; set; }
        [field: SerializeField] public virtual AutofillTMP Help { get; set; }

        [field: SerializeField] public virtual TabButtonUI TabButtonPrefab { get; set; }
        [field: SerializeField] public virtual TabCreatorUI AddTabPopup { get; set; }
    }
}