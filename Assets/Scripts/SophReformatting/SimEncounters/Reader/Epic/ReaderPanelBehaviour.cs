using ClinicalTools.SimEncounters.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseChildUserPanelsDrawer : MonoBehaviour
    {
        public abstract void Display(OrderedCollection<UserPanel> panels, bool active);
    }
    public abstract class BaseChildUserPinsDrawer : MonoBehaviour
    {
        public abstract void Display(UserPinGroup pins);
    }

    public class ReaderPanelBehaviour : UserPanelSelectorBehaviour
    {
        public virtual BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; set => childPanelsDrawer = value; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer;
        public virtual BaseChildUserPinsDrawer PinsDrawer { get => pinsDrawer; set => pinsDrawer = value; }
        [SerializeField] private BaseChildUserPinsDrawer pinsDrawer;
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
            if (PinsDrawer != null && userPanel.ChildPanels?.Count > 0)
                PinsDrawer.Display(eventArgs.SelectedPanel.PinGroup);

            if (SetReadOnSelect && eventArgs.Active && !userPanel.IsRead() && !userPanel.HasChildren())
                userPanel.SetRead(true);
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, ReaderPanelBehaviour> { }
    }
}