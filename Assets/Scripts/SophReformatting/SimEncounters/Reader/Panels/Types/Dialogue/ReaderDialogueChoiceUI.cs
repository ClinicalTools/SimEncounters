using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueChoiceUI : BaseReaderPanelUI
    {
        public event Action Completed;

        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private Transform optionsParent;
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }

        [SerializeField] private ReaderDialogueChoiceOptionUI choicePrefab;
        public ReaderDialogueChoiceOptionUI OptionPrefab { get => choicePrefab; set => choicePrefab = value; }

        [SerializeField] private Transform feedbackParent;
        public Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }

        [SerializeField] private GameObject instructions;
        public virtual GameObject Instructions { get => instructions; set => instructions = value; }

        [SerializeField] private Button showOptionsButton;
        public Button ShowOptionsButton { get => showOptionsButton; set => showOptionsButton = value; }

        [SerializeField] private ToggleGroup optionGroup;
        public ToggleGroup OptionGroup { get => optionGroup; set => optionGroup = value; }

        protected List<ReaderDialogueChoiceOptionUI> Options { get; private set; }

        protected ReaderPanelCreator ReaderPanelCreator { get; set; }

        public override void Initialize(ReaderScene reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);

            ReaderPanelCreator = new ReaderPanelCreator(reader, OptionsParent);

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            valueFieldInitializer.InitializePanelValueFields(gameObject, keyedPanel.Value);

            Options = DeserializeChildren(keyedPanel.Value.ChildPanels);

            ShowOptionsButton.onClick.AddListener(ShowOptions);
        }
        protected virtual List<ReaderDialogueChoiceOptionUI> DeserializeChildren(OrderedCollection<Panel> panels)
        {
            var options = new List<ReaderDialogueChoiceOptionUI>();
            foreach (var panel in panels)
                options.Add(CreateOption(panel));

            return options;
        }
        protected virtual ReaderDialogueChoiceOptionUI CreateOption(KeyValuePair<string, Panel> panel)
        {
            var option = ReaderPanelCreator.Deserialize(panel, OptionPrefab);
            option.CorrectlySelected += CorrectlySelected;
            option.SetGroup(OptionGroup);
            option.Feedback.transform.SetParent(FeedbackParent);
            return option;
        }
        protected virtual void CorrectlySelected(ReaderDialogueChoiceOptionUI panel)
        {
            foreach (var option in Options)
                if (option != panel)
                    option.gameObject.SetActive(false);

            Instructions.SetActive(false);
            ShowOptionsButton.gameObject.SetActive(true);

            Completed?.Invoke();
        }
        protected virtual void ShowOptions()
        {
            foreach (var option in Options)
                option.gameObject.SetActive(true);

            Instructions.SetActive(true);
            ShowOptionsButton.gameObject.SetActive(false);
        }
    }
}