using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueChoiceUI : BaseReaderPanelUI
    {
        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private Transform optionsParent;
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }

        [SerializeField] private ReaderDialogueChoiceOptionUI choicePrefab;
        public ReaderDialogueChoiceOptionUI OptionPrefab { get => choicePrefab; set => choicePrefab = value; }

        [SerializeField] private Transform feedbackParent;
        public Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }

        [SerializeField] private GameObject instructions;
        public virtual GameObject Instructions { get => instructions; set => instructions = value; }

        [SerializeField] private Button showOptionsButton;
        public Button ShowOptionsButton { get => showOptionsButton; set => showOptionsButton = value; }

        [SerializeField] private ToggleGroup optionGroup;
        public ToggleGroup OptionGroup { get => optionGroup; set => optionGroup = value; }

        public event Action Completed;

        public override void Display(UserPanel userPanel)
        {
            Debug.LogError("pleaseee implement");
        }
    }
}