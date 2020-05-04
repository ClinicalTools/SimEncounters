using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
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

            var prefab = GetTabPrefab(tab);
            CurrentTabDrawer = Instantiate(prefab, TabParent);
            CurrentTabDrawer.Display(encounter, tab);
        }

        protected virtual BaseTabDrawer GetTabPrefab(Tab tab)
        {
            var tabFolder = $"Writer/Prefabs/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefabGameObject = Resources.Load(tabPrefabPath) as GameObject;
            return tabPrefabGameObject.GetComponent<BaseTabDrawer>();
        }
    }
}