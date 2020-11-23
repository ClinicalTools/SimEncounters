using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownValueField : MonoBehaviour, IPanelField
    {
        public virtual string Name => name;
        public virtual string Value => (Dropdown.value >= 0) ? Dropdown.options[Dropdown.value].text : null;
        
        private TMP_Dropdown dropdown;
        protected TMP_Dropdown Dropdown {
            get {
                if (dropdown == null)
                    dropdown = GetComponent<TMP_Dropdown>();
                return dropdown;
            }
        }

        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
        }

        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            if (!values.ContainsKey(Name)) {
                Dropdown.value = 0;
                return;
            }

            var value = values[Name];
            for (var i = 0; i < Dropdown.options.Count; i++) {
                if (Dropdown.options[i].text != value)
                    continue;

                Dropdown.value = i;
                break;
            }
        }
    }
}