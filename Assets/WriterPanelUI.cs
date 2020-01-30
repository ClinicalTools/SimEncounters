using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPanelUI : MonoBehaviour, IComparable<WriterPanelUI>
    {
        [SerializeField] private PinsUI pinsUI;
        public PinsUI PinsUI { get => pinsUI; set => pinsUI = value; }

        [SerializeField] private Button deleteButton;
        public Button DeleteButton { get => deleteButton; set => deleteButton = value; }

        public string Key { get; set; }
        public Panel Panel { get; protected set; }
        public int Index { get; protected set; }
        public int UpdateIndex() => Index = transform.GetSiblingIndex();
        protected IValueField[] ValueFields { get; set; }


        public virtual void Initialize(EncounterWriter writer, Panel panel)
        {
            Panel = panel;
            ValueFields = InitializePanelValueFields(writer, panel);
        }

        public IValueField[] InitializePanelValueFields(EncounterWriter writer, Panel panel)
        {
            var valueFields = GetComponentsInChildren<IValueField>();
            foreach (var valueField in valueFields) {
                if (panel.Data.ContainsKey(valueField.Name))
                    valueField.Initialize(panel.Data[valueField.Name]);
                else
                    valueField.Initialize();
            }

            return valueFields;
        }

        public virtual void Serialize()
        {
            Panel.Data.Clear();
            foreach (var valueField in ValueFields) {
                if (!string.IsNullOrEmpty(valueField.Value))
                    Panel.Data.Add(valueField.Name, valueField.Value);
            }
        }

        public int CompareTo(WriterPanelUI other) => Index.CompareTo(other.Index);
    }
}