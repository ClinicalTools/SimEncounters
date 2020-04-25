using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueChoiceDisplay : IReaderPanelDisplay
    {
        public event Action Completed;

        protected List<ReaderDialogueChoiceOptionDisplay> Options { get; set; }

        protected ReaderScene Reader { get; }
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected ReaderDialogueChoice ChoiceUI { get; }

        public  ReaderDialogueChoiceDisplay(ReaderScene reader, ReaderDialogueChoice choiceUI)
        {
            Reader = reader;
            ReaderPanelCreator = new ReaderPanelCreator(reader, null);
            ChoiceUI = choiceUI;

            ChoiceUI.ShowOptionsButton.onClick.AddListener(ShowOptions);
        }

        public void Display(KeyValuePair<string, Panel> keyedPanel)
        {
            Reader.ValueFieldInitializer.InitializePanelValueFields(ChoiceUI.gameObject, keyedPanel.Value);
            Options = DeserializeChildren(keyedPanel.Value.ChildPanels);
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
            return null;
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