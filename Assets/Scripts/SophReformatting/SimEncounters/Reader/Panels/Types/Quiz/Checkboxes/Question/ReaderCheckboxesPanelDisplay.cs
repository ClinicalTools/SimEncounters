using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxesPanelDisplay : ReaderPanelDisplay
    {
        protected ReaderCheckboxesPanelUI CheckboxesPanelUI { get; }
        //protected List<IFeedback> Options { get; } = new List<IFeedback>();
        public ReaderCheckboxesPanelDisplay(ReaderScene reader, ReaderCheckboxesPanelUI checkboxesPanelUI)
            : base(reader, checkboxesPanelUI)
        {
            CheckboxesPanelUI = checkboxesPanelUI;
            CheckboxesPanelUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected override IReaderPanelDisplay DeserializeChild(KeyValuePair<string, Panel> keyedPanel)
        {
            var panelDisplay = base.DeserializeChild(keyedPanel);
            //if (panelDisplay is IFeedback option)
                //Options.Add(option);
            return panelDisplay;
        }
        protected virtual void GetFeedback()
        {
            //foreach (var option in Options)
                //option.GetFeedback();
        }
    }
}