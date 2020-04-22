using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ClinicalTools.SimEncounters.Extensions;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableGroupPanelUI : BaseReaderPanelUI
    {
        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private DraggableGroupUI draggableGroup;
        public DraggableGroupUI DraggableGroupUI { get => draggableGroup; set => draggableGroup = value; }

        [SerializeField] private List<ReaderOrderableItemPanelUI> orderableItemOptions;
        public virtual List<ReaderOrderableItemPanelUI> OrderableItemOptions { get => orderableItemOptions; set => orderableItemOptions = value; }

        [SerializeField] private GameObject feedbackObject;
        public virtual GameObject FeedbackObject { get => feedbackObject; set => feedbackObject = value; }

        protected UserPinGroupDrawer PinButtons { get; set; }
        protected IPanelFieldInitializer FieldInitializer { get; set; }

        [Inject]
        public virtual void Inject(UserPinGroupDrawer pinButtons, IPanelFieldInitializer fieldInitializer)
        {
            PinButtons = pinButtons;
            FieldInitializer = fieldInitializer;
        }

        public override void Display(UserPanel panel)
        {
            CreatePinButtons(panel);

            FieldInitializer.InitializePanelValueFields(transform, panel);

            var childPanels = panel.GetChildPanels();
            var shuffeledPanels = new List<UserPanel>(childPanels);
            if (childPanels.Count > 1) {
                while (HasSamePanelOrder(shuffeledPanels, childPanels))
                    shuffeledPanels.Shuffle();
            }

            var childrenGroup = DeserializeChildren(shuffeledPanels);
            //childrenGroup.OrderChanged += (draggableObjects) => OrderChanged(draggableObjects, keyedPanel);
        }

        public VerticalDraggableGroup DeserializeChildren(IEnumerable<UserPanel> panels)
        {
            foreach (var panel in panels) {
                /*
                var panelUI = ReaderPanelCreator.Deserialize(panel, OrderableItemOptions);
                var panelDisplay = Reader.PanelDisplayFactory.CreateOrderableItemPanel(panelUI);
                panelDisplay.Display(panel);
                childrenGroup.Add(panelUI);
                ChildPanels.Add(panelDisplay);*/
            }
            return null;
            //return childrenGroup;
        }
        private void OrderChanged(List<IDraggable> draggableObjects, KeyValuePair<string, Panel> keyedPanel)
        {
            var allCorrect = true;
            for (int i = 0; i < draggableObjects.Count; i++) {
                /*var orderableItem = draggableObjects[i] as ReaderOrderableItemPanelUI;
                var orderableItem = ChildPanels.FirstOrDefault(panelDisplay => (draggableObjects[i] as ReaderOrderableItemPanelUI) == panelDisplay.PanelUI);
                if (orderableItem == null)
                    continue;
                var actualIndex = keyedPanel.Value.ChildPanels.IndexOf(orderableItem.KeyedPanel.Value);
                var distanceFromCorrect = Math.Abs(actualIndex - i);
                var optionType = GetOptionType(distanceFromCorrect);
                if (optionType != OptionType.Correct)
                    allCorrect = false;

                orderableItem.SetColor(FeedbackColorInfo.GetColor(optionType));*/
            }

            FeedbackObject.SetActive(allCorrect);
        }

        private bool HasSamePanelOrder(List<UserPanel> shuffeledPanels, List<UserPanel> childPanels)
        {
            for (int i = 0; i < childPanels.Count; i++) {
                if (shuffeledPanels[i] != childPanels[i])
                    return false;
            }

            return true;
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

        protected virtual UserPinGroupDrawer CreatePinButtons(UserPanel userPanel)
        {
            if (userPanel.PinGroup == null)
                return null;

            var pinButtons = Instantiate(PinButtons, transform);
            pinButtons.Display(userPanel.PinGroup);

            return pinButtons;
        }
    }
}