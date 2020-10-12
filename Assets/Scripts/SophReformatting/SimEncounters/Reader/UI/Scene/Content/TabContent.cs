using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class TabContent
    {
        public MonoBehaviour Behaviour { get; }
        public GameObject GameObject => Behaviour.gameObject;
        public RectTransform RectTransform => (RectTransform)Behaviour.transform;
        private UserTab tab;
        public UserTab Tab {
            get => tab;
            set {
                tab = value;
                if (tab != null && Behaviour is IUserTabDrawer tabDrawer)
                    tabDrawer.Display(tab);
            }
        }

        public TabContent(MonoBehaviour behaviour) => Behaviour = behaviour;
    }
}
