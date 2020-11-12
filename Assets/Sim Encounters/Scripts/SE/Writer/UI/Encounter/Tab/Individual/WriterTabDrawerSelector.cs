using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class WriterTabDrawerSelector : BaseTabDrawer
    {
        public Transform TabParent { get => tabParent; set => tabParent = value; }
        [SerializeField] private Transform tabParent;

        protected BaseTabDrawer CurrentTabDrawer { get; set; }
        public override void Display(Encounter encounter, Tab tab)
        {
            if (CurrentTabDrawer != null)
                Destroy(CurrentTabDrawer.gameObject);
            if (tab == null) {
                CurrentTabDrawer = null;
                return;
            }


            var prefab = GetTabPrefab(tab);
            CurrentTabDrawer = Instantiate(prefab, TabParent);
            CurrentTabDrawer.Display(encounter, tab);
        }

        protected virtual BaseTabDrawer GetTabPrefab(Tab tab)
        {
            var tabFolder = $"Prefabs/Desktop/Writer/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefabGameObject = Resources.Load(tabPrefabPath) as GameObject;
            return tabPrefabGameObject.GetComponent<BaseTabDrawer>();
        }

        public override Tab Serialize() => CurrentTabDrawer.Serialize();
    }
}