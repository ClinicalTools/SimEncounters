using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface ICheckboxOption
    {
        void GetFeedback();
    }
    public class ReaderCheckboxesPanelUI : ReaderPanelUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        protected List<ICheckboxOption> Options { get; } = new List<ICheckboxOption>();

        protected virtual void Awake()
        {
            GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected override UserPanelDrawer DeserializeChild(UserPanel userPanel)
        {
            var panel = base.DeserializeChild(userPanel);
            if (panel is ICheckboxOption option)
                Options.Add(option);

            return panel;
        }


        protected virtual void GetFeedback()
        {
            foreach (var option in Options)
                option.GetFeedback();
        }
    }
}