using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderLabelField : BaseValueField
    {
        public override string Name => name;
        public override string Value => Label.text;

        public List<GameObject> ControlledObjects { get => controlledObjects; set => controlledObjects = value; }
        [SerializeField] private List<GameObject> controlledObjects;

        public string Prefix { get => prefix; set => prefix = value; }
        [SerializeField] private string prefix;

        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

        public override void Initialize() => HideControlledObjects();
        public override void Initialize(string value)
        {
            SetText(value);

            if (string.IsNullOrWhiteSpace(value))
                HideControlledObjects();
        }

        protected virtual void SetText(string value)
        {
            var text = "";
            if (Prefix != null)
                text += Prefix;
            if (value != null)
                text += value;
            Label.text = text;
        }

        protected virtual void HideControlledObjects()
        {
            foreach (var controlledObject in ControlledObjects)
                controlledObject.SetActive(false);
        }
    }
}