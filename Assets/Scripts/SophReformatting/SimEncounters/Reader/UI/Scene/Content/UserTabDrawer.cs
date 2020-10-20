using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(RectTransform))]
    public class UserTabDrawer : MonoBehaviour
    {
        public ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        [Inject] public virtual void Inject(ISelector<UserTabSelectedEventArgs> userTabSelector) => UserTabSelector = userTabSelector;

        public RectTransform RectTransform => (RectTransform)transform;
        public UserTab Tab { get; protected set; }
        public virtual void ChangeTab(object sender, UserTabSelectedEventArgs eventArgs)
        {
            Tab = eventArgs.SelectedTab;
            UserTabSelector.Select(sender, eventArgs);
        }
    }
}
