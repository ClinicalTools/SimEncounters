using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableItemPanelDisplay : IReaderPanelDisplay
    {
        protected ReaderScene Reader { get; }
        public KeyValuePair<string, Panel> KeyedPanel { get; }
        public ReaderOrderableItemPanelUI PanelUI { get; }
        public RectTransform RectTransform => PanelUI.RectTransform;
        public LayoutElement LayoutElement => PanelUI.LayoutElement;

        protected IValueField[] ValueFields { get; }

        public ReaderOrderableItemPanelDisplay(ReaderScene reader, ReaderOrderableItemPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
        {
            Reader = reader;
            PanelUI = panelUI;
            KeyedPanel = keyedPanel;

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            ValueFields = valueFieldInitializer.InitializePanelValueFields(PanelUI.gameObject, keyedPanel.Value);
        }

        public void SetColor(Color color)
        {
            color.a = 1;
            foreach (var image in PanelUI.ColoredImages)
                image.color = color;
        }
    }
}