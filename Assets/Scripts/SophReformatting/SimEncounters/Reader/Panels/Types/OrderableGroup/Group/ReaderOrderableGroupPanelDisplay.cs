using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using ClinicalTools.SimEncounters.Extensions;
using ClinicalTools.SimEncounters.Collections;
using System;
using System.Linq;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableGroupPanelDisplay : IReaderPanelDisplay
    {
        protected ReaderScene Reader { get; }
        protected ReaderOrderableGroupPanelUI PanelUI { get; }
        protected virtual FeedbackColorInfo FeedbackColorInfo { get; } = new FeedbackColorInfo();
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected List<ReaderOrderableItemPanelDisplay> ChildPanels { get; } = new List<ReaderOrderableItemPanelDisplay>();
        protected IValueField[] ValueFields { get; }

        public ReaderOrderableGroupPanelDisplay(ReaderScene reader, ReaderOrderableGroupPanelUI panelUI)
        {
            Reader = reader;
            PanelUI = panelUI;

            ReaderPanelCreator = new ReaderPanelCreator(Reader, null);
        }

        public void Display(KeyValuePair<string, Panel> keyedPanel)
        {
            CreatePinButtons(keyedPanel);

            var panel = keyedPanel.Value;

            Reader.ValueFieldInitializer.InitializePanelValueFields(PanelUI.gameObject, panel);

            var shuffeledPanels = new List<KeyValuePair<string, Panel>>(panel.ChildPanels);
            if (panel.ChildPanels.Count > 1) {
                while (HasSamePanelOrder(shuffeledPanels, panel.ChildPanels))
                    shuffeledPanels.Shuffle();
            }

            var childrenGroup = DeserializeChildren(shuffeledPanels);
            childrenGroup.OrderChanged += (draggableObjects) => OrderChanged(draggableObjects, keyedPanel);
        }

        public VerticalDraggableGroup DeserializeChildren(IEnumerable<KeyValuePair<string, Panel>> panels)
        {
            var childrenGroup = new VerticalDraggableGroup(Reader.Mouse, null);
            foreach (var keyedPanel in panels) {/*
                var panelUI = ReaderPanelCreator.Deserialize(keyedPanel, null);
                var panelDisplay = Reader.PanelDisplayFactory.CreateOrderableItemPanel(panelUI);
                panelDisplay.Display(keyedPanel);
                childrenGroup.Add(panelUI);
                ChildPanels.Add(panelDisplay);*/
            }
            return childrenGroup;
        }
        private void OrderChanged(List<IDraggable> draggableObjects, KeyValuePair<string, Panel> keyedPanel)
        {
            var allCorrect = true;
            for (int i = 0; i < draggableObjects.Count; i++) {
                //var orderableItem = draggableObjects[i] as ReaderOrderableItemPanelUI;
                var orderableItem = ChildPanels.FirstOrDefault(panelDisplay => (draggableObjects[i] as ReaderOrderableItemPanelUI) == panelDisplay.PanelUI);
                if (orderableItem == null)
                    continue;
                var actualIndex = keyedPanel.Value.ChildPanels.IndexOf(orderableItem.KeyedPanel.Value);
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

        protected virtual ReaderPinsGroup CreatePinButtons(KeyValuePair<string, Panel> keyedPanel) => Reader.Pins.CreateButtons(keyedPanel.Value.Pins, PanelUI.transform);
    }
}