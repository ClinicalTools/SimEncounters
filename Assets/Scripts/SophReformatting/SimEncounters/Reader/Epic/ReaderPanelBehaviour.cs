using ClinicalTools.SimEncounters.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseChildUserPanelsDrawer : MonoBehaviour
    {
        public abstract void Display(OrderedCollection<UserPanel> panels, bool active);
    }

    public class ReaderPanelBehaviour : UserPanelSelectorBehaviour
    {
        public virtual BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; set => childPanelsDrawer = value; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer;
        public virtual BaseUserPinGroupDrawer PinsDrawer { get => pinsDrawer; set => pinsDrawer = value; }
        [SerializeField] private BaseUserPinGroupDrawer pinsDrawer;
        public virtual bool SetReadOnSelect { get => setReadOnSelect; set => setReadOnSelect = value; }
        [SerializeField] private bool setReadOnSelect = true;
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

            if (SetReadOnSelect && eventArgs.Active && !CurrentPanel.IsRead() && !CurrentPanel.HasChildren())
                CurrentPanel.SetRead(true);
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, ReaderPanelBehaviour> { }
    }
    public abstract class BaseReaderPanelBehaviour : UserPanelSelectorBehaviour
    {
        public virtual BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; set => childPanelsDrawer = value; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer;
        public virtual BaseUserPinGroupDrawer PinsDrawer { get => pinsDrawer; set => pinsDrawer = value; }
        [SerializeField] private BaseUserPinGroupDrawer pinsDrawer;
        public virtual bool SetReadOnSelect { get => setReadOnSelect; set => setReadOnSelect = value; }
        [SerializeField] private bool setReadOnSelect = true;
        public virtual string Type { get => type; set => type = value; }
        [SerializeField] private string type;

        public override void Select(object sender, UserPanelSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            var userPanel = eventArgs.SelectedPanel;

            if (ChildPanelsDrawer != null && userPanel.ChildPanels?.Count > 0)
                ChildPanelsDrawer.Display(eventArgs.SelectedPanel.ChildPanels, eventArgs.Active);
            if (PinsDrawer != null)
                PinsDrawer.Display(eventArgs.SelectedPanel.PinGroup);

            if (SetReadOnSelect && eventArgs.Active && !userPanel.IsRead() && !userPanel.HasChildren())
                userPanel.SetRead(true);
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, ReaderPanelBehaviour> { }
    }
}