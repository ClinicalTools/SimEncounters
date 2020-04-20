using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueChoiceOptionUI : BaseReaderPanelUI
    {
        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private Toggle toggle;
        public virtual Toggle Toggle { get => toggle; set => toggle = value; }

        [SerializeField] private Color onColor;
        public virtual Color OnColor { get => onColor; set => onColor = value; }
        protected Color OffColor { get; set; }

        [SerializeField] private Image border;
        public virtual Image Border { get => border; set => border = value; }
        
        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }

        public override void Display(UserPanel userPanel)
        {
            Debug.LogError("pleaseee implement");
        }
    }
}