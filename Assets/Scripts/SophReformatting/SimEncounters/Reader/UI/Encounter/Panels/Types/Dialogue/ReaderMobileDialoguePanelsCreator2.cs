using ClinicalTools.SimEncounters.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileDialoguePanelsCreator2 : MonoBehaviour
    {
        public Image Background { get => background; set => background = value; }
        [SerializeField] private Image background;
        public ColorType ChoiceBackgroundColor { get => choiceBackgroundColor; set => choiceBackgroundColor = value; }
        [SerializeField] private ColorType choiceBackgroundColor;
        public ReaderPanelBehaviour DialogueEntryLeft { get => dialogueEntryLeft; set => dialogueEntryLeft = value; }
        [SerializeField] private ReaderPanelBehaviour dialogueEntryLeft;
        public ReaderPanelBehaviour DialogueEntryRight { get => dialogueEntryRight; set => dialogueEntryRight = value; }
        [SerializeField] private ReaderPanelBehaviour dialogueEntryRight;
        public CompletableReaderPanelBehaviour DialogueChoice { get => dialogueChoice; set => dialogueChoice = value; }
        [SerializeField] private CompletableReaderPanelBehaviour dialogueChoice;

        protected ReaderPanelBehaviour.Factory ReaderPanelFactory { get; set; }
        protected ISelector<UserDialoguePin> PinSelector { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<UserDialoguePin> pinSelector,
            ReaderPanelBehaviour.Factory readerPanelFactory)
        {
            PinSelector = pinSelector;
            ReaderPanelFactory = readerPanelFactory;
        }
        protected virtual void Start() => PinSelector.AddSelectedListener(OnPinSelected);
        protected virtual void OnDestroy() => PinSelector.RemoveSelectedListener(OnPinSelected);

        protected virtual void OnPinSelected(object sender, UserDialoguePin dialoguePin)
            => DrawChildPanels(dialoguePin.GetPanels());

        protected virtual List<ReaderPanelBehaviour> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            var panels = new List<ReaderPanelBehaviour>();
            var childList = new List<UserPanel>(childPanels);
            DeserializeChildren(panels, childList, 0);

            return panels;
        }

        protected virtual void DeserializeChildren(List<ReaderPanelBehaviour> readerPanels, List<UserPanel> panels, int startIndex)
        {
            Background.color = Color.white;

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

        private const string CharacterNameKey = "characterName";
        private const string ProviderName = "Provider";
        protected virtual ReaderPanelBehaviour CreateEntry(UserPanel panel)
        {
            var values = panel.Data.Values;
            ReaderPanelBehaviour entryPrefab =
                (values.ContainsKey(CharacterNameKey) && values[CharacterNameKey] == ProviderName) ?
                DialogueEntryRight : DialogueEntryLeft;

            var panelDisplay = ReaderPanelFactory.Create(entryPrefab);
            panelDisplay.transform.SetParent(transform);
            panelDisplay.transform.localScale = Vector3.one;
            panelDisplay.transform.SetAsLastSibling();
            panelDisplay.Select(this, new UserPanelSelectedEventArgs(panel, true));
            return panelDisplay;
        }

        protected virtual CompletableReaderPanelBehaviour CreateChoice(List<ReaderPanelBehaviour> readerPanels, List<UserPanel> panels, int panelIndex)
        {
            Background.color = ColorManager.GetColor(ChoiceBackgroundColor);

            var panelDisplay = (CompletableReaderPanelBehaviour)ReaderPanelFactory.Create(DialogueChoice);
            panelDisplay.transform.SetParent(transform);
            panelDisplay.transform.localScale = Vector3.one;
            panelDisplay.transform.SetAsLastSibling();
            panelDisplay.Select(this, new UserPanelSelectedEventArgs(panels[panelIndex], true));

            panelDisplay.Completed += () => DeserializeChildren(readerPanels, panels, panelIndex + 1);
            return panelDisplay;
        }
    }
}