using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public enum CursorState
    {
        Normal, Draggable
    }

    public class MouseInput : MonoBehaviour
    {
        public static MouseInput Instance { get; private set; }

        public SwipeHandler SwipeHandler { get; set; } = new SwipeHandler();

        [SerializeField] private Image cursorImage;
        public Image CursorImage { get => cursorImage; set => cursorImage = value; }
        [SerializeField] private Sprite normalCursorSprite;
        public Sprite NormalCursorSprite { get => normalCursorSprite; set => normalCursorSprite = value; }
        [SerializeField] private Sprite draggableCursorSprite;
        public Sprite DraggableCursorSprite { get => draggableCursorSprite; set => draggableCursorSprite = value; }
        [SerializeField] private Texture2D normalCursorTexture;
        public Texture2D NormalCursorTexture { get => normalCursorTexture; set => normalCursorTexture = value; }
        [SerializeField] private Texture2D draggableCursorTexture;
        public Texture2D DraggableCursorTexture { get => draggableCursorTexture; set => draggableCursorTexture = value; }

        public virtual bool CanDrag => DraggedObjects.Count == 0;

        public virtual CursorState CurrentCursorState {
            get {
                if (DraggedObjects.Count > 0)
                    return CursorState.Draggable;
                else if (CurrentCursorStates.Count == 0)
                    return CursorState.Normal;
                else
                    return CurrentCursorStates[CurrentCursorStates.Count - 1];
            }
        }
        protected virtual List<CursorState> CurrentCursorStates { get; } = new List<CursorState>();

        protected virtual List<IDraggable> DraggedObjects { get; } = new List<IDraggable>();

        protected virtual void UpdateCursor()
        {
            var currentCursorState = CurrentCursorState;
            if (currentCursorState == CursorState.Normal) {
                CursorImage.sprite = NormalCursorSprite;
                Cursor.SetCursor(NormalCursorTexture, new Vector2(32, 32), CursorMode.ForceSoftware);
            } else if (currentCursorState == CursorState.Draggable) {
                CursorImage.sprite = DraggableCursorSprite;
                Cursor.SetCursor(DraggableCursorTexture, new Vector2(32, 32), CursorMode.ForceSoftware);
            }
        }

        public virtual void SetCursorState(CursorState cursorState)
        {
            CurrentCursorStates.Add(cursorState);
            UpdateCursor();
        }

        public virtual void RemoveCursorState(CursorState cursorState)
        {
            if (!CurrentCursorStates.Contains(cursorState))
                return;

            CurrentCursorStates.Remove(cursorState);
            UpdateCursor();
        }

        protected virtual void Awake()
        {
            Instance = this;
            UpdateCursor();
        }

        protected virtual void Update()
        {
            if (DraggedObjects.Count > 0) {
                if (Input.GetMouseButtonUp(0))
                    EndDrag(Input.mousePosition);
                else
                    Drag(Input.mousePosition);
                
                return;
            } 

            if (Input.touches.Length == 1)
                SwipeHandler.TouchPosition(Input.touches[0].position);
            else if (Input.GetMouseButton(0))
                SwipeHandler.TouchPosition(Input.mousePosition);
            else if (SwipeHandler.IsSwiping)
                SwipeHandler.ReleaseTouch();
        }

        public bool RegisterDraggable(IDraggable draggable)
        {
            if (!Input.GetMouseButtonDown(0))
                return false;

            DraggedObjects.Add(draggable);
            draggable.StartDrag(Input.mousePosition);
            UpdateCursor();
            return true;
        }

        protected virtual void Drag(Vector3 mousePosition)
        {
            foreach (var draggedObject in DraggedObjects)
                draggedObject.Drag(mousePosition);
        }

        protected virtual void EndDrag(Vector3 mousePosition)
        {
            foreach (var draggedObject in DraggedObjects)
                draggedObject.EndDrag(mousePosition);
            DraggedObjects.Clear();
            UpdateCursor();
        }
    }
}