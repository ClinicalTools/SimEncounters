using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTabDrawerSelector : BaseUserTabDrawer
    {
        public Transform TabParent { get => tabParent; set => tabParent = value; }
        [SerializeField] private Transform tabParent;

        protected BaseUserTabDrawer CurrentTabDrawer { get; set; }
        public override void Select(UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTabDrawer != null)
                Destroy(CurrentTabDrawer.gameObject);

            var prefab = GetTabPrefab(eventArgs.SelectedTab.Data);
            CurrentTabDrawer = Instantiate(prefab, TabParent);
            CurrentTabDrawer.Select(eventArgs);
        }

        protected virtual BaseUserTabDrawer GetTabPrefab(Tab tab)
        {
            var tabFolder = $"Reader/Prefabs/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefabGameObject = Resources.Load(tabPrefabPath) as GameObject;
            return tabPrefabGameObject.GetComponent<BaseUserTabDrawer>();
        }
    }
}