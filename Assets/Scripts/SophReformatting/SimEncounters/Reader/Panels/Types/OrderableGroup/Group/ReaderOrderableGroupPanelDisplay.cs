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
        public KeyValuePair<string, Panel> KeyedPanel { get; }

        public ReaderOrderableGroupPanelDisplay(ReaderScene reader, ReaderOrderableGroupPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
        {
            Reader = reader;
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

            ReaderPanelCreator = new ReaderPanelCreator(Reader, PanelUI.DraggableGroupUI.ChildrenParent);
            DeserializeChildren(shuffeledPanels);
        }

        public void DeserializeChildren(IEnumerable<KeyValuePair<string, Panel>> panels)
        {
            var verticalDraggableGroup = new VerticalDraggableGroup(Reader.Mouse, PanelUI.DraggableGroupUI);
            foreach (var keyedPanel in panels) {
                var panelUI = ReaderPanelCreator.Deserialize(keyedPanel, PanelUI.OrderableItemOptions);
                var panelDisplay = Reader.PanelDisplayFactory.CreateOrderableItemPanel(panelUI, keyedPanel);
                verticalDraggableGroup.Add(panelUI);
                ChildPanels.Add(panelDisplay);
            }
            verticalDraggableGroup.OrderChanged += OrderChanged;
        }
        private void OrderChanged(List<IDraggable> draggableObjects)
        {
            var allCorrect = true;
            for (int i = 0; i < draggableObjects.Count; i++) {
                //var orderableItem = draggableObjects[i] as ReaderOrderableItemPanelUI;
                var orderableItem = ChildPanels.FirstOrDefault(panelDisplay => draggableObjects[i] == panelDisplay.PanelUI);
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

        protected virtual ReaderPinsGroup CreatePinButtons(ReaderScene reader, Panel panel) => reader.Pins.CreateButtons(panel.Pins, PanelUI.transform);
    }
}