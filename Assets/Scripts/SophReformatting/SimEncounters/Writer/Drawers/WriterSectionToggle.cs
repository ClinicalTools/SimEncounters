using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterSectionToggle : MonoBehaviour, IDraggable
    {
        public virtual RectTransform RectTransform => (RectTransform)transform;

        public abstract LayoutElement LayoutElement { get; }

        public abstract Layout.ILayoutElement LayoutElement2 { get; }

        public abstract event Action Selected;
        public abstract event Action<Section> Edited;
        public abstract event Action<Section> Deleted;
        public event Action<IDraggable, Vector3> DragStarted;
        public event Action<IDraggable, Vector3> DragEnded;
        public event Action<IDraggable, Vector3> Dragging;

        public abstract void Display(Encounter encounter, Section section);
        public abstract void SetToggleGroup(ToggleGroup group);
        public abstract void Select();

        public virtual void StartDrag(Vector3 mousePosition) => DragStarted?.Invoke(this, mousePosition);
        public virtual void EndDrag(Vector3 mousePosition) => DragEnded?.Invoke(this, mousePosition);
        public virtual void Drag(Vector3 mousePosition) => Dragging?.Invoke(this, mousePosition);
    }
    public class WriterSectionToggle : BaseWriterSectionToggle
    {
        public EncounterToggleBehaviour SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private EncounterToggleBehaviour selectToggle;
        public Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;
        public Image Icon { get => icon; set => icon = value; }
        [SerializeField] private Image icon;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;
        public Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public override LayoutElement LayoutElement { get => layoutElement; }
        [SerializeField] private LayoutElement layoutElement = null;
        public override Layout.ILayoutElement LayoutElement2 => LayoutElement3;
        public Layout.HorizontalGroup LayoutElement3 { get => layoutElement3; set => layoutElement3 = value; }
        [SerializeField] private Layout.HorizontalGroup layoutElement3;
        public BaseDragHandle DragHandle { get => dragHandle; set => dragHandle = value; }
        [SerializeField] private BaseDragHandle dragHandle;

        protected virtual SectionEditorPopup SectionEditorPopup { get; set; }
        [Inject] public void Inject(SectionEditorPopup sectionEditorPopup) => SectionEditorPopup = sectionEditorPopup;

        public override event Action Selected;
        public override event Action<Section> Deleted;
        public override event Action<Section> Edited;

        protected virtual void Awake()
        {         
            SelectToggle.Selected += OnSelected;
            SelectToggle.Unselected += OnUnselected;
            EditButton.onClick.AddListener(Edit);

            DragHandle.StartDragging += () => MouseInput.Instance.RegisterDraggable(this);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected Section CurrentSection { get; set; }
        public override void Display(Encounter encounter, Section section)
        {
            CurrentEncounter = encounter;
            CurrentSection = section;

            Image.color = section.Color;
            var icons = encounter.Images.Icons;
            if (section.IconKey != null && icons.ContainsKey(section.IconKey))
                Icon.sprite = icons[section.IconKey];

            NameLabel.text = section.Name;
        }

        public override void Select() => SelectToggle.Select();

        public override void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void OnSelected()
        {
            Selected?.Invoke();
            EditButton.gameObject.SetActive(true);
            LayoutElement3.GroupWidth.ControlChild = false;
            LayoutElement3.GroupWidth.ExpandChild = false;
            LayoutElement3.GroupWidth.FitChild = true;
            LayoutElement3.GroupWidth.DimensionLayout.Preferred = null;
        }
        protected virtual void OnUnselected()
        {
            EditButton.gameObject.SetActive(false);
            LayoutElement3.GroupWidth.ControlChild = true;
            LayoutElement3.GroupWidth.ExpandChild = true;
            LayoutElement3.GroupWidth.FitChild = false;
            LayoutElement3.GroupWidth.DimensionLayout.Preferred = 150;
        }

        protected virtual void Edit()
        {
            var section =  SectionEditorPopup.EditSection(CurrentEncounter, CurrentSection);
            section.AddOnCompletedListener(FinishEdit);
        }

        protected virtual void FinishEdit(WaitedResult<Section> editedSection)
        {
            if (editedSection.IsError())
                return;
            
            if (editedSection.Value == null) {
                Deleted?.Invoke(CurrentSection);
                Destroy(gameObject);
                return;
            }

            CurrentSection = editedSection.Value;
            Edited?.Invoke(CurrentSection);

            Display(CurrentEncounter, CurrentSection);
        }
    }
}