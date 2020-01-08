using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionCreatorUI : MonoBehaviour
    {
        [field: SerializeField] public Button CancelButton { get; set; }
        [field: SerializeField] public Button CreateButton { get; set; }


        [field: SerializeField] public TMP_InputField NameField { get; set; }

        [field: SerializeField] public ColorUI Color { get; set; }


        [field: SerializeField] protected Transform IconsParent { get; set; }
        private Toggle[] icons;
        public virtual Toggle[] Icons {
            get {
                if (icons == null)
                    icons = IconsParent.GetComponentsInChildren<Toggle>();

                return icons;
            }
        }
    }
}