﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ClinicalTools.SimEncounters.Extensions;
using UnityEngine.EventSystems;

namespace ClinicalTools.SimEncounters
{
    public class DropdownTextfield : MonoBehaviour
    {
        public BaseOptionsRetriever OptionRetriever { get => optionRetriever; set => optionRetriever = value; }
        [SerializeField] private BaseOptionsRetriever optionRetriever;

        public DropdownOption OptionPrefab { get => optionPrefab; set => optionPrefab = value; }
        [SerializeField] private DropdownOption optionPrefab;
        public ScrollRect OptionsScrollRect { get => optionsScrollRect; set => optionsScrollRect = value; }
        [SerializeField] private ScrollRect optionsScrollRect;
        public Button ShowOptionsButton { get => showOptionsButton; set => showOptionsButton = value; }
        [SerializeField] private Button showOptionsButton;
        public TMP_InputField InputField { get => inputField; set => inputField = value; }
        [SerializeField] private TMP_InputField inputField;
        public GameObject Dropdown { get => dropdown; set => dropdown = value; }
        [SerializeField] private GameObject dropdown;
        public GameObject SuggestionsBorders { get => suggestionsBorders; set => suggestionsBorders = value; }
        [SerializeField] private GameObject suggestionsBorders;
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons;

        protected virtual List<DropdownOption> Options { get; } = new List<DropdownOption>();
        protected virtual int HighlightedOptionIndex { get; set; } = -1;
        protected virtual List<DropdownOption> DisplayedOptions { get; set; } = new List<DropdownOption>();

        // Start is used rather than Awake, so that automatically populating fields doesn't trigger listeners
        protected virtual void Start()
        {
            InputField.onValueChanged.AddListener(TextChanged);
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Close);

            ShowOptionsButton.onClick.AddListener(ButtonPressed);

            var options = OptionRetriever.GetOptions();
            foreach (var option in options)
                AddOption(option);
        }

        protected virtual void Update()
        {
            if (!Dropdown.gameObject.activeSelf || DisplayedOptions.Count == 0)
                return;

            if (Input.GetKeyDown(KeyCode.DownArrow) && HighlightedOptionIndex < DisplayedOptions.Count - 1)
                ShiftHighlightedOption(+1);
            else if (Input.GetKeyDown(KeyCode.UpArrow) && HighlightedOptionIndex > 0)
                ShiftHighlightedOption(-1);
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Tab))
                OptionSelected(DisplayedOptions[HighlightedOptionIndex].Value);
        }

        public virtual void AddOption(string option)
        {
            var optionObject = Instantiate(OptionPrefab, OptionsScrollRect.content);
            optionObject.Display(option);
            optionObject.Selected += OptionSelected;
            Options.Add(optionObject);
        }

        protected virtual void TextChanged(string text)
        {
            var displayedOptions = new List<DropdownOption>();
            var invariantText = text.ToUpperInvariant();
            foreach (var option in Options) {
                var invariantOptionValue = option.Value.ToUpperInvariant();
                var optionActive = invariantText != invariantOptionValue && invariantOptionValue.Contains(invariantText);
                if (optionActive)
                    displayedOptions.Add(option);
                option.SetActive(optionActive);
            }

            SetDisplayedOptions(displayedOptions);
        }

        protected virtual void ButtonPressed()
        {
            if (Dropdown.activeSelf) {
                Dropdown.SetActive(false);
                return;
            }

            EventSystem.current.SetSelectedGameObject(null);

            SetDisplayedOptions(Options);
            foreach (var option in Options)
                option.SetActive(true);
        }

        protected virtual void OptionSelected(string value)
        {
            InputField.text = value;
            Close();
        }
        protected virtual void Close() => Dropdown.SetActive(false);

        protected virtual void ShiftHighlightedOption(int shiftAmount)
        {
            DisplayedOptions[HighlightedOptionIndex].RemoveHighlight();
            SetHighlightedOption(HighlightedOptionIndex + shiftAmount);
        }

        protected virtual void SetDisplayedOptions(List<DropdownOption> displayedOptions)
        {
            Dropdown.SetActive(displayedOptions.Count > 0);
            if (DisplayedOptions.Count > 0)
                DisplayedOptions[HighlightedOptionIndex].RemoveHighlight();
            DisplayedOptions = displayedOptions;
            if (DisplayedOptions.Count == 0)
                return;

            SetHighlightedOption(0);
            // canvas must be updated to get the correct heights for determining whether to show the suggestion borders
            Canvas.ForceUpdateCanvases();
            SuggestionsBorders.SetActive(OptionsScrollRect.content.rect.height > OptionsScrollRect.viewport.rect.height);
        }

        protected virtual void SetHighlightedOption(int index)
        {
            HighlightedOptionIndex = index;
            var highlightedOption = DisplayedOptions[HighlightedOptionIndex];
            highlightedOption.Highlight();
            OptionsScrollRect.EnsureChildIsShowing((RectTransform)highlightedOption.transform);
        }
    }

    public abstract class BaseOptionsRetriever : MonoBehaviour
    {
        public abstract IEnumerable<string> GetOptions();
    }
}