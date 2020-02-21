using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using ClinicalTools.SimEncounters.Extensions;
using ClinicalTools.SimEncounters.Collections;
using System;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableGroupPanelDislay
    {
        protected ReaderOrderableGroupPanelUI PanelUI { get; }
        protected virtual FeedbackColorInfo FeedbackColorInfo { get; } = new FeedbackColorInfo();
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }
        protected List<ReaderOrderableItemPanelUI> ChildPanels { get; set; }
        protected IValueField[] ValueFields { get; set; }
        public KeyValuePair<string, Panel> KeyedPanel { get; }

        public ReaderOrderableGroupPanelDislay(ReaderScene reader, ReaderOrderableGroupPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
        {
            PanelUI = panelUI;
            KeyedPanel = keyedPanel;
            var panel = keyedPanel.Value;

            CreatePinButtons(reader, panel);

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            ValueFields = valueFieldInitializer.InitializePanelValueFields(panelUI.gameObject, panel);

            var shuffeledPanels = new List<KeyValuePair<string, Panel>>(panel.ChildPanels);
            if (panel.ChildPanels.Count > 1) {
                while (HasSamePanelOrder(shuffeledPanels, panel.ChildPanels))
                    shuffeledPanels.Shuffle();
            }

            ReaderPanelCreator = new ReaderPanelCreator(reader, panelUI.DraggableGroupUI.ChildrenParent);
            ChildPanels = ReaderPanelCreator.Deserialize(shuffeledPanels, panelUI.OrderableItemOptions);

            var verticalDraggableGroup = new VerticalDraggableGroup(reader.Mouse, panelUI.DraggableGroupUI);
            foreach (var childPanel in ChildPanels)
                verticalDraggableGroup.Add(childPanel);
            verticalDraggableGroup.OrderChanged += OrderChanged;
        }

        private void OrderChanged(List<IDraggable> draggableObjects)
        {
            var allCorrect = true;
            for (int i = 0; i < draggableObjects.Count; i++) {
                var orderableItem = draggableObjects[i] as ReaderOrderableItemPanelUI;
                if (orderableItem == null)
                    continue;

                var actualIndex = KeyedPanel.Value.ChildPanels.IndexOf(orderableItem.KeyedPanel.Value);
                var distanceFromCorrect = Math.Abs(actualIndex - i);
                var optionType = GetOptionType(distanceFromCorrect);
                if (optionType != OptionType.Correct)
                    allCorrect = false;

                orderableItem.SetColor(FeedbackColorInfo.GetColor(optionType));
            }

            PanelUI.FeedbackObject.SetActive(allCorrect);
        }

        private bool HasSamePanelOrder(List<KeyValuePair<string, Panel>> shuffeledPanels, OrderedCollection<Panel> childPanels)
        {
            for (int i = 0; i < childPanels.Count; i++) {
                if (shuffeledPanels[i].Key != childPanels[i].Key)
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

        protected virtual ReaderPinsGroup CreatePinButtons(ReaderScene reader, Panel panel) => reader.Pins.CreateButtons(panel.Pins, transform);
    }

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

        protected virtual FeedbackColorInfo FeedbackColorInfo { get; } = new FeedbackColorInfo();
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }
        protected List<ReaderOrderableItemPanelUI> ChildPanels { get; set; }
        protected IValueField[] ValueFields { get; set; }

        public override void Initialize(ReaderScene reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);
            var panel = keyedPanel.Value;

            CreatePinButtons(reader, panel);

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            ValueFields = valueFieldInitializer.InitializePanelValueFields(gameObject, panel);

            var shuffeledPanels = new List<KeyValuePair<string, Panel>>(panel.ChildPanels);
            if (panel.ChildPanels.Count > 1) {
                while (HasSamePanelOrder(shuffeledPanels, panel.ChildPanels))
                    shuffeledPanels.Shuffle();
            }

            ReaderPanelCreator = new ReaderPanelCreator(reader, DraggableGroupUI.ChildrenParent);
            ChildPanels = ReaderPanelCreator.Deserialize(shuffeledPanels, OrderableItemOptions);

            var verticalDraggableGroup = new VerticalDraggableGroup(reader.Mouse, DraggableGroupUI);
            foreach (var childPanel in ChildPanels)
                verticalDraggableGroup.Add(childPanel);
            verticalDraggableGroup.OrderChanged += OrderChanged;
        }

        private void OrderChanged(List<IDraggable> draggableObjects)
        {
            var allCorrect = true;
            for (int i = 0; i < draggableObjects.Count; i++) {
                var orderableItem = draggableObjects[i] as ReaderOrderableItemPanelUI;
                if (orderableItem == null)
                    continue;

                var actualIndex = KeyedPanel.Value.ChildPanels.IndexOf(orderableItem.KeyedPanel.Value);
                var distanceFromCorrect = Math.Abs(actualIndex - i);
                var optionType = GetOptionType(distanceFromCorrect);
                if (optionType != OptionType.Correct)
                    allCorrect = false;

                orderableItem.SetColor(FeedbackColorInfo.GetColor(optionType));
            }

            FeedbackObject.SetActive(allCorrect);
        }

        private bool HasSamePanelOrder(List<KeyValuePair<string, Panel>> shuffeledPanels, OrderedCollection<Panel> childPanels)
        {
            for (int i = 0; i < childPanels.Count; i++) {
                if (shuffeledPanels[i].Key != childPanels[i].Key)
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

        protected virtual ReaderPinsGroup CreatePinButtons(ReaderScene reader, Panel panel) => reader.Pins.CreateButtons(panel.Pins, transform);
    }
}