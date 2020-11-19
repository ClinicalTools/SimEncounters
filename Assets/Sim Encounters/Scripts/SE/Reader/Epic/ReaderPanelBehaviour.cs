using ClinicalTools.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseChildUserPanelsDrawer : MonoBehaviour
    {
        public abstract void Display(OrderedCollection<UserPanel> panels, bool active);
    }

    public class ReaderPanelBehaviour : BaseReaderPanelBehaviour
    {
        protected override BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer = null;
        protected override BaseUserPinGroupDrawer PinsDrawer { get => pinsDrawer; }
        [SerializeField] private BaseUserPinGroupDrawer pinsDrawer = null;
        public virtual bool SetReadOnSelect { get => setReadOnSelect; set => setReadOnSelect = value; }
        [SerializeField] private bool setReadOnSelect = true;
        public override void Select(object sender, UserPanelSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            if (SetReadOnSelect && eventArgs.Active && !CurrentPanel.IsRead() && !CurrentPanel.HasChildren())
                CurrentPanel.SetRead(true);
        }
    }

    public abstract class BaseReaderPanelBehaviour : UserPanelSelectorBehaviour
    {
        protected abstract BaseChildUserPanelsDrawer ChildPanelsDrawer { get; }
        protected abstract BaseUserPinGroupDrawer PinsDrawer { get; }
        public virtual string Type { get => type; set => type = value; }
        [SerializeField] private string type;

        protected UserPanel CurrentPanel { get; set; }

        public override void Select(object sender, UserPanelSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            CurrentPanel = eventArgs.SelectedPanel;

            if (ChildPanelsDrawer != null && CurrentPanel.ChildPanels?.Count > 0)
                ChildPanelsDrawer.Display(eventArgs.SelectedPanel.ChildPanels, eventArgs.Active);
            if (PinsDrawer != null)
                PinsDrawer.Display(eventArgs.SelectedPanel.PinGroup);

            if (eventArgs.Active)
                eventArgs.SelectedPanel.SetRead(true);
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, BaseReaderPanelBehaviour> { }
    }

}