using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class TabContent
    {
        public MonoBehaviour Behaviour { get; }
        public GameObject GameObject => Behaviour.gameObject;
        public RectTransform RectTransform => (RectTransform)Behaviour.transform;
        public UserTab Tab { get; protected set; }
        public virtual void ChangeTab(UserTabSelectedEventArgs eventArgs)
        {
            Tab = eventArgs.SelectedTab;
            if (Tab != null && Behaviour is IUserTabDrawer tabDrawer)
                tabDrawer.Display(eventArgs);
        }

        public TabContent(MonoBehaviour behaviour) => Behaviour = behaviour;
    }
}
