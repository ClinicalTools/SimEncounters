using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderDialoguePopupUI : BaseUserDialoguePinDrawer, ISelector<UserDialoguePinSelectedEventArgs>
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public BaseChildUserPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseChildUserPanelsDrawer panelCreator;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;
        public ScrollRectGradient ScrollGradient { get => scrollGradient; set => scrollGradient = value; }
        [SerializeField] private ScrollRectGradient scrollGradient;


        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
        }

        public override void Display(UserDialoguePin dialoguePin)
        {
            Select(this, new UserDialoguePinSelectedEventArgs(dialoguePin));

            gameObject.SetActive(true);

            PanelCreator.Display(dialoguePin.Panels, true);

            ScrollRect.normalizedPosition = Vector2.one;
            if (ScrollGradient != null)
                ScrollGradient.ResetGradients();
        }

        protected virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        protected ISelector<UserDialoguePinSelectedEventArgs> Selector { get; } = new Selector<UserDialoguePinSelectedEventArgs>();
        public UserDialoguePinSelectedEventArgs CurrentValue => Selector.CurrentValue;
        public void Select(object sender, UserDialoguePinSelectedEventArgs eventArgs) => Selector.Select(sender, eventArgs);
        public void AddSelectedListener(SelectedHandler<UserDialoguePinSelectedEventArgs> handler) => Selector.AddSelectedListener(handler);
        public void RemoveSelectedListener(SelectedHandler<UserDialoguePinSelectedEventArgs> handler) => Selector.RemoveSelectedListener(handler);
    }
}