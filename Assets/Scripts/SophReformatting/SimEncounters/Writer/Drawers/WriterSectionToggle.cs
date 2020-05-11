using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSectionToggle : MonoBehaviour
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
        public Layout.LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }
        [SerializeField] private Layout.LayoutElement layoutElement;

        protected virtual SectionEditorPopup SectionEditorPopup { get; set; }
        [Inject] public void Inject(SectionEditorPopup sectionEditorPopup) => SectionEditorPopup = sectionEditorPopup;

        public Action Selected;
        public Action<Section> Deleted;
        public Action<Section> Edited;

        protected virtual void Awake()
        {
            LayoutElement.WidthValues.Min = 160;
         
            SelectToggle.Selected += OnSelected;
            SelectToggle.Unselected += OnUnselected;
            EditButton.onClick.AddListener(Edit);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected Section CurrentSection { get; set; }
        public void Display(Encounter encounter, Section section)
        {
            CurrentEncounter = encounter;
            CurrentSection = section;

            Image.color = section.Color;
            var icons = encounter.Images.Icons;
            if (icons.ContainsKey(section.IconKey))
                Icon.sprite = icons[section.IconKey];

            NameLabel.text = section.Name;
        }

        public void Select() => SelectToggle.Select();

        public void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void OnSelected()
        {
            Selected?.Invoke();
            EditButton.gameObject.SetActive(true);
            LayoutElement.WidthValues.Min = 220;
        }
        protected virtual void OnUnselected()
        {
            EditButton.gameObject.SetActive(false);
            LayoutElement.WidthValues.Min = 160;
        }

        protected virtual void Edit()
        {
            var section =  SectionEditorPopup.EditSection(CurrentEncounter, CurrentSection);
            section.AddOnCompletedListener((result) => FinishEdit(section));
        }

        protected virtual void FinishEdit(WaitableResult<Section> editedSection)
        {
            if (editedSection.IsError)
                return;
            
            if (editedSection.Result == null) {
                Deleted?.Invoke(CurrentSection);
                Destroy(gameObject);
                return;
            }

            CurrentSection = editedSection.Result;
            Edited?.Invoke(CurrentSection);

            Display(CurrentEncounter, CurrentSection);
        }
    }
}