using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderLabelField2 : BaseValueField
    {
        public override string Name => name;
        public override string Value => Label.text;

        public List<GameObject> ControlledObjects { get => controlledObjects; set => controlledObjects = value; }
        [SerializeField] private List<GameObject> controlledObjects;

        public string Prefix { get => prefix; set => prefix = value; }
        [SerializeField] private string prefix;
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
        protected ISelectedListener<Panel> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<Panel> panelSelectedListener) 
            => PanelSelectedListener = panelSelectedListener;


        protected virtual void Start() => PanelSelectedListener.AddSelectedListener(OnPanelSelected);
        protected virtual void OnDestroy() => PanelSelectedListener.RemoveSelectedListener(OnPanelSelected);
        protected virtual void OnPanelSelected(object sender, Panel panel)
        {
            if (!panel.Values.ContainsKey(Name)) {
                HideControlledObjects();
                return;
            }

            var value = panel.Values[Name];
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
            foreach (var controlledObject in ControlledObjects)
                controlledObject.SetActive(false);
        }
    }
}