using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSingleChildTypePanelUI : WriterPanelUI
    {
        [SerializeField] private Button addChildButton;
        public Button AddChildButton { get => addChildButton; set => addChildButton = value; }

        [SerializeField] private Transform childrenParent;
        public virtual Transform ChildrenParent { get => childrenParent; set => childrenParent = value; }

        [SerializeField] private WriterPanelUI childPanelPrefab;
        public virtual WriterPanelUI ChildPanelPrefab { get => childPanelPrefab; set => childPanelPrefab = value; }


        protected WriterPanelCreator WriterPanelCreator { get; private set; }
        protected List<WriterPanelUI> WriterPanels { get; set; }
        protected PanelGroupSerializer PanelGroupSerializer { get; } = new PanelGroupSerializer();


        public override void Initialize(EncounterWriter writer, Panel panel)
        {
            base.Initialize(writer, panel);

            WriterPanelCreator = new WriterPanelCreator(writer, ChildrenParent);

            if (panel.ChildPanels.Count > 0)
                WriterPanels = WriterPanelCreator.Deserialize(panel.ChildPanels, ChildPanelPrefab);
            else
                WriterPanels = new List<WriterPanelUI>();

            AddChildButton.onClick.AddListener(() => AddChildPanel());
        }

        protected void AddChildPanel()
        {
            var writerPanel = WriterPanelCreator.AddPanel(ChildPanelPrefab);
            WriterPanels.Add(writerPanel);
        }

        public override void Serialize()
        {
            base.Serialize();

            PanelGroupSerializer.Serialize(WriterPanels, Panel.ChildPanels);
        }
    }
}