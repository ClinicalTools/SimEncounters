using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ToggleValueField : MonoBehaviour, IValueField
    {
        public string Name => name;
        public string Value => Toggle.isOn ? true.ToString() : null;

        protected Toggle Toggle { get; set; }

        protected virtual void Awake()
        {
            Toggle = GetComponent<Toggle>();
        }

        public virtual void Initialize() { }
        public virtual void Initialize(string value)
        {
            if (bool.TryParse(value, out var boolVal))
                Toggle.isOn = boolVal;
        }
    }
}