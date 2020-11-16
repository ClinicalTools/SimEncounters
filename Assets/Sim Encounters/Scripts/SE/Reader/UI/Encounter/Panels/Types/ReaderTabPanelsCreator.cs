using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTabPanelsCreator : MonoBehaviour
    {
        public virtual BaseChildUserPanelsDrawer PanelsDrawer { get => panelsDrawer; set => panelsDrawer = value; }
        [SerializeField] private BaseChildUserPanelsDrawer panelsDrawer;

        protected ISelector<UserTabSelectedEventArgs> TabSelector { get; set; }
        [Inject] public virtual void Inject(ISelector<UserTabSelectedEventArgs> tabSelector) => TabSelector = tabSelector;

        protected virtual void Start()
        {
            TabSelector.Selected += OnTabSelected;
            if (TabSelector.CurrentValue != null)
                OnTabSelected(TabSelector, TabSelector.CurrentValue);
        }
        protected virtual void OnDestroy() => TabSelector.Selected -= OnTabSelected;

        private void OnTabSelected(object sender, UserTabSelectedEventArgs e)
            => PanelsDrawer.Display(e.SelectedTab.Panels, e.ChangeType != ChangeType.Inactive);
    }
}