using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterDialoguePanelsDrawer : BaseWriterPanelsDrawer
    {
        public Button PatientEntryButton { get => patientEntryButton; set => patientEntryButton = value; }
        [SerializeField] private Button patientEntryButton;
        public Button ProviderEntryButton { get => providerEntryButton; set => providerEntryButton = value; }
        [SerializeField] private Button providerEntryButton;
        public Button InstructorEntryButton { get => instructorEntryButton; set => instructorEntryButton = value; }
        [SerializeField] private Button instructorEntryButton;

        public Button ChoiceButton { get => choiceButton; set => choiceButton = value; }
        [SerializeField] private Button choiceButton;
        public BaseRearrangeableGroup ReorderableGroup { get => reorderableGroup; set => reorderableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup reorderableGroup;
        public BaseWriterAddablePanel EntryPrefab { get => entryPrefab; set => entryPrefab = value; }
        [SerializeField] private BaseWriterAddablePanel entryPrefab;
        public BaseWriterAddablePanel ChoicePrefab { get => choicePrefab; set => choicePrefab = value; }
        [SerializeField] private BaseWriterAddablePanel choicePrefab;

        protected virtual OrderedCollection<BaseWriterPanel> WriterPanels { get; set; } = new OrderedCollection<BaseWriterPanel>();

        protected virtual List<BaseWriterAddablePanel> PanelPrefabs { get; } = new List<BaseWriterAddablePanel>();
        protected virtual void Awake()
        {
            PanelPrefabs.Add(EntryPrefab);
            PanelPrefabs.Add(ChoicePrefab);
            PatientEntryButton.onClick.AddListener(AddPatientEntry);
            ProviderEntryButton.onClick.AddListener(AddProviderEntry);
            InstructorEntryButton.onClick.AddListener(AddInstructorEntry);
            ChoiceButton.onClick.AddListener(AddChoice);
        }

        private void AddPatientEntry() => CreateEntryPanel("Patient", new Color(0.106f, 0.722f, 0.059f));
        private void AddProviderEntry() => CreateEntryPanel("Provider", new Color(0, 0.2509804f, 0.9568627f));
        private void AddInstructorEntry() => CreateEntryPanel("Instructor", new Color(0.569f, 0.569f, 0.569f));
        private void CreateEntryPanel(string characterName, Color characterColor)
        {
            var panel = new Panel("Entry");
            panel.Values.Add("characterName", characterName);
            panel.Values.Add("charColor", characterColor.ToString());

            var panelUI = ReorderableGroup.AddFromPrefab(EntryPrefab);
            panelUI.Display(CurrentEncounter, panel);
            WriterPanels.Add(panelUI);
        }

        private void AddChoice()
        {
            var panelUI = ReorderableGroup.AddFromPrefab(ChoicePrefab);
            panelUI.Display(CurrentEncounter);
            WriterPanels.Add(panelUI);
        }

        protected Encounter CurrentEncounter { get; set; }
        public override List<BaseWriterPanel> DrawChildPanels(Encounter encounter, OrderedCollection<Panel> childPanels)
        {
            CurrentEncounter = encounter;

            foreach (var writerPanel in WriterPanels.Values) {
                if (writerPanel != null)
                    Destroy(writerPanel.gameObject);
            }

            var blah = new ChildrenPanelManager();
            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panel in childPanels) {
                var prefab = blah.ChoosePrefab(PanelPrefabs, panel.Value);
                var panelUI = ReorderableGroup.AddFromPrefab(prefab);
                panelUI.Display(encounter, panel.Value);
                panels.Add(panelUI);
                WriterPanels.Add(panel.Key, panelUI);
            }

            return panels;
        }

        public override List<BaseWriterPanel> DrawDefaultChildPanels(Encounter encounter)
        {
            WriterPanels = new OrderedCollection<BaseWriterPanel>();
            return new List<BaseWriterPanel>();
        }

        public override OrderedCollection<Panel> SerializeChildren()
        {
            var blah = new ChildrenPanelManager();
            return blah.SerializeChildren(WriterPanels);
        }
    }
}