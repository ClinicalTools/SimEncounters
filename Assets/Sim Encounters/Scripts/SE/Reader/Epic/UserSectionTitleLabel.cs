using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UserSectionTitleLabel : MonoBehaviour
    {
        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

        protected ISelectedListener<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<UserSectionSelectedEventArgs> userSectionSelector)
        {
            UserSectionSelector = userSectionSelector;
        }
        protected virtual void Start()
        {
            UserSectionSelector.Selected += OnSectionSelected;
            if (UserSectionSelector.CurrentValue != null)
                OnSectionSelected(UserSectionSelector, UserSectionSelector.CurrentValue);
        }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
            => Label.text = eventArgs.SelectedSection.Data.Name;

        protected virtual void OnDestroy() => UserSectionSelector.Selected -= OnSectionSelected;
    }
}