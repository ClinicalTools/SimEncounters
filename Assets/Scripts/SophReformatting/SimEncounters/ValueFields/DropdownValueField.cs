using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class DropdownValueField : BaseValueField
    {
        public override string Name => name;
        public override string Value => (Dropdown.value >= 0) ? Dropdown.options[Dropdown.value].text : null;
        
        private TMP_Dropdown dropdown;
        protected TMP_Dropdown Dropdown {
            get {
                if (dropdown == null)
                    dropdown = GetComponent<TMP_Dropdown>();
                return dropdown;
            }
        }

        public override void Initialize() { }

        public override void Initialize(string value)
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