using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(RectTransform))]
    public class UserTabDrawer : MonoBehaviour
    {
        public UserTabSelectorBehaviour UserTabSelector { get => userTabSelector; set => userTabSelector = value; }
        [SerializeField] private UserTabSelectorBehaviour userTabSelector;

        public RectTransform RectTransform => (RectTransform)transform;
        public UserTab Tab { get; protected set; }
        public virtual void ChangeTab(object sender, UserTabSelectedEventArgs eventArgs)
        {
            Tab = eventArgs.SelectedTab;
            UserTabSelector.Select(sender, eventArgs);
        }
    }
}
