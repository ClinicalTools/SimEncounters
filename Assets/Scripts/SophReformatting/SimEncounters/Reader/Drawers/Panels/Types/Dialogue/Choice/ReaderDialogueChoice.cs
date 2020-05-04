using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueChoice : BaseReaderDialogueChoice
    {
        public Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }
        [SerializeField] private Transform feedbackParent;
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
            Completed?.Invoke();
        }
    }
}