using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableGroupPanelUI : BaseReaderPanelUI
    {
        [SerializeField] private ChildOrderablePanelsDrawer childPanelCreator;
        public virtual ChildOrderablePanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }

        [SerializeField] private DraggableGroupUI draggableGroup;
        public DraggableGroupUI DraggableGroupUI { get => draggableGroup; set => draggableGroup = value; }

        [SerializeField] private GameObject feedbackObject;
        public virtual GameObject FeedbackObject { get => feedbackObject; set => feedbackObject = value; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }
        protected virtual FeedbackColorInfo FeedbackColorInfo { get; set; }
        [Inject]
        public virtual void Inject(FeedbackColorInfo feedbackColorInfo, BasicReaderPanelDrawer basicReaderPanel)
        {
            FeedbackColorInfo = feedbackColorInfo; 
            BasicPanelDrawer = basicReaderPanel;
        }

        protected virtual void Awake()
        {
            DraggableGroupUI.OrderChanged += OrderChanged;
        }

        protected List<UserPanel> CorrectPanelOrder { get; set; }
        public override void Display(UserPanel userPanel)
        {
            BasicPanelDrawer.Display(userPanel, transform, transform);

            CorrectPanelOrder = userPanel.GetChildPanels();
            var readerPanels = ChildPanelCreator.DrawChildPanels(CorrectPanelOrder);
            foreach (var readerPanel in readerPanels)
                DraggableGroupUI.Add(readerPanel);
        }

        private void OrderChanged(List<IDraggable> draggableObjects)
        {
            if (CorrectPanelOrder == null)
                return;

            var allCorrect = true;
            for (int i = 0; i < CorrectPanelOrder.Count; i++) {
                var actualIndex = IndexOfPanel(CorrectPanelOrder[i], draggableObjects);
                var distanceFromCorrect = Math.Abs(actualIndex - i);
                var optionType = GetOptionType(distanceFromCorrect);
                if (optionType != OptionType.Correct)
                    allCorrect = false;

                var orderableItem = draggableObjects[actualIndex] as ReaderOrderableItemPanelUI;
                orderableItem.SetColor(FeedbackColorInfo.GetColor(optionType));
            }

            FeedbackObject.SetActive(allCorrect);
        }

        private int IndexOfPanel(UserPanel userPanel, List<IDraggable> draggableObjects)
        {
            for (int i = 0; i < draggableObjects.Count; i++) {
                var orderableItem = draggableObjects[i] as ReaderOrderableItemPanelUI;
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