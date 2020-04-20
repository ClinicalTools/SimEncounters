using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderableGroupPanelUI : BaseReaderPanelUI
    {
        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private DraggableGroupUI draggableGroup;
        public DraggableGroupUI DraggableGroupUI { get => draggableGroup; set => draggableGroup = value; }

        [SerializeField] private List<ReaderOrderableItemPanelUI> orderableItemOptions;
        public virtual List<ReaderOrderableItemPanelUI> OrderableItemOptions { get => orderableItemOptions; set => orderableItemOptions = value; }

        [SerializeField] private GameObject feedbackObject;
        public virtual GameObject FeedbackObject { get => feedbackObject; set => feedbackObject = value; }

        public override void Display(UserPanel userPanel)
        {
            Debug.LogError("pleaseee implement");
        }
    }
}