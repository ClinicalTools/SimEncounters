using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class DropdownValueField : MonoBehaviour, IValueField
    {
        public string Name => name;
        public string Value => (Dropdown.value >= 0) ? Dropdown.options[Dropdown.value].text : null;

        protected TMP_Dropdown Dropdown { get; set; }

        protected virtual void Awake()
        {
            Dropdown = GetComponent<TMP_Dropdown>();
        }
        public void Initialize() { }

        public void Initialize(string value)
        {
            for (int i = 0; i < Dropdown.options.Count; i++) {
                if (Dropdown.options[i].text != value)
                    continue;

                Dropdown.value = i;
                break;
            }
        }
    }
}