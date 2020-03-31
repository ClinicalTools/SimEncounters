using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderFeedbackUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI optionTypeLabel;
        public TextMeshProUGUI OptionTypeLabel { get => optionTypeLabel; set => optionTypeLabel = value; }
        public OptionType OptionType {
            get {
                switch (OptionTypeLabel.text) {
                    case "Correct":
                        return OptionType.Correct;
                    case "Incorrect":
                        return OptionType.Incorrect;
                    default:
                        return OptionType.PartiallyCorrect;
                }
            }
        }

        [SerializeField] private TextMeshProUGUI isCorrectLabel;
        public TextMeshProUGUI IsCorrectLabel { get => isCorrectLabel; set => isCorrectLabel = value; }

        [SerializeField] private Image stripes;
        public Image Stripes { get => stripes; set => stripes = value; }

        [SerializeField] private List<Image> coloredImages = new List<Image>();
        public List<Image> ColoredImages { get => coloredImages; set => coloredImages = value; }

        [SerializeField] private Button closeButton;
        public Button CloseButton { get => closeButton; set => closeButton = value; }

        [SerializeField] private List<GameObject> controlledObjects = new List<GameObject>();
        public List<GameObject> ControlledObjects { get => controlledObjects; set => controlledObjects = value; }

        [SerializeField] private List<GameObject> incorrectObjects = new List<GameObject>();
        public List<GameObject> IncorrectObjects { get => incorrectObjects; set => incorrectObjects = value; }
    }
}