using ClinicalTools.Layout;
using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterTabToggle : MonoBehaviour, IDraggable
    {
        public virtual RectTransform RectTransform => (RectTransform)transform;

        public abstract UnityEngine.UI.LayoutElement LayoutElement { get; }

        public abstract Layout.ILayoutElement LayoutElement2 { get; }

        public abstract event Action Selected;
        public abstract event Action<Tab> Edited;
        public abstract event Action<Tab> Deleted;
        public event Action<IDraggable, Vector3> DragStarted;
        public event Action<IDraggable, Vector3> DragEnded;
        public event Action<IDraggable, Vector3> Dragging;

        public abstract void Display(Encounter encounter, Tab tab); 
        public abstract void SetToggleGroup(ToggleGroup group);
        public abstract void Select();

        public virtual void StartDrag(Vector3 mousePosition) => DragStarted?.Invoke(this, mousePosition);
        public virtual void EndDrag(Vector3 mousePosition) => DragEnded?.Invoke(this, mousePosition);
        public virtual void Drag(Vector3 mousePosition) => Dragging?.Invoke(this, mousePosition);
    }

    public class WriterTabToggle : BaseWriterTabToggle
    {
        public EncounterToggleBehaviour SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private EncounterToggleBehaviour selectToggle;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;
        public Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public override Layout.ILayoutElement LayoutElement2 => horizontalGroup;
        [SerializeField] private HorizontalGroup horizontalGroup;
        public override UnityEngine.UI.LayoutElement LayoutElement { get => layoutElement; }
        [SerializeField] private UnityEngine.UI.LayoutElement layoutElement;
        public BaseDragHandle DragHandle { get => dragHandle; set => dragHandle = value; }
        [SerializeField] private BaseDragHandle dragHandle;

        protected virtual TabEditorPopup TabEditorPopup { get; set; }
        [Inject] public void Inject(TabEditorPopup tabEditorPopup) => TabEditorPopup = tabEditorPopup;

        public override event Action Selected;
        public override event Action<Tab> Edited;
        public override event Action<Tab> Deleted;
        protected virtual void Awake()
        {
            SelectToggle.Selected += OnSelected;
            SelectToggle.Unselected += OnUnselected;
            EditButton.onClick.AddListener(Edit);

            DragHandle.StartDragging += StartDragging;
        }
        protected virtual void StartDragging()
        {
            MouseInput.Instance.RegisterDraggable(this);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected Tab CurrentTab { get; set; }



        public override void Display(Encounter encounter, Tab tab)
        {
            CurrentEncounter = encounter;
            CurrentTab = tab;
            NameLabel.text = tab.Name;
        }

        public override void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        public override void Select() => SelectToggle.Select();

        protected virtual void OnSelected()
        {
            Selected?.Invoke();
            EditButton.gameObject.SetActive(true);
        }
        protected virtual void OnUnselected()
        {
            EditButton.gameObject.SetActive(false);
        }

        protected virtual void Edit()
        {
            var tab = TabEditorPopup.EditTab(CurrentEncounter, CurrentTab);
            tab.AddOnCompletedListener((result) => FinishEdit(tab));
        }

        protected virtual void FinishEdit(WaitableResult<Tab> editedTab)
        {
            if (editedTab.IsError)
                return;

            if (editedTab.Result == null) {
                Deleted?.Invoke(CurrentTab);
                Destroy(gameObject);
                return;
            }

            CurrentTab = editedTab.Result;
            Edited?.Invoke(CurrentTab);

            Display(CurrentEncounter, CurrentTab);
        }
    }
}