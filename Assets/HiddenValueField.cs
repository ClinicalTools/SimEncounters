using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class HiddenValueField : MonoBehaviour, IValueField
    {
        public string Name => name;

        public string Value { get; set; }

        public void Initialize() { }

        public void Initialize(string value) => Value = value;
    }
}