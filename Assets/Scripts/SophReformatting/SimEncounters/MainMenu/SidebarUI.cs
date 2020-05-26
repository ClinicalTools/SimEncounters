using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class SidebarUI : MonoBehaviour
    {
        public virtual Button ToggleOpenButton { get => toggleOpenButton; set => toggleOpenButton = value; }
        [SerializeField] private Button toggleOpenButton;
        public virtual Button HideButton { get => hideButton; set => hideButton = value; }
        [SerializeField] private Button hideButton;
        public virtual SearchStuffUI SearchStuff { get => searchStuff; set => searchStuff = value; }
        [SerializeField] private SearchStuffUI searchStuff;
        public Button ShowOptionsButton { get => showOptionsButton; set => showOptionsButton = value; }
        [SerializeField] private Button showOptionsButton;
        public Button QuitButton { get => quitButton; set => quitButton = value; }
        [SerializeField] private Button quitButton;

        public void Show()
        {
            ToggleOpenButton.gameObject.SetActive(true);
            ToggleOpenButton.interactable = true;
        }

        public void Hide()
        {
            ToggleOpenButton.gameObject.SetActive(false);
            ToggleOpenButton.interactable = false;
        }
    }
}