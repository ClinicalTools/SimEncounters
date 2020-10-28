using ClinicalTools.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabScrollView : MonoBehaviour
    {
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;
        public ScrollRectGradient ScrollGradient { get => scrollGradient; set => scrollGradient = value; }
        [SerializeField] private ScrollRectGradient scrollGradient;

        protected ISelectedListener<TabSelectedEventArgs> UserTabSelector { get; set; }
        [Inject] public virtual void Inject(ISelectedListener<TabSelectedEventArgs> userTabSelector) => UserTabSelector = userTabSelector;
        
        protected virtual void Start() => UserTabSelector.AddSelectedListener(OnTabSelected);
        protected virtual void OnDestroy() => UserTabSelector?.RemoveSelectedListener(OnTabSelected);

        protected BaseUserTabDrawer CurrentTabDrawer { get; set; }
        protected virtual void OnTabSelected(object sender, TabSelectedEventArgs eventArgs)
        {
            if (ScrollRect != null)
                ScrollRect.verticalNormalizedPosition = 1;
            if (ScrollGradient != null)
                ScrollGradient.ResetGradients();
        }
    }
}