using ClinicalTools.SimEncounters.UI;
using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class MobileReaderTabToggle : BaseReaderTabToggle, ICompletionDrawer
    {
        public SelectableToggle SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private SelectableToggle selectToggle;
        public Image SelectedImage { get => selectedImage; set => selectedImage = value; }
        [SerializeField] private Image selectedImage;
        public GameObject VisitedCheck { get => visitedCheck; set => visitedCheck = value; }
        [SerializeField] private GameObject visitedCheck;

        public override event Action Selected;

        protected virtual void Awake() => SelectToggle.Selected += () => Selected?.Invoke();

        protected UserTab CurrentTab { get; set; }
        public override void Display(UserTab tab)
        {
            CurrentTab = tab;

            SelectToggle.Unselected += ToggleUnselected;
            SelectToggle.Selected += ToggleSelected;
            if (CurrentTab.IsRead())
                SetVisited();
        }

        public override void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void SetVisited()
        {
            VisitedCheck.SetActive(true);
            SelectToggle.Toggle.image.color = ColorManager.GetColor(ColorType.Green);
        }

        protected virtual void ToggleUnselected()
        {
            SelectedImage.gameObject.SetActive(false);
            if (CurrentTab.IsRead())
                SetVisited();
        }
        protected virtual void ToggleSelected()
        {
            SelectedImage.gameObject.SetActive(true);
        }

        public override void Select() => SelectToggle.Select();

        public void CompletionDraw(ReaderSceneInfo readerSceneInfo) => SetVisited();
    }
}