using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UserTabTitleLabel : MonoBehaviour
    {
        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label
        {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            UserTabSelector = userTabSelector;
            UserTabSelector.AddSelectedListener(OnTabSelected);
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
            => Label.text = eventArgs.SelectedTab.Data.Name;

        protected virtual void OnDestroy() => UserTabSelector?.RemoveSelectedListener(OnTabSelected);
    }
}