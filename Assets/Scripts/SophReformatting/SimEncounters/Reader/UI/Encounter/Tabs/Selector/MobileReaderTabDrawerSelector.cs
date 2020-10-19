using ClinicalTools.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MobileReaderTabDrawerSelector : MonoBehaviour
    {
        public Transform TabParent { get => tabParent; set => tabParent = value; }
        [SerializeField] private Transform tabParent;
        public TMP_Text TabName { get => tabName; set => tabName = value; }
        [SerializeField] private TMP_Text tabName;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;
        public ScrollRectGradient ScrollGradient { get => scrollGradient; set => scrollGradient = value; }
        [SerializeField] private ScrollRectGradient scrollGradient;


        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            UserTabSelector = userTabSelector;
            UserTabSelector.AddSelectedListener(OnTabSelected);
        }

        protected virtual void OnDestroy() => UserTabSelector?.RemoveSelectedListener(OnTabSelected);

        protected BaseUserTabDrawer CurrentTabDrawer { get; set; }
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTabDrawer != null)
                Destroy(CurrentTabDrawer.gameObject);

            if (ScrollRect != null)
                ScrollRect.verticalNormalizedPosition = 1;
            if (ScrollGradient != null)
                ScrollGradient.ResetGradients();

            var tab = eventArgs.SelectedTab;
            TabName.text = tab.Data.Name;
            var prefab = GetTabPrefab(tab.Data);
            CurrentTabDrawer = Instantiate(prefab, TabParent);
            CurrentTabDrawer.Display(eventArgs);
        }

        protected virtual BaseUserTabDrawer GetTabPrefab(Tab tab)
        {
            var tabFolder = $"soph/se/Mobile/Reader/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefabGameObject = Resources.Load(tabPrefabPath) as GameObject;
            return tabPrefabGameObject.GetComponent<BaseUserTabDrawer>();
        }
    }
}