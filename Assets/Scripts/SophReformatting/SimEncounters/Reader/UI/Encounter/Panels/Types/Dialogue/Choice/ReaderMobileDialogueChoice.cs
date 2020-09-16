using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileDialogueChoice : BaseReaderDialogueChoice
    {
        public BaseDialogueOptionsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseDialogueOptionsDrawer childPanelCreator;
        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }
        [SerializeField] private GameObject completedObject;
        public virtual GameObject IncompletedObject { get => incompletedObject; set => incompletedObject = value; }
        [SerializeField] private GameObject incompletedObject;

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

        private void Option_CorrectlySelected(BaseReaderDialogueOption selectedOption)
        {
            CompletedObject.gameObject.SetActive(true);
            IncompletedObject.gameObject.SetActive(false);
            selectedOption.transform.SetParent(CompletedObject.transform);
            Completed?.Invoke();
        }
    }
}