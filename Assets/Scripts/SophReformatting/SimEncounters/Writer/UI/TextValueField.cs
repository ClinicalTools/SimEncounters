using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TextValueField : MonoBehaviour, IValueField
    {
        public string Name => name;
        public string Value => InputField.text;

        protected TMP_InputField InputField { get; set; }

        protected virtual void Awake()
        {
            InputField = GetComponent<TMP_InputField>();
        }

        public void Initialize() { }
        public void Initialize(string value)
        {
            InputField.text = value;
        }
    }
}