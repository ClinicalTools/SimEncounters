using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelLabel : BaseValueField
    {
        [SerializeField] private string valueName;
        public override string Name
        {
            get {
                if (!string.IsNullOrWhiteSpace(valueName))
                    return valueName;
                return name;
            }
        }
        public override string Value => Label.text;

        public List<GameObject> ControlledObjects { get => controlledObjects; set => controlledObjects = value; }
        [SerializeField] private List<GameObject> controlledObjects;

        public string Prefix { get => prefix; set => prefix = value; }
        [Multiline] [SerializeField] private string prefix;
        public bool Trim { get => trim; set => trim = value; }
        [SerializeField] private bool trim;

        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label
        {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.AddSelectedListener(OnPanelSelected);
        }

        protected virtual void OnDestroy() => PanelSelectedListener.RemoveSelectedListener(OnPanelSelected);
        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs eventArgs)
        {
            if (!eventArgs.Panel.Values.ContainsKey(Name)) {
                HideControlledObjects();
                return;
            }

            var value = eventArgs.Panel.Values[Name];
            SetText(value);

            if (string.IsNullOrWhiteSpace(value))
                HideControlledObjects();
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
            if (value != null) {
                if (Trim)
                    value = value.Trim();

                text += value;
            }
            Label.text = text;
        }

        protected virtual void HideControlledObjects()
        {
            foreach (var controlledObject in ControlledObjects) {
                if (controlledObject == null)
                    Debug.LogError(gameObject.name);
                else
                    controlledObject.SetActive(false);
            }
        }
    }
}