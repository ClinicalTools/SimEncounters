using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTabToggle : MonoBehaviour
    {
        public EncounterToggleBehaviour SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private EncounterToggleBehaviour selectToggle;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;
        public Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public Layout.LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }
        [SerializeField] private Layout.LayoutElement layoutElement;

        protected virtual TabEditorPopup TabEditorPopup { get; set; }
        [Inject] public void Inject(TabEditorPopup tabEditorPopup) => TabEditorPopup = tabEditorPopup;

        public event Action Selected;
        public event Action<Tab> Edited;
        public event Action<Tab> Deleted;
        protected virtual void Awake()
        {
            LayoutElement.WidthValues.Min = 150;

            SelectToggle.Selected += OnSelected;
            SelectToggle.Unselected += OnUnselected;
            EditButton.onClick.AddListener(Edit);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected Tab CurrentTab { get; set; }
        public void Display(Encounter encounter, Tab tab)
        {
            CurrentEncounter = encounter;
            CurrentTab = tab;
            NameLabel.text = tab.Name;
        }

        public void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        public virtual void Select() => SelectToggle.Select();

        protected virtual void OnSelected()
        {
            Selected?.Invoke();
            EditButton.gameObject.SetActive(true);
            LayoutElement.WidthValues.Min = 210;
        }
        protected virtual void OnUnselected()
        {
            EditButton.gameObject.SetActive(false);
            LayoutElement.WidthValues.Min = 150;
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