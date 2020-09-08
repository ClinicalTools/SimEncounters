using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSidebarThing : MonoBehaviour
    {
        public Animator SidebarAnimator { get => sidebarAnimator; set => sidebarAnimator = value; }
        [SerializeField] private Animator sidebarAnimator;
        public List<Button> OpenSidebarButtons { get => openSidebarButtons; set => openSidebarButtons = value; }
        [SerializeField] private List<Button> openSidebarButtons;
        public List<Button> CloseSidebarButtons { get => closeSidebarButtons; set => closeSidebarButtons = value; }
        [SerializeField] private List<Button> closeSidebarButtons;

        protected void Awake()
        {
            foreach (var openButton in OpenSidebarButtons)
                openButton.onClick.AddListener(OpenSidebar);
            foreach (var closeButton in CloseSidebarButtons)
                closeButton.onClick.AddListener(CloseSidebar);
        }

        protected virtual void OpenSidebar() => SetSidebarShowing(true);
        protected virtual void CloseSidebar() => SetSidebarShowing(false);

        protected virtual void SetSidebarShowing(bool showing) 
            => SidebarAnimator.SetBool("Showing", showing);
    }
}