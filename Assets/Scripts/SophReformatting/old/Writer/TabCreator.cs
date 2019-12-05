using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    // I'd make this a struct, but as it's vague what exact ways Sim Encounters should be expandable,
    // I really want everything to be able to be overridable.
    public class TabInfo
    {
        public virtual string Category { get; protected set; }
        public virtual string Display { get; protected set; }
        public virtual string Prefab { get; protected set; }
        public virtual string Description { get; protected set; } = "";

        public TabInfo(string category, string display, string prefab, string description = null)
        {
            Category = category;
            Display = display;
            Prefab = prefab;
            if (description != null)
                Description = description.Trim();
        }
    }

    public class TabCreator : MonoBehaviour
    {
        private const string tabsFileName = "Tabs.tsv";
        private string TabsPath => Application.streamingAssetsPath + "/Instructions/" + tabsFileName;
        private char splitChar = '\t';

        [SerializeField] private GameObject AddTabButton;

        // maybe this should be more abstractable
        protected enum ColumnIndices
        {
            Category = 0,
            Display = 1,
            Prefab = 2,
            Description = 3
        };

        protected virtual List<TabInfo> GetTabInfoList()
        {
            string[] lines = File.ReadAllLines(TabsPath);
            var tabInfoList = new List<TabInfo>();

            // First line is just header, so skip
            for (int i = 1; i < lines.Length; i++) {
                var splitLine = lines[i].Split(splitChar);
                var tabInfo = GetTabInfo(splitLine);
                if (tabInfo != null)
                    tabInfoList.Add(tabInfo);
            }

            return tabInfoList;
        }

        protected virtual TabInfo GetTabInfo(string[] splitLine)
        {
            // don't really like the assumption that description is bigger than all the others, 
            // but there's no real reason to be more thorough since the enum values won't change
            if (splitLine.Length < (int)ColumnIndices.Description)
                return null;

            var category = splitLine[(int)ColumnIndices.Category];
            var display = splitLine[(int)ColumnIndices.Display];
            var prefab = splitLine[(int)ColumnIndices.Prefab];
            if (splitLine.Length > (int)ColumnIndices.Description)
                return new TabInfo(category, display, prefab, splitLine[(int)ColumnIndices.Description]);
            else
                return new TabInfo(category, display, prefab);
        }

        [SerializeField] private ToggleGroup categoryGroup;
        protected virtual ToggleGroup CategoryGroup => categoryGroup;
        [SerializeField] private ToggleGroup tabGroup;
        protected virtual ToggleGroup TabGroup => tabGroup;

        // what does "tab template" mean???

        [SerializeField] private Object tabSelectorPrefab = Resources.Load("Writer/Prefabs/Panels/TabTemplatePanel");
        protected virtual Object TabTemplatePanel => tabSelectorPrefab;

        protected virtual void Start()
        {
            var tabInfoList = GetTabInfoList();

            var categories = new HashSet<string>();

            // using a for instead of a foreach, so the variable can be declared in the loop and used in delegates
            // TODO: make sure that's important
            for (int i = 0; i < tabInfoList.Count; i++) {
                var tabInfo = tabInfoList[i];

                if (tabInfo.Category.Equals("END OF FILE"))
                    break;

                if (tabInfo.Category.Equals("N/A")) { // create a tab type button in the category section 
                    var tabSelector = CreateTabSelector(CategoryGroup, tabInfo.Display);

                    tabSelector.onValueChanged.AddListener((on) => {
                        if (on) TabSelected(tabInfo);
                    });
                } else if (!categories.Contains(tabInfo.Category)) {
                    categories.Add(tabInfo.Category);

                    var categorySelector = CreateTabSelector(CategoryGroup, tabInfo.Category);
                    categorySelector.transform.Find("Selected/RightArrow").gameObject.SetActive(true);

                    categorySelector.onValueChanged.AddListener((on) => {
                        if (on) ExpandCategory(categorySelector, tabInfo.Category, tabInfoList);
                    });
                }
            }
        }

        protected virtual string ToggleTitlePath => "Selected/TabTemplateTitle";
        protected virtual Toggle CreateTabSelector(ToggleGroup toggleGroup, string name)
        {
            GameObject baseObj = Instantiate(TabTemplatePanel, toggleGroup.transform) as GameObject;
            baseObj.name = name;
            baseObj.transform.Find(ToggleTitlePath).GetComponent<TextMeshProUGUI>().text = name;
            var toggle = baseObj.GetComponent<Toggle>();
            toggle.group = toggleGroup;
            return toggle;
        }

        protected virtual TabInfo SelectedTabInfo { get; set; }
        [SerializeField] private TextMeshProUGUI tabDescriptionLabel;
        protected virtual TextMeshProUGUI TabDescriptionLabel => tabDescriptionLabel;
        public void ExpandCategory(Toggle categoryToggle, string category, List<TabInfo> tabInfoList)
        {
            // TODO: check back over these "allowSwitchOffs"
            var toggleGroup = categoryToggle.group;
            if (toggleGroup)
                toggleGroup.allowSwitchOff = false;

            if (!categoryToggle.isOn)
                return;

            toggleGroup.allowSwitchOff = true;

            SelectedTabInfo = null;
            TabDescriptionLabel.text = "";

            foreach (Transform child in TabGroup.transform)
                Destroy(child.gameObject);

            for (int i = 0; i < tabInfoList.Count; i++) {
                var tabInfo = tabInfoList[i];

                if (category == tabInfo.Category) {
                    var tabSelector = CreateTabSelector(TabGroup, tabInfo.Display);

                    tabSelector.onValueChanged.AddListener((on) => {
                        if (on) TabSelected(tabInfo);
                    });
                }
            }
        }

        protected virtual void TabSelected(TabInfo tabInfo)
        {
            SelectedTabInfo = tabInfo;
            TabDescriptionLabel.text = $"<b>{tabInfo.Prefab}</b>\n{tabInfo.Display}";
        }


        /**
         * Adds a tab from the TabSelectorBG
         * Pass in the display name
         */
        public void AddTab(TextMeshProUGUI tabNameField)
        {
            var tabCustomName = GetCustomTabName(tabNameField);

            var section = TabManager.Instance.GetCurrentSection();
            var tempTabList = section.GetTabList();
            if (tempTabList.Contains(tabCustomName)) {
                WriterHandler.WriterInstance.ShowMessage("Cannot name two tabs the same in one step!", true);
                throw new System.Exception("Cannot name two tabs the same in one step!");
            }

            section.AddData(SelectedTabInfo.Prefab, tabCustomName, null);
            var newTab = CreateTabButton(tabCustomName);
            newTab.ChangeTab();
            Destroy(gameObject);
        }

        public string GetCustomTabName(TextMeshProUGUI tabNameField)
        {
            var tabName = tabNameField.text.Replace(GlobalData.EMPTY_WIDTH_SPACE + "", "");

            // when I refactor tabs more, hopefully I can remove these restrictions
            var writer = WriterHandler.WriterInstance;
            if (!WriterHandler.WriterInstance.IsValidName(tabName, "Tab"))
                throw new System.Exception("Name not valid: Please rename your tab");

            string tabCustomName;
            //Do not remove last compare below. It's looking for a zero width space (unicode 8203 / Alt+200B)
            if (string.IsNullOrWhiteSpace(tabNameField.text))
                tabCustomName = SelectedTabInfo.Prefab;
            else
                tabCustomName = tabNameField.text;


            return tabCustomName;
        }

        protected GameObject TabButtonContentPar;
        public SwapTabScript CreateTabButton(string tabName)
        {
            // this can't be the only place tab buttons are loaded
            GameObject newTab = Resources.Load(GlobalData.resourcePath + "/Prefabs/TabButton") as GameObject;

            // change to tab
            TabButtonContentPar = EncounterHandler.Instance.transform.Find("ContentPanel/TabButtonsPanel/Scroll View/Viewport/TabButtonContent").gameObject;
            //The button's position
            newTab = Instantiate(newTab, TabButtonContentPar.transform);
            newTab.name = tabName;
            var tabSwapper = newTab.GetComponent<SwapTabScript>();
            tabSwapper.TabKey = tabName;
            tabSwapper.SetName(tabName);

            return tabSwapper;
        }
    }
}