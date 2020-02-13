using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderLabelField : MonoBehaviour, IValueField
    {
        public string Name => name;
        public string Value => Label.text;

        [SerializeField] private List<GameObject> controlledObjects;
        public List<GameObject> ControlledObjects { get => controlledObjects; set => controlledObjects = value; }

        [SerializeField] private string prefix;
        public string Prefix { get => prefix; set => prefix = value; }

        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

        public void Initialize()
        {
            HideControlledObjects();
        }
        public void Initialize(string value)
        {
            SetText(value);

            if (string.IsNullOrWhiteSpace(value))
                HideControlledObjects();
        }

        protected void SetText(string value)
        {
            var text = "";
            if (Prefix != null)
                text += Prefix;
            if (value != null)
                text += value;
            Label.text = text;
        }

        protected void HideControlledObjects()
        {
            foreach (var controlledObject in ControlledObjects)
                controlledObject.SetActive(false);
        }
    }
}