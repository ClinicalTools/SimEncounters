using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class DropdownTextfield : MonoBehaviour
    {
        protected static OptionRetriever OptionRetriever { get; } = new OptionRetriever();

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
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons;

        protected virtual List<DropdownOption> Options { get; } = new List<DropdownOption>();
        protected virtual int HighlightedOptionIndex { get; set; } = -1;
        protected virtual List<DropdownOption> DisplayedOptions { get; set; } = new List<DropdownOption>();

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Close);

            ShowOptionsButton.onClick.AddListener(ButtonPressed);
            InputField.onValueChanged.AddListener(TextChanged);

            var options = OptionRetriever.GetOptions();
            foreach (var option in options)
                AddOption(option);
        }

        protected virtual void Update()
        {
            if (!Dropdown.gameObject.activeSelf)
                return;

            if (DisplayedOptions.Count == 0)
                return;
            if (Input.GetKeyDown(KeyCode.DownArrow) && HighlightedOptionIndex < DisplayedOptions.Count - 1) {
                DisplayedOptions[HighlightedOptionIndex].RemoveHighlight();
                HighlightedOptionIndex++; 
                Highlight();
            } else if (Input.GetKeyDown(KeyCode.UpArrow) && HighlightedOptionIndex > 0) {
                DisplayedOptions[HighlightedOptionIndex].RemoveHighlight();
                HighlightedOptionIndex--;
                Highlight();
            } else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Tab)) {
                OptionSelected(DisplayedOptions[HighlightedOptionIndex].Value);
            }
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
            var showOptions = false;

            if (DisplayedOptions.Count > 0)
                DisplayedOptions[HighlightedOptionIndex].RemoveHighlight();
            DisplayedOptions = new List<DropdownOption>();
            HighlightedOptionIndex = 0;
            foreach (var option in Options) {
                var optionActive = option.Value.ToUpperInvariant().Contains(text.ToUpperInvariant());
                if (optionActive)
                    showOptions = true;
                option.SetActive(optionActive);
                DisplayedOptions.Add(option);
            }
            Dropdown.SetActive(showOptions);
            if (showOptions)
                DisplayedOptions[HighlightedOptionIndex].Highlight();
        }

        protected virtual void ButtonPressed()
        {
            if (Options.Count == 0)
                return;

            var showDropdown = !Dropdown.activeSelf;
            Dropdown.SetActive(showDropdown);
            if (!showDropdown)
                return;

            if (DisplayedOptions.Count > 0)
                DisplayedOptions[HighlightedOptionIndex].RemoveHighlight();
            DisplayedOptions = Options;
            HighlightedOptionIndex = 0;
            foreach (var option in Options)
                option.SetActive(true); 
            Highlight();
        }

        protected virtual void OptionSelected(string value)
        {
            InputField.text = value;
            Close();
        }
        protected virtual void Close() => Dropdown.SetActive(false);

        protected virtual void Highlight()
        {
            if (DisplayedOptions.Count <= HighlightedOptionIndex)
                return;
            var highlightedOption = DisplayedOptions[HighlightedOptionIndex];
            highlightedOption.Highlight();
            var optionCorners = new Vector3[4];
            ((RectTransform)highlightedOption.transform).GetWorldCorners(optionCorners);
            var viewportCorners = new Vector3[4];
            OptionsScrollRect.viewport.GetWorldCorners(viewportCorners);
            
            // check if bottom left corner is below the viewport
            if (optionCorners[0].y < viewportCorners[0].y) {
                var contentCorners = new Vector3[4];
                OptionsScrollRect.content.GetWorldCorners(contentCorners);
                var height = contentCorners[1].y - contentCorners[0].y;
                var y = 0;

                OptionsScrollRect.verticalNormalizedPosition = 0;
                
                Debug.LogWarning("below");

            // check if top left corner is above the viewport
            } else if (optionCorners[1].y > viewportCorners[1].y) {
                var contentCorners = new Vector3[4];
                OptionsScrollRect.content.GetWorldCorners(contentCorners);
                var height = contentCorners[1].y - contentCorners[0].y;
                OptionsScrollRect.verticalNormalizedPosition = 1 - 
                    (contentCorners[1].y - optionCorners[1].y) / OptionsScrollRect.content.rect.height;


                Debug.LogWarning("above");
            }
        }
    }

    public class OptionRetriever
    {
        IEnumerable<string> options;

        public virtual IEnumerable<string> GetOptions()
        {
            if (options == null)
                options = InitializeOptions();

            return options;
        }

        protected virtual string[] Filenames { get; } = new string[] {
            "Complete Blood Count.csv",
            "Comprehensive Metabolic Panel.csv",
            "Lipid Panel.csv"
        };

        protected virtual IEnumerable<string> InitializeOptions()
        {
            var options = new List<string>();
            foreach (var filename in Filenames)
                options.AddRange(GetOptions(GetFilepath(filename)));
            options.Sort();
            return options;
        }

        protected string Folder { get; } = "Medical Panels";
        protected virtual string GetFilepath(string filename)
            => Path.Combine(Application.streamingAssetsPath, Folder, filename);

        protected virtual IEnumerable<string> GetOptions(string filePath)
        {
            var options = new List<string>();
            var reader = new StreamReader(filePath);
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                if (line == null || line.Length == 0)
                    continue;

                var parts = line.Split(',');
                options.Add(parts[0]);
            }

            return options;
        }
    }
}