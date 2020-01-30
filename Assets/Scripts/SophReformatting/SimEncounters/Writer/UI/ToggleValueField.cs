using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ToggleValueField : MonoBehaviour, IValueField
    {
        public string Name => name;
        public string Value => Toggle.isOn ? true.ToString() : null;

        private Toggle toggle;
        protected Toggle Toggle {
            get {
                if (toggle == null)
                    toggle = GetComponent<Toggle>();
                return toggle;
            }
        }

        public virtual void Initialize() { }
        public virtual void Initialize(string value)
        {
            if (bool.TryParse(value, out var boolVal))
                Toggle.isOn = boolVal;
        }
    }
}