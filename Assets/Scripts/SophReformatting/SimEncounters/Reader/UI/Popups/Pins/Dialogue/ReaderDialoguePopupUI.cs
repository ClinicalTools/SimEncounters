using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderDialoguePopupUI : BaseUserDialoguePinDrawer, ISelector<UserDialoguePin>
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public BaseReaderPanelsCreator PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseReaderPanelsCreator panelCreator;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;
        public ScrollRectGradient ScrollGradient { get => scrollGradient; set => scrollGradient = value; }
        [SerializeField] private ScrollRectGradient scrollGradient;

        protected List<BaseReaderPanel> ReaderPanels { get; set; }

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
        }

        public override void Display(UserDialoguePin dialoguePin)
        {
            Select(this, dialoguePin);
            SetPanelsAsRead(dialoguePin.GetPanels());

            gameObject.SetActive(true);

            ReaderPanels = PanelCreator.DrawChildPanels(dialoguePin.GetPanels());

            ScrollRect.normalizedPosition = Vector2.one;
            if (ScrollGradient != null)
                ScrollGradient.ResetGradients();
        }

        protected virtual void Hide()
        {
            if (ReaderPanels != null) {
                foreach (var readerPanel in ReaderPanels)
                    Destroy(readerPanel.gameObject);
            }

            gameObject.SetActive(false);
        }

        protected virtual void SetPanelsAsRead(IEnumerable<UserPanel> panels)
        {
            foreach (var panel in panels) {
                var childPanels = new List<UserPanel>(panel.GetChildPanels());
                if (childPanels.Count > 0)
                    SetPanelsAsRead(childPanels);
                else
                    panel.SetRead(true);
            }
        }

        protected ISelector<UserDialoguePin> Selector { get; } = new Selector<UserDialoguePin>();
        public UserDialoguePin CurrentValue => Selector.CurrentValue;
        public void Select(object sender, UserDialoguePin eventArgs) => Selector.Select(sender, eventArgs);
        public void AddSelectedListener(SelectedHandler<UserDialoguePin> handler) => Selector.AddSelectedListener(handler);
        public void RemoveSelectedListener(SelectedHandler<UserDialoguePin> handler) => Selector.RemoveSelectedListener(handler);
    }
}