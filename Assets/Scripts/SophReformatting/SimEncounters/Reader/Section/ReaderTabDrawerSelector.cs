using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabDrawerSelector : UserTabDrawer
    {
        [SerializeField] private Transform tabParent;

        protected UserTabDrawer CurrentTabDrawer { get; set; }
        public override void Display(UserTab userTab)
        {
            if (CurrentTabDrawer != null)
                Destroy(CurrentTabDrawer.gameObject);

            var prefab = GetTabPrefab(userTab.Data);
            CurrentTabDrawer = Instantiate(prefab, tabParent);
            CurrentTabDrawer.Display(userTab);
        }

        protected virtual UserTabDrawer GetTabPrefab(Tab tab)
        {
            var tabFolder = $"Reader/Prefabs/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefabGameObject = Resources.Load(tabPrefabPath) as GameObject;
            return tabPrefabGameObject.GetComponent<UserTabDrawer>();
        }
    }
}