using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

        protected BaseMessageHandler MessageHandler { get; set; }
        [Inject] public virtual void Inject(BaseMessageHandler messageHandler) => MessageHandler = messageHandler;

        public virtual WaitableResult<Tab> CreateTab()
        {
            CurrentWaitableTab?.SetError("New popup opened");
            CurrentWaitableTab = new WaitableResult<Tab>();

            gameObject.SetActive(true);
            TabGroups.allowSwitchOff = true;
            TabTypes.allowSwitchOff = true;
            foreach (Transform child in TabTypes.transform)
                Destroy(child.gameObject);
            if (SelectedGroupButton != null)
                SelectedGroupButton.Deselect();
            DescriptionLabel.text = "";

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
            if (string.IsNullOrEmpty(name)) {
                MessageHandler.ShowMessage("Name cannot be empty.", MessageType.Error);
                return;
            }

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
                var groupButton = Instantiate(TypeButtonPrefab, TabGroups.transform);
                groupButton.Label.text = group.Key;
                groupButton.Toggle.group = TabGroups;
                groupButton.Toggle.AddOnSelectListener(() => GroupSelected(groupButton, group.Value));
            }
        }

        protected virtual TabTypeButtonUI SelectedGroupButton { get; set; }
        protected virtual void GroupSelected(TabTypeButtonUI groupButton, List<TabType> tabTypes)
        {
            SelectedGroupButton = groupButton;
            TabGroups.allowSwitchOff = false;
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
            TabTypes.allowSwitchOff = false;
            DescriptionLabel.text = tabType.Description;
            SelectedPrefab = tabType.Prefab;
        }
    }
}