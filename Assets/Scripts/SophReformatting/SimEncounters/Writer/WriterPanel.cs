using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPanel : IComparable<WriterPanel>
    {
        protected EncounterWriter Writer { get; }
        protected WriterPanelUI PanelUI { get; }
        protected IValueField[] ValueFields { get; }
        public string Key { get; }
        public Panel Panel { get; }
        public int Index { get; protected set; }
        public int UpdateIndex() => Index = PanelUI.transform.GetSiblingIndex();

        public WriterPanel(EncounterWriter writer, WriterPanelUI panelUI, KeyValuePair<string, Panel> panel)
        {
            Writer = writer;
            PanelUI = panelUI;
            Key = panel.Key;
            Panel = panel.Value;
            ValueFields = InitializePanelValueFields(writer, panelUI, panel.Value);
        }

        public WriterPanel(EncounterWriter writer, WriterPanelUI panelUI, Panel panel)
        {
            Writer = writer;
            PanelUI = panelUI;
            Panel = panel;
            ValueFields = InitializePanelValueFields(writer, panelUI, panel);
        }

        public IValueField[] InitializePanelValueFields(EncounterWriter writer, WriterPanelUI panelUI, Panel panel)
        {
            var valueFields = panelUI.GetComponentsInChildren<IValueField>();
            foreach (var valueField in valueFields) {
                if (panel.Data.ContainsKey(valueField.Name))
                    valueField.Initialize(panel.Data[valueField.Name]);
                else
                    valueField.Initialize();
            }

            return valueFields;
        }

        public void Serialize()
        {
            Panel.Data.Clear();
            foreach (var valueField in ValueFields) {
                if (!string.IsNullOrEmpty(valueField.Value))
                    Panel.Data.Add(valueField.Name, valueField.Value);
            }
        }

        public int CompareTo(WriterPanel other) => Index.CompareTo(other.Index);
    }
}