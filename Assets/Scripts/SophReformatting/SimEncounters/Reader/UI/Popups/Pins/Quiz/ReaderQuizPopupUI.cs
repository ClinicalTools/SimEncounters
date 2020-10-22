using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderQuizPopupUI : BaseUserQuizPinDrawer
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

        public override void Display(UserQuizPin quizPin)
        {
            SetPanelsAsRead(quizPin.GetPanels());

            gameObject.SetActive(true);
            ReaderPanels = PanelCreator.DrawChildPanels(quizPin.GetPanels());

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
    }
}