
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using ClinicalTools.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderOrderableGroupPanelUI : BaseReaderPanel
    {
        public virtual BaseOrderablePanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseOrderablePanelsDrawer childPanelCreator;
        public BaseRearrangeableGroup DraggableGroupUI { get => draggableGroup; set => draggableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup draggableGroup;
        public virtual GameObject FeedbackObject { get => feedbackObject; set => feedbackObject = value; }
        [SerializeField] private GameObject feedbackObject;

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        protected virtual FeedbackColorInfo FeedbackColorInfo { get; set; }
        [Inject] public virtual void Inject(FeedbackColorInfo feedbackColorInfo, IReaderPanelDisplay basicReaderPanel)
        {
            FeedbackColorInfo = feedbackColorInfo; 
            BasicPanelDrawer = basicReaderPanel;
        }

        protected virtual void Awake()
        {
            DraggableGroupUI.Rearranged += OrderChanged;
        }

        protected List<UserPanel> CorrectPanelOrder { get; set; }
        public override void Display(UserPanel userPanel)
        {
            BasicPanelDrawer.Display(userPanel, transform, transform);

            CorrectPanelOrder = new List<UserPanel>(userPanel.GetChildPanels());
            var readerPanels = ChildPanelCreator.DrawChildPanels(CorrectPanelOrder);
            foreach (var readerPanel in readerPanels)
                DraggableGroupUI.Add(readerPanel);
        }
        protected virtual void OnDestroy() => BasicPanelDrawer.Dispose();

        private void OrderChanged(object sender, RearrangedEventArgs2 rearrangedArgs)
        {
            if (CorrectPanelOrder == null)
                return;

            var allCorrect = true;
            for (int i = 0; i < CorrectPanelOrder.Count; i++) {
                var actualIndex = IndexOfPanel(CorrectPanelOrder[i], rearrangedArgs.CurrentOrder);
                var distanceFromCorrect = Math.Abs(actualIndex - i);
                var optionType = GetOptionType(distanceFromCorrect);
                if (optionType != OptionType.Correct)
                    allCorrect = false;

                var orderableItem = rearrangedArgs.CurrentOrder[actualIndex] as BaseReaderOrderableItemPanel;
                orderableItem.SetColor(FeedbackColorInfo.GetColor(optionType));
            }

            FeedbackObject.SetActive(allCorrect);
        }

        private int IndexOfPanel(UserPanel userPanel, List<IDraggable> draggableObjects)
        {
            for (int i = 0; i < draggableObjects.Count; i++) {
                var orderableItem = draggableObjects[i] as BaseReaderOrderableItemPanel;
                if (orderableItem.CurrentPanel == userPanel)
                    return i;
            }
            return -1;
        }

        private OptionType GetOptionType(int distance)
        {
            if (distance == 0)
                return OptionType.Correct;
            else if (distance == 1)
                return OptionType.PartiallyCorrect;
            else
                return OptionType.Incorrect;
        }
    }
}