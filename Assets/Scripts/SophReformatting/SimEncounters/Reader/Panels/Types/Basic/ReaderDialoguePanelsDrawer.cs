using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialoguePanelsDrawer : BaseChildPanelsDrawer
    {
        [SerializeField] private BaseReaderPanelUI dialogueEntryLeft;
        public BaseReaderPanelUI DialogueEntryLeft { get => dialogueEntryLeft; set => dialogueEntryLeft = value; }

        [SerializeField] private BaseReaderPanelUI dialogueEntryRight;
        public BaseReaderPanelUI DialogueEntryRight { get => dialogueEntryRight; set => dialogueEntryRight = value; }

        [SerializeField] private BaseReaderDialogueChoice dialogueChoice;
        public BaseReaderDialogueChoice DialogueChoice { get => dialogueChoice; set => dialogueChoice = value; }

        public override List<BaseReaderPanelUI> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var panels = new List<BaseReaderPanelUI>();
            var childList = new List<UserPanel>(childPanels);
            DeserializeChildren(panels, childList, 0);

            return panels;
        }

        protected virtual void DeserializeChildren(List<BaseReaderPanelUI> readerPanels, List<UserPanel> panels, int startIndex)
        {
            if (startIndex < readerPanels.Count)
                return;

            for (var i = startIndex; i < panels.Count; i++) {
                var panel = panels[i];
                if (!panel.Data.Type.Contains("DialogueEntry")) {
                    readerPanels.Add(CreateChoice(readerPanels, panels, i));
                    return;
                }

                readerPanels.Add(CreateEntry(panel));
            }
        }

        private const string characterNameKey = "characterName";
        private const string providerName = "Provider";
        protected virtual BaseReaderPanelUI CreateEntry(UserPanel panel)
        {
            BaseReaderPanelUI entryPrefab;
            if (panel.Data.Data.ContainsKey(characterNameKey) && panel.Data.Data[characterNameKey] == providerName)
                entryPrefab = DialogueEntryRight;
            else
                entryPrefab = DialogueEntryLeft;

            var panelDisplay = Instantiate(entryPrefab, transform);
            panelDisplay.Display(panel);
            return panelDisplay;
        }

        protected virtual BaseReaderDialogueChoice CreateChoice(List<BaseReaderPanelUI> readerPanels, List<UserPanel> panels, int panelIndex)
        {
            var panelDisplay = Instantiate(DialogueChoice, transform);
            panelDisplay.Display(panels[panelIndex]);

            panelDisplay.Completed += () => DeserializeChildren(readerPanels, panels, panelIndex + 1);
            return panelDisplay;
        }
    }
}