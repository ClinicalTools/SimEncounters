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

        [SerializeField] private BaseChildPanelsDrawer panelCreator;
        public BaseChildPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }

        protected List<BaseReaderPanelUI> ReaderPanels { get; set; }

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
        }

        public override void Display(UserDialoguePin dialoguePin)
        {
            gameObject.SetActive(true);
            ReaderPanels = PanelCreator.DrawChildPanels(dialoguePin.GetPanels());
        }

        protected virtual void Hide()
        {
            if (ReaderPanels != null) {
                foreach (var readerPanel in ReaderPanels)
                    Destroy(readerPanel.gameObject);
            }

            gameObject.SetActive(false);
        }
    }
}