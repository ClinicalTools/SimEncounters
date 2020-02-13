using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableItemPanelUI : BaseReaderPanelUI, IDraggable, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public RectTransform RectTransform => (RectTransform)transform;

        public event Action<IDraggable, Vector3> DragStarted;
        public event Action<IDraggable, Vector3> DragEnded;
        public event Action<IDraggable, Vector3> Dragging;

        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private LayoutElement layoutElement;
        public LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }

        [SerializeField] private Button dragHandle;
        public Button DragHandle { get => dragHandle; set => dragHandle = value; }

        [SerializeField] private List<Image> coloredImages;
        public List<Image> ColoredImages { get => coloredImages; set => coloredImages = value; }

        protected EncounterReader Reader { get; set; }
        protected IValueField[] ValueFields { get; set; }

        public override void Initialize(EncounterReader reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);
            Reader = reader;
            var panel = keyedPanel.Value;

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            ValueFields = valueFieldInitializer.InitializePanelValueFields(gameObject, panel);
        }

        public void SetColor(Color color)
        {
            color.a = 1;
            foreach (var image in ColoredImages)
                image.color = color;
        }

        public void StartDrag(Vector3 mousePosition)
        {
            DragHandle.interactable = false;
            DragStarted?.Invoke(this, mousePosition);
        }

        public void EndDrag(Vector3 mousePosition)
        {
            DragHandle.interactable = true;
            DragEnded?.Invoke(this, mousePosition);
        }

        public void Drag(Vector3 mousePosition)
        {
            Dragging?.Invoke(this, mousePosition);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Reader.Mouse.RegisterDraggable(this);
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