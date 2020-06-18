using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterAddablePanel : BaseWriterAddablePanel
    {
        public override event Action<IDraggable, Vector3> DragStarted;
        public override event Action<IDraggable, Vector3> DragEnded;
        public override event Action<IDraggable, Vector3> Dragging;
        public override event Action Deleted;

        public override LayoutElement LayoutElement => layoutElement;
        [SerializeField] private LayoutElement layoutElement = null;
        public BaseDragHandle DragHandle { get => dragHandle; set => dragHandle = value; }
        [SerializeField] private BaseDragHandle dragHandle;
        public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }
        [SerializeField] private CanvasGroup canvasGroup;
        public Button DeleteButton { get => deleteButton; set => deleteButton = value; }
        [SerializeField] private Button deleteButton;

        public BaseWriterPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseWriterPanelsDrawer childPanelCreator;

        public BaseWriterPinsDrawer PinsDrawer { get => pinsDrawer; set => pinsDrawer = value; }
        [SerializeField] private BaseWriterPinsDrawer pinsDrawer;


        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup) => ConfirmationPopup = confirmationPopup;

        protected virtual void Awake()
        {
            DeleteButton.onClick.AddListener(ConfirmDelete);
            DragHandle.StartDragging += StartDragging;
        }
        protected virtual void StartDragging()
        {
            MouseInput.Instance.RegisterDraggable(this);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected Panel CurrentPanel { get; set; }
        public override void Display(Encounter encounter)
        {
            CurrentEncounter = encounter;
            CurrentPanel = new Panel(Type);
            var panelDisplay = new WriterPanelValueDisplay();
            Fields = panelDisplay.Display(encounter, CurrentPanel, transform);
            if (PinsDrawer != null)
                PinsDrawer.Display(encounter, new PinData());
        }
        protected BaseField[] Fields { get; set; }
        public override void Display(Encounter encounter, Panel panel)
        {
            CurrentEncounter = encounter;
            CurrentPanel = new Panel(Type);
            var panelDisplay = new WriterPanelValueDisplay();
            Fields = panelDisplay.Display(encounter, panel, transform);

            if (panel.ChildPanels.Count > 0)
                ChildPanelCreator.DrawChildPanels(encounter, panel.ChildPanels);
            if (PinsDrawer != null) {
                if (panel.Pins == null)
                    panel.Pins = new PinData();
                PinsDrawer.Display(encounter, panel.Pins);
            }
        }

        public override Panel Serialize()
        {
            CurrentPanel.Type = Type;

            var panelDisplay = new WriterPanelValueDisplay();
            CurrentPanel.Values = panelDisplay.Serialize(Fields);
            if (ChildPanelCreator != null)
                CurrentPanel.ChildPanels = ChildPanelCreator.SerializeChildren();
            if (PinsDrawer != null)
                CurrentPanel.Pins = PinsDrawer.Serialize();

            return CurrentPanel;
        }


        protected virtual void ConfirmDelete() => ConfirmationPopup.ShowConfirmation(Delete, "Confirm", "Are you sure you want to remove this entry?");
        protected virtual void Delete()
        {
            Deleted?.Invoke();
            Destroy(gameObject);
        }

        public override void StartDrag(Vector3 mousePosition)
        {
            CanvasGroup.alpha = .5f;
            DragStarted?.Invoke(this, mousePosition);
        }
        public override void Drag(Vector3 mousePosition) => Dragging?.Invoke(this, mousePosition);
        public override void EndDrag(Vector3 mousePosition)
        {
            CanvasGroup.alpha = 1;
            DragEnded?.Invoke(this, mousePosition);
        }
    }
}