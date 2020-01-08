using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    internal class TabCreator : IApply<Tab>
    {
        protected virtual TabCreatorUI SectionCreatorUI { get; }

        public event Action<Tab> Apply;

        protected string SelectedPrefab { get; set; }

        public TabCreator(TabCreatorUI tabCreatorUI, EncounterWriter writer)
        {
            AddListeners(tabCreatorUI);
            AddCategories(tabCreatorUI, writer.TabTypes.Groups);
        }

        protected virtual void AddListeners(TabCreatorUI tabCreatorUI)
        {
            tabCreatorUI.CancelButton.onClick.AddListener(() => Close(tabCreatorUI));
            tabCreatorUI.CreateButton.onClick.AddListener(() => AddTab(tabCreatorUI));
        }

        protected virtual void AddTab(TabCreatorUI tabCreatorUI)
        {
            var name = tabCreatorUI.NameField.text;

            var tab = new Tab(SelectedPrefab, name);
            Apply?.Invoke(tab);

            Close(tabCreatorUI);
        }

        protected virtual void Close(TabCreatorUI tabCreatorUI)
        {
            UnityEngine.Object.Destroy(tabCreatorUI.gameObject);
        }

        public void AddCategories(TabCreatorUI tabCreatorUI, Dictionary<string, List<TabType>> tabTypeGroups)
        {
            foreach (var group in tabTypeGroups) {
                // I feel like I should avoid .transform and .gameObject because it inherently binds the component to being attached at the point of entry
                // While I can't think of a reason it would be otherwise, this is less expandable, although the extra complexity is probably not worth it while we maintain control over the code
                var tabButton = UnityEngine.Object.Instantiate(tabCreatorUI.TypeButtonPrefab, tabCreatorUI.TabGroups.transform);
                tabButton.Label.text = group.Key;
                tabButton.Toggle.group = tabCreatorUI.TabGroups;
                tabButton.Toggle.AddOnSelectListener(() => GroupSelected(tabCreatorUI, group.Value));
            }
        }

        protected virtual void GroupSelected(TabCreatorUI tabCreatorUI, List<TabType> tabTypes)
        {
            foreach (Transform child in tabCreatorUI.TabTypes.transform)
                UnityEngine.Object.Destroy(child.gameObject);

            tabCreatorUI.TabTypes.allowSwitchOff = true;
            tabCreatorUI.DescriptionLabel.text = "";

            foreach (var tabType in tabTypes) {
                var tabButton = UnityEngine.Object.Instantiate(tabCreatorUI.TypeButtonPrefab, tabCreatorUI.TabTypes.transform);
                tabButton.Label.text = tabType.Display;
                tabButton.Toggle.group = tabCreatorUI.TabTypes;
                tabButton.Toggle.AddOnSelectListener(() => TypeSelected(tabCreatorUI, tabType));
            }
        }
        
        protected virtual void TypeSelected(TabCreatorUI tabCreatorUI, TabType tabType)
        {
            tabCreatorUI.DescriptionLabel.text = tabType.Description;
            SelectedPrefab = tabType.Prefab;
        }
    }

}