using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableItemPanelDisplay : IReaderPanelDisplay
    {
        protected ReaderScene Reader { get; set; }
        protected ReaderOrderableItemPanelUI PanelUI { get; set; }
        protected IValueField[] ValueFields { get; set; }

        public ReaderOrderableItemPanelDisplay(ReaderScene reader, ReaderOrderableItemPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
        {
            Reader = reader;
            PanelUI = panelUI;
            var panel = keyedPanel.Value;

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            ValueFields = valueFieldInitializer.InitializePanelValueFields(PanelUI.gameObject, panel);
        }

        public void SetColor(Color color)
        {
            color.a = 1;
            foreach (var image in PanelUI.ColoredImages)
                image.color = color;
        }

        public void StartDrag(Vector3 mousePosition)
        {
            PanelUI.DragHandle.interactable = false;
        }

        public void EndDrag(Vector3 mousePosition)
        {
            PanelUI.DragHandle.interactable = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Reader.Mouse.RegisterDraggable(PanelUI);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Reader.Mouse.SetCursorState(CursorState.Draggable);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Reader.Mouse.RemoveCursorState(CursorState.Draggable);
        }

    }
}