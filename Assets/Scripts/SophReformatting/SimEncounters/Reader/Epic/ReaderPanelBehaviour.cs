using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelBehaviour : UserPanelSelectorBehaviour {
        public virtual string Type { get => type; set => type = value; }
        [SerializeField] private string type;

        public override void Select(object sender, UserPanelSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            var userPanel = eventArgs.SelectedPanel;
            if (eventArgs.ChangeType != ChangeType.Inactive && !userPanel.IsRead() && !userPanel.HasChildren())
                userPanel.SetRead(true);
        }

        public class Factory : PlaceholderFactory<Object, ReaderPanelBehaviour> { }
    }
}