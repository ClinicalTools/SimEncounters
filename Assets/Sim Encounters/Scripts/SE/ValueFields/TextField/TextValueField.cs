using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_InputField))]
    public class TextValueField : BaseValueField
    {
        public override string Name => name;
        public override string Value => InputField.text;

        private TMP_InputField inputField;
        protected TMP_InputField InputField {
            get {
                if (inputField == null)
                    inputField = GetComponent<TMP_InputField>();
                return inputField;
            }
        }

        public override void Initialize() { }
        public override void Initialize(string value) => InputField.text = value;
    }
}