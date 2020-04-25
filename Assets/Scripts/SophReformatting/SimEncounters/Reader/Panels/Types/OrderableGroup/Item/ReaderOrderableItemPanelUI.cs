using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableItemPanelUI : BaseReaderPanelUI, IDraggable, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UserPanel CurrentPanel { get; protected set; }
        public RectTransform RectTransform => (RectTransform)transform;

        public event Action<IDraggable, Vector3> DragStarted;
        public event Action<IDraggable, Vector3> DragEnded;
        public event Action<IDraggable, Vector3> Dragging;

        [SerializeField] private LayoutElement layoutElement;
        public LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }

        [SerializeField] private Button dragHandle;
        public Button DragHandle { get => dragHandle; set => dragHandle = value; }

        [SerializeField] private List<Image> coloredImages;
        public List<Image> ColoredImages { get => coloredImages; set => coloredImages = value; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }
        [Inject]
        public void Inject(BasicReaderPanelDrawer basicReaderPanel) => BasicPanelDrawer = basicReaderPanel;

        public override void Display(UserPanel userPanel)
        {
            CurrentPanel = userPanel;
            BasicPanelDrawer.Display(userPanel, transform, transform);
        }
        public virtual void SetColor(Color color)
        {
            color.a = 1;
            foreach (var image in ColoredImages)
                image.color = color;
        }

        public void OnPointerDown(PointerEventData eventData) => MouseInput.Instance.RegisterDraggable(this);
        public void OnPointerEnter(PointerEventData eventData) => MouseInput.Instance.SetCursorState(CursorState.Draggable);
        public void OnPointerExit(PointerEventData eventData) => MouseInput.Instance.RemoveCursorState(CursorState.Draggable);
        
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
    }
}