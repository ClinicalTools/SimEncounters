
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderDialogueChoice : BaseReaderDialogueChoice
    {
        public virtual GameObject Instructions { get => instructions; set => instructions = value; }
        [SerializeField] private GameObject instructions;
        public Button ShowOptionsButton { get => showOptionsButton; set => showOptionsButton = value; }
        [SerializeField] private Button showOptionsButton;
        public BaseDialogueOptionsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseDialogueOptionsDrawer childPanelCreator;


        public override event Action Completed;

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

        protected List<BaseReaderDialogueOption> Options { get; set; }

        protected virtual void Awake()
        {
            ShowOptionsButton.onClick.AddListener(ShowOptions);
        }

        public override void Display(UserPanel panel)
        {
            BasicPanelDrawer.Display(panel, transform, transform);
            Options = ChildPanelCreator.DrawChildPanels(panel.GetChildPanels());

            foreach (var option in Options)
                option.CorrectlySelected += Option_CorrectlySelected;
        }

        private void Option_CorrectlySelected(ReaderDialogueOption selectedOption)
        {
            foreach (var option in Options) {
                if (option != selectedOption)
                    option.gameObject.SetActive(false);
            }
            Instructions.gameObject.SetActive(false);
            ShowOptionsButton.gameObject.SetActive(true);
            Completed?.Invoke();
        }

        protected virtual void ShowOptions()
        {
            foreach (var option in Options) {
                option.gameObject.SetActive(true);
                option.CloseFeedback();
            }
            Instructions.gameObject.SetActive(true);
            ShowOptionsButton.gameObject.SetActive(false);
        }
    }
}