using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTab
    {
        private readonly string tabFolder;
        public WriterTab(Transform tabContent, Tab tab, EncounterWriter writer)
        {
            tabFolder = $"Writer/Prefabs/Tabs/{tab.Type.Replace("_", " ")} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace("_", string.Empty)}Tab";
            Debug.LogError(tabPrefabPath);
            var tabPrefab = Resources.Load(tabPrefabPath) as GameObject;
            var newTab = Object.Instantiate(tabPrefab, tabContent.transform).GetComponent<TabUI>();

            Debug.LogError(newTab);
            Debug.LogError(newTab.PanelsParent);
            Deserialize(newTab.PanelsParent, tab);
        }

        public void Deserialize(Transform panelsParent, Tab tab)
        {
            foreach (var panel in tab.Panels) {
                var panelPrefabPath = $"{tabFolder}{panel.Value.Type}";
                Debug.LogError(panelPrefabPath);
                var panelPrefab = Resources.Load(panelPrefabPath) as GameObject;
                Debug.LogError(panelPrefab);
                Debug.LogError(panelsParent);
                var newPrefab = Object.Instantiate(panelPrefab, panelsParent).GetComponent<PanelUI>();
            }
        }

        public void Serialize()
        {

        }
    }
}