using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelSelectableValue : BaseValueField
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
        private string value;
        public override string Value => value;


        public UnityEvent Selected;

        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(PanelSelectedListener, PanelSelectedListener.CurrentValue);
        }

        protected virtual void OnDestroy() => PanelSelectedListener.Selected -= OnPanelSelected;
        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs eventArgs)
        {
            if (!eventArgs.Panel.Values.ContainsKey(Name)) {
                this.value = null;
                return;
            }

            var value = eventArgs.Panel.Values[Name];
            SetValue(value);
        }


        public override void Initialize() { }
        public override void Initialize(string value)
        {
            SetValue(value);
        }

        protected virtual void SetValue(string value)
        {
            if (value == null) {
                this.value = null;
                return;
            }

            this.value = value.Trim();

            if (this.value == "1" || this.value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                Selected?.Invoke();
        }
    }
    [Serializable]
    public class PanelToggleText
    {
        public string ValueName { get => valueName; set => valueName = value; }
        [SerializeField] private string valueName;

        public string Text { get => text; set => text = value; }
        [SerializeField] private string text;
    }
}