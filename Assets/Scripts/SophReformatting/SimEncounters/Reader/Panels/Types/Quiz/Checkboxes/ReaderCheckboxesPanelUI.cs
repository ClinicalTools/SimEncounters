using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxesPanelDisplay : ReaderPanelDisplay
    {

        protected ReaderCheckboxesPanelUI CheckboxesPanelUI { get; }
        public ReaderCheckboxesPanelDisplay(ReaderScene reader, ReaderCheckboxesPanelUI checkboxesPanelUI, KeyValuePair<string, Panel> keyedPanel)
            : base(reader, checkboxesPanelUI, keyedPanel)
        {
            CheckboxesPanelUI = checkboxesPanelUI;
            CheckboxesPanelUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in ChildPanels)
            {
                if (!(child is ReaderCheckboxOptionUI))
                    continue;

                var feedbackChild = (ReaderCheckboxOptionUI)child;
                feedbackChild.GetFeedback();
            }
        }
    }
    public class ReaderCheckboxesPanelUI : ReaderPanelUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        public override void Initialize(ReaderScene reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);

            GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in ChildPanels) {
                if (!(child is ReaderCheckboxOptionUI))
                    continue;

                var feedbackChild = (ReaderCheckboxOptionUI)child;
                feedbackChild.GetFeedback();
            }
        }
    }
}