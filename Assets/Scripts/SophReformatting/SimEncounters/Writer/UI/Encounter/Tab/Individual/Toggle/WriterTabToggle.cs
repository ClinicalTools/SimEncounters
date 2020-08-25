using ClinicalTools.Layout;

using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTabToggle : BaseWriterTabToggle
    {
        public SelectableToggle SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private SelectableToggle selectToggle;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;
        public Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public override Layout.ILayoutElement LayoutElement2 => horizontalGroup;
        [SerializeField] private HorizontalGroup horizontalGroup = null;
        public override UnityEngine.UI.LayoutElement LayoutElement { get => layoutElement; }
        [SerializeField] private UnityEngine.UI.LayoutElement layoutElement = null;
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
            tab.AddOnCompletedListener(FinishEdit);
        }

        protected virtual void FinishEdit(WaitedResult<Tab> editedTab)
        {
            if (editedTab.IsError())
                return;

            if (editedTab.Value == null) {
                Deleted?.Invoke(CurrentTab);
                Destroy(gameObject);
                return;
            }

            CurrentTab = editedTab.Value;
            Edited?.Invoke(CurrentTab);

            Display(CurrentEncounter, CurrentTab);
        }
    }
}