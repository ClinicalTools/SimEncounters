using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueChoiceDisplay : IReaderPanelDisplay
    {
        public event Action Completed;

        protected List<ReaderDialogueChoiceOptionDisplay> Options { get; }

        protected ReaderScene Reader { get; }
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected ReaderDialogueChoiceUI ChoiceUI { get; }

        public  ReaderDialogueChoiceDisplay(ReaderScene reader, ReaderDialogueChoiceUI choiceUI, KeyValuePair<string, Panel> keyedPanel)
        {
            ReaderPanelCreator = new ReaderPanelCreator(reader, choiceUI.OptionsParent);
            ChoiceUI = choiceUI;

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            valueFieldInitializer.InitializePanelValueFields(ChoiceUI.gameObject, keyedPanel.Value);

            Options = DeserializeChildren(keyedPanel.Value.ChildPanels);

            ChoiceUI.ShowOptionsButton.onClick.AddListener(ShowOptions);
        }
        protected virtual List<ReaderDialogueChoiceOptionDisplay> DeserializeChildren(OrderedCollection<Panel> panels)
        {
            var options = new List<ReaderDialogueChoiceOptionDisplay>();
            foreach (var panel in panels)
                options.Add(CreateOption(panel));

            return options;
        }
        protected virtual ReaderDialogueChoiceOptionDisplay CreateOption(KeyValuePair<string, Panel> panel)
        {
            var optionUI = ReaderPanelCreator.Deserialize(ChoiceUI.OptionPrefab);
            var option = Reader.PanelDisplayFactory.CreateDialogueChoiceOptionPanel(optionUI, panel);

            option.CorrectlySelected += CorrectlySelected;
            option.SetGroup(ChoiceUI.OptionGroup);
            option.Feedback.SetParent(ChoiceUI.FeedbackParent);
            return option;
        }
        protected virtual void CorrectlySelected(ReaderDialogueChoiceOptionDisplay panel)
        {
            foreach (var option in Options)
                if (option != panel)
                    option.SetActive(false);

            ChoiceUI.Instructions.SetActive(false);
            ChoiceUI.ShowOptionsButton.gameObject.SetActive(true);

            Completed?.Invoke();
        }
        protected virtual void ShowOptions()
        {
            foreach (var option in Options)
                option.SetActive(true);

            ChoiceUI.Instructions.SetActive(true);
            ChoiceUI.ShowOptionsButton.gameObject.SetActive(false);
        }

    }
}