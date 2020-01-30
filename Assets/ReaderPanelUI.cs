using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelUI : MonoBehaviour
    {
        [SerializeField] private string type;
        public virtual string Type { get => type; set => type = value; }

        [SerializeField] private Transform childrenParent;
        public virtual Transform ChildrenParent { get => childrenParent; set => childrenParent = value; }

        [SerializeField] private List<ReaderPanelUI> childPanelOptions;
        public virtual List<ReaderPanelUI> ChildPanelOptions { get => childPanelOptions; set => childPanelOptions = value; }


        protected string Key { get; private set; }
        protected Panel Panel { get; private set; }
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }
        protected List<ReaderPanelUI> ChildPanels { get; set; }
        protected IValueField[] ValueFields { get; set; }

        public void Initialize(EncounterReader reader, KeyValuePair<string, Panel> keyedPanel)
        {
            Key = keyedPanel.Key;
            Panel = keyedPanel.Value;
            ReaderPanelCreator = new ReaderPanelCreator(reader, ChildrenParent);

            if (Panel.ChildPanels.Count > 0)
                ChildPanels = ReaderPanelCreator.Deserialize(Panel.ChildPanels, ChildPanelOptions);

            ValueFields = InitializePanelValueFields(reader, Panel);
        }

        public IValueField[] InitializePanelValueFields(EncounterReader reader, Panel panel)
        {
            var valueFields = GetComponentsInChildren<IValueField>();
            foreach (var valueField in valueFields) {
                if (panel.Data.ContainsKey(valueField.Name))
                    valueField.Initialize(panel.Data[valueField.Name]);
                else
                    valueField.Initialize();
            }
            var readerValueFields = GetComponentsInChildren<IReaderValueField>();
            foreach (var valueField in readerValueFields) {
                if (panel.Data.ContainsKey(valueField.Name))
                    valueField.Initialize(reader, panel.Data[valueField.Name]);
                else
                    valueField.Initialize(reader);
            }

            return valueFields;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}