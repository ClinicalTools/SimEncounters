using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class TextValueField : MonoBehaviour, IValueField
    {
        public string Name => name;
        public string Value => InputField.text;

        private TMP_InputField inputField;
        protected TMP_InputField InputField {
            get {
                if (inputField == null)
                    inputField = GetComponent<TMP_InputField>();
                return inputField;
            }
        }

        public void Initialize() { }
        public void Initialize(string value)
        {
            InputField.text = value;
        }
    }
}