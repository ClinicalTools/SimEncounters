
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialoguePanelsCreator : BaseReaderPanelsCreator
    {
        public BaseReaderPanel DialogueEntryLeft { get => dialogueEntryLeft; set => dialogueEntryLeft = value; }
        [SerializeField] private BaseReaderPanel dialogueEntryLeft;
        public BaseReaderPanel DialogueEntryRight { get => dialogueEntryRight; set => dialogueEntryRight = value; }
        [SerializeField] private BaseReaderPanel dialogueEntryRight;
        public BaseReaderDialogueChoice DialogueChoice { get => dialogueChoice; set => dialogueChoice = value; }
        [SerializeField] private BaseReaderDialogueChoice dialogueChoice;

        public override List<BaseReaderPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var panels = new List<BaseReaderPanel>();
            var childList = new List<UserPanel>(childPanels);
            DeserializeChildren(panels, childList, 0);

            return panels;
        }

        protected virtual void DeserializeChildren(List<BaseReaderPanel> readerPanels, List<UserPanel> panels, int startIndex)
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
        protected virtual BaseReaderPanel CreateEntry(UserPanel panel)
        {
            BaseReaderPanel entryPrefab;
            if (panel.Data.Values.ContainsKey(characterNameKey) && panel.Data.Values[characterNameKey] == providerName)
                entryPrefab = DialogueEntryRight;
            else
                entryPrefab = DialogueEntryLeft;

            var panelDisplay = Instantiate(entryPrefab, transform);
            panelDisplay.Display(panel);
            return panelDisplay;
        }

        protected virtual BaseReaderDialogueChoice CreateChoice(List<BaseReaderPanel> readerPanels, List<UserPanel> panels, int panelIndex)
        {
            var panelDisplay = Instantiate(DialogueChoice, transform);
            panelDisplay.Display(panels[panelIndex]);

            panelDisplay.Completed += () => DeserializeChildren(readerPanels, panels, panelIndex + 1);
            return panelDisplay;
        }
    }
}