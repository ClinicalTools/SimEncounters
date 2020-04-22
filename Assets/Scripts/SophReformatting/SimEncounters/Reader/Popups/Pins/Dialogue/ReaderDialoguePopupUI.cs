using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialoguePopupUI : UserDialoguePinDrawer
    {
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }

        [SerializeField] private ReaderDialogueEntryUI dialogueEntryLeft;
        public ReaderDialogueEntryUI DialogueEntryLeft { get => dialogueEntryLeft; set => dialogueEntryLeft = value; }

        [SerializeField] private ReaderDialogueEntryUI dialogueEntryRight;
        public ReaderDialogueEntryUI DialogueEntryRight { get => dialogueEntryRight; set => dialogueEntryRight = value; }

        [SerializeField] private ReaderDialogueChoiceUI dialogueChoice;
        public ReaderDialogueChoiceUI DialogueChoice { get => dialogueChoice; set => dialogueChoice = value; }

        [SerializeField] private Transform panelsParent;
        public Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public override void Display(UserDialoguePin dialoguePin)
        {
            gameObject.SetActive(true);
            DeserializeChildren(dialoguePin.GetPanels());
        }

        private int lastIndex = -1;
        protected virtual void DeserializeChildren(List<UserPanel> panels, int startIndex = 0)
        {
            if (startIndex <= lastIndex)
                return;

            for (lastIndex = startIndex; lastIndex < panels.Count; lastIndex++) {
                var panel = panels[lastIndex];
                if (!panel.Data.Type.Contains("DialogueEntry")) {
                    CreateChoice(panels, lastIndex);
                    return;
                }

                CreateEntry(panel);
            }
        }

        private const string characterNameKey = "characterName";
        private const string providerName = "Provider";
        protected virtual ReaderDialogueEntryUI CreateEntry(UserPanel panel)
        {
            ReaderDialogueEntryUI entryPrefab;
            if (panel.Data.Data.ContainsKey(characterNameKey) && panel.Data.Data[characterNameKey] == providerName)
                entryPrefab = DialogueEntryRight;
            else
                entryPrefab = DialogueEntryLeft;

            var panelDisplay = Instantiate(entryPrefab, PanelsParent);
            panelDisplay.Display(panel);
            return panelDisplay;
        }

        protected virtual ReaderDialogueChoiceUI CreateChoice(List<UserPanel> panels, int panelIndex)
        {
            var panelDisplay = Instantiate(DialogueChoice, PanelsParent);
            panelDisplay.Display(panels[panelIndex]);

            panelDisplay.Completed += () => DeserializeChildren(panels, panelIndex + 1);
            return panelDisplay;
        }
    }
}