using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialoguePopup : ReaderPopup
    {
        protected ReaderScene Reader { get; }
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected ReaderDialoguePopupUI DialoguePopupUI { get; }

        public ReaderDialoguePopup(ReaderScene reader, ReaderDialoguePopupUI dialoguePopupUI, DialoguePin pin) : base(reader, dialoguePopupUI)
        {
            Reader = reader;
            DialoguePopupUI = dialoguePopupUI;

            ReaderPanelCreator = new ReaderPanelCreator(reader, dialoguePopupUI.PanelsParent);

            DeserializeChildren(pin.Conversation);
        }

        private int lastIndex = -1;
        public virtual void DeserializeChildren(OrderedCollection<Panel> panels, int startIndex = 0)
        {
            if (startIndex <= lastIndex)
                return;

            for (lastIndex = startIndex; lastIndex < panels.Count; lastIndex++) {
                var panel = panels[lastIndex]; 
                if (!panel.Value.Type.Contains("DialogueEntry")) {
                    CreateChoice(panels, lastIndex);
                    return;
                }

                CreateEntry(panel);
            }
        }

        private const string characterNameKey = "characterName";
        private const string providerName = "Provider";
        protected virtual ReaderDialogueEntryUI CreateEntry(KeyValuePair<string, Panel> panel)
        {
            ReaderDialogueEntryUI entryPrefab;
            if (panel.Value.Data.ContainsKey(characterNameKey) && panel.Value.Data[characterNameKey] == providerName)
                entryPrefab = DialoguePopupUI.DialogueEntryRight;
            else
                entryPrefab = DialoguePopupUI.DialogueEntryLeft;

            return ReaderPanelCreator.Deserialize(panel, entryPrefab); 
        }

        protected virtual ReaderDialogueChoiceUI CreateChoice(OrderedCollection<Panel> panels, int panelIndex)
        {
            var choice = ReaderPanelCreator.Deserialize(panels[panelIndex], DialoguePopupUI.DialogueChoice);
            choice.Completed += () => DeserializeChildren(panels, panelIndex + 1);
            return choice;
        }
    }
}