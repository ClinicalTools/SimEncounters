using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableItemPanelDisplay : IReaderPanelDisplay
    {
        protected ReaderScene Reader { get; }
        public KeyValuePair<string, Panel> KeyedPanel { get; protected set; }
        public ReaderOrderableItemPanelUI PanelUI { get; }
        public RectTransform RectTransform => PanelUI.RectTransform;
        public LayoutElement LayoutElement => PanelUI.LayoutElement;

        protected IValueField[] ValueFields { get; }

        public ReaderOrderableItemPanelDisplay(ReaderScene reader, ReaderOrderableItemPanelUI panelUI)
        {
            Reader = reader;
            PanelUI = panelUI;
        }

        public virtual void Display(KeyValuePair<string, Panel> keyedPanel)
        {
            KeyedPanel = keyedPanel;
            InitializeValueFields(keyedPanel);
        }

        public virtual void SetColor(Color color)
        {
            color.a = 1;
            foreach (var image in PanelUI.ColoredImages)
                image.color = color;
        }

        protected virtual IValueField[] InitializeValueFields(KeyValuePair<string, Panel> keyedPanel) => Reader.ValueFieldInitializer.InitializePanelValueFields(PanelUI.gameObject, keyedPanel.Value);
    }
}