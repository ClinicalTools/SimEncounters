using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Extensions;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabCreatorPopup : MonoBehaviour
    {
        public List<Button> CancelButtons { get => cancelButtons; set => cancelButtons = value; }
        [SerializeField] private List<Button> cancelButtons;
        public Button CreateButton { get => createButton; set => createButton = value; }
        [SerializeField] private Button createButton;

        public TMP_InputField NameField { get => nameField; set => nameField = value; }
        [SerializeField] private TMP_InputField nameField;

        public ToggleGroup TabGroups { get => tabGroups; set => tabGroups = value; }
        [SerializeField] private ToggleGroup tabGroups;
        public ToggleGroup TabTypes { get => tabTypes; set => tabTypes = value; }
        [SerializeField] private ToggleGroup tabTypes;

        public TextMeshProUGUI DescriptionLabel { get => descriptionLabel; set => descriptionLabel = value; }
        [SerializeField] private TextMeshProUGUI descriptionLabel;

        public TabTypeButtonUI TypeButtonPrefab { get => typeButtonPrefab; set => typeButtonPrefab = value; }
        [SerializeField] private TabTypeButtonUI typeButtonPrefab;

        protected WaitableResult<Tab> CurrentWaitableTab { get; set; }

        public virtual WaitableResult<Tab> CreateTab()
        {
            CurrentWaitableTab?.SetError("New popup opened");
            CurrentWaitableTab = new WaitableResult<Tab>();

            gameObject.SetActive(true);

            return CurrentWaitableTab;
        }

        protected virtual void Awake()
        {
            foreach (var cancelButton in CancelButtons)
                cancelButton.onClick.AddListener(() => Close());

            CreateButton.onClick.AddListener(() => AddTab());

            var tabTypesInfo = new TabTypesInfo();
            AddCategories(tabTypesInfo.Groups);
        }

        protected string SelectedPrefab { get; set; }
        protected virtual void AddTab()
        {
            var name = NameField.text;

            var tab = new Tab(SelectedPrefab, name);
            CurrentWaitableTab.SetResult(tab);
            CurrentWaitableTab = null;

            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableTab != null) {
                CurrentWaitableTab.SetError("Canceled");
                CurrentWaitableTab = null;
            }
            gameObject.SetActive(false);
        }

        public void AddCategories(Dictionary<string, List<TabType>> tabTypeGroups)
        {
            foreach (var group in tabTypeGroups) {
                var tabButton = Instantiate(TypeButtonPrefab, TabGroups.transform);
                tabButton.Label.text = group.Key;
                tabButton.Toggle.group = TabGroups;
                tabButton.Toggle.AddOnSelectListener(() => GroupSelected(group.Value));
            }
        }

        protected virtual void GroupSelected(List<TabType> tabTypes)
        {
            foreach (Transform child in TabTypes.transform)
                Destroy(child.gameObject);

            TabTypes.allowSwitchOff = true;
            DescriptionLabel.text = "";

            foreach (var tabType in tabTypes) {
                var tabButton = Instantiate(TypeButtonPrefab, TabTypes.transform);
                tabButton.Label.text = tabType.Display;
                tabButton.Toggle.group = TabTypes;
                tabButton.Toggle.AddOnSelectListener(() => TypeSelected(tabType));
            }
        }

        protected virtual void TypeSelected(TabType tabType)
        {
            DescriptionLabel.text = tabType.Description;
            SelectedPrefab = tabType.Prefab;
        }
    }
    public class TabType
    {
        public string Display { get; }
        public string Prefab { get; }
        public virtual string Description { get; }

        public TabType(string display, string prefab, string description)
        {
            Display = display;
            Prefab = prefab;
            Description = $"<b>{display}</b>\n{description}";
        }
    }
    public class TabTypesInfo
    {
        protected virtual int ColumnCount { get; } = 4;
        protected enum ColumnIndices
        {
            Category = 0,
            Display = 1,
            Prefab = 2,
            Description = 3
        };
        // First line is just header, so first data line has index of 1
        protected virtual int FirstLineIndex { get; } = 1;

        protected virtual string TabsFileName { get; } = "Tabs.tsv";
        protected virtual string TabsPath => Application.streamingAssetsPath + "/Instructions/" + TabsFileName;
        protected virtual char SplitChar => '\t';

        public Dictionary<string, List<TabType>> Groups { get; } = new Dictionary<string, List<TabType>>();

        public TabTypesInfo()
        {
            AddGroups();
        }

        protected virtual void AddGroups()
        {
            string[] lines = File.ReadAllLines(TabsPath);

            for (int i = FirstLineIndex; i < lines.Length; i++)
                ProcessLine(lines[i]);
        }

        protected virtual void ProcessLine(string line)
        {
            var splitLine = line.Split(SplitChar);
            if (!ValidLine(splitLine))
                return;

            var tabType = GetTabType(splitLine);
            AddToCategory(splitLine, tabType);
        }

        protected virtual bool ValidLine(string[] splitLine)
        {
            return splitLine.Length >= ColumnCount;
        }

        protected virtual TabType GetTabType(string[] splitLine)
        {
            // don't really like the assumption that description is bigger than all the others, 
            // but there's no real reason to be more thorough since the enum values won't change
            if (splitLine.Length <= (int)ColumnIndices.Description)
                return null;

            var display = splitLine[(int)ColumnIndices.Display];
            var prefab = splitLine[(int)ColumnIndices.Prefab];
            var description = splitLine[(int)ColumnIndices.Description];

            return new TabType(display, prefab, description);
        }

        protected virtual void AddToCategory(string[] splitLine, TabType tabType)
        {
            var category = splitLine[(int)ColumnIndices.Category];
            if (!Groups.ContainsKey(category))
                Groups.Add(category, new List<TabType>());

            Groups[category].Add(tabType);
        }
    }
}