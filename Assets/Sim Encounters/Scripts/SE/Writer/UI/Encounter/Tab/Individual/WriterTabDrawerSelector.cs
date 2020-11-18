using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterTabDrawerSelector : BaseTabDrawer
    {
        public Transform TabParent { get => tabParent; set => tabParent = value; }
        [SerializeField] private Transform tabParent;

        protected Factory TabDrawerFactory { get; set; }
        [Inject] public virtual void Inject(Factory tabDrawerFactory) => TabDrawerFactory = tabDrawerFactory;

        protected BaseTabDrawer CurrentTabDrawer { get; set; }
        public override void Display(Encounter encounter, Tab tab)
        {
            if (CurrentTabDrawer != null)
                Destroy(CurrentTabDrawer.gameObject);
            if (tab == null) {
                CurrentTabDrawer = null;
                return;
            }
            
            CurrentTabDrawer = TabDrawerFactory.Create(GetTabPath(tab));
            CurrentTabDrawer.transform.SetParent(TabParent);
            CurrentTabDrawer.transform.localScale = Vector3.one;
            ((RectTransform)CurrentTabDrawer.transform).offsetMin = Vector2.zero;
            ((RectTransform)CurrentTabDrawer.transform).offsetMax = Vector2.zero;

            CurrentTabDrawer.Display(encounter, tab);
        }

        protected virtual string GetTabPath(Tab tab)
        {
            var tabFolder = $"Prefabs/Desktop/Writer/Tabs/{tab.Type} Tab/";
            return $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
        }

        public override Tab Serialize() => CurrentTabDrawer.Serialize();
    }
}