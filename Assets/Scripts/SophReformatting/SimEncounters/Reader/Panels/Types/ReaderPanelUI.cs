using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelUI : BaseReaderPanelUI
    {
        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private Transform childrenParent;
        public virtual Transform ChildrenParent { get => childrenParent; set => childrenParent = value; }

        [SerializeField] private List<ReaderPanelUI> childPanelOptions;
        public virtual List<ReaderPanelUI> ChildPanelOptions { get => childPanelOptions; set => childPanelOptions = value; }

        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }
        protected List<ReaderPanelUI> ChildPanels { get; set; }
        protected IValueField[] ValueFields { get; set; }

        public override void Initialize(ReaderScene reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);
            var panel = keyedPanel.Value;

            CreatePinButtons(reader, panel);

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            ValueFields = valueFieldInitializer.InitializePanelValueFields(gameObject, panel);

            ReaderPanelCreator = new ReaderPanelCreator(reader, ChildrenParent);
            ChildPanels = ReaderPanelCreator.Deserialize(panel.ChildPanels, ChildPanelOptions);
        }

        protected virtual ReaderPinsGroup CreatePinButtons(ReaderScene reader, Panel panel) => reader.Pins.CreateButtons(panel.Pins, transform);

    }
}