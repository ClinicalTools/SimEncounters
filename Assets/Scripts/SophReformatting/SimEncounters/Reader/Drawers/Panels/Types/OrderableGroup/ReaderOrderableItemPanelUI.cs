
using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableItemPanelUI : BaseReaderOrderableItemPanel, IDraggable, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public override RectTransform RectTransform => (RectTransform)transform;

        public override event Action<IDraggable, Vector3> DragStarted;
        public override event Action<IDraggable, Vector3> DragEnded;
        public override event Action<IDraggable, Vector3> Dragging;

        public override LayoutElement LayoutElement { get => layoutElement; }
        [SerializeField] private LayoutElement layoutElement = null;
        public Button DragHandle { get => dragHandle; set => dragHandle = value; }
        [SerializeField] private Button dragHandle;
        public List<Image> ColoredImages { get => coloredImages; set => coloredImages = value; }
        [SerializeField] private List<Image> coloredImages;

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay basicReaderPanel) => BasicPanelDrawer = basicReaderPanel;

        public override void Display(UserPanel userPanel)
        {
            CurrentPanel = userPanel;
            BasicPanelDrawer.Display(userPanel, transform, transform);
        }
        public override void SetColor(Color color)
        {
            color.a = 1;
            foreach (var image in ColoredImages)
                image.color = color;
        }

        public void OnPointerDown(PointerEventData eventData) => MouseInput.Instance.RegisterDraggable(this);
        public void OnPointerEnter(PointerEventData eventData) => MouseInput.Instance.SetCursorState(CursorState.Draggable);
        public void OnPointerExit(PointerEventData eventData) => MouseInput.Instance.RemoveCursorState(CursorState.Draggable);
        
        public override void StartDrag(Vector3 mousePosition)
        {
            DragHandle.interactable = false;
            DragStarted?.Invoke(this, mousePosition);
        }

        public override void Drag(Vector3 mousePosition)
        {
            Dragging?.Invoke(this, mousePosition);
        }

        public override void EndDrag(Vector3 mousePosition)
        {
            DragHandle.interactable = true;
            DragEnded?.Invoke(this, mousePosition);
        }
    }
}