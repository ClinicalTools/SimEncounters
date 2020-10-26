using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class PanelPinButtonsDrawer : MonoBehaviour
    {
        protected BaseUserPinGroupDrawer PinGroupDrawer { get; set; }
        protected BaseUserPinGroupDrawer.Pool PinGroupDrawerPool { get; set; }
        protected ISelectedListener<UserPanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            BaseUserPinGroupDrawer.Pool pinGroupDrawerPool,
            ISelectedListener<UserPanelSelectedEventArgs> panelSelectedListener)
        {
            PinGroupDrawerPool = pinGroupDrawerPool;
            PanelSelectedListener = panelSelectedListener;
        }

        protected virtual void Start() => PanelSelectedListener.AddSelectedListener(OnPanelSelected);
        protected virtual void OnDestroy()
        {
            if (PinGroupDrawer != null)
                PinGroupDrawerPool.Despawn(PinGroupDrawer);
            PanelSelectedListener.RemoveSelectedListener(OnPanelSelected);
        }
        protected virtual void OnPanelSelected(object sender, UserPanelSelectedEventArgs panel)
        {
            if (PinGroupDrawer != null)
                PinGroupDrawerPool.Despawn(PinGroupDrawer);
            var pins = panel.SelectedPanel.Data.Pins;
            if (pins == null || (pins.Dialogue == null && pins.Quiz == null))
                return;

            PinGroupDrawer = PinGroupDrawerPool.Spawn();
            PinGroupDrawer.transform.SetParent(transform);
            PinGroupDrawer.transform.localScale = Vector3.one;
            PinGroupDrawer.transform.SetAsLastSibling();
            PinGroupDrawer.GetComponent<HorizontalOrVerticalLayoutGroup>().enabled = true;
            PinGroupDrawer.Display(panel.SelectedPanel.PinGroup);
        }
    }
}