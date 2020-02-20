using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class SidebarUI : MonoBehaviour
    {
        [SerializeField] private Button toggleOpenButton;
        public virtual Button ToggleOpenButton { get => toggleOpenButton; set => toggleOpenButton = value; }

        [SerializeField] private Button hideButton;
        public virtual Button HideButton { get => hideButton; set => hideButton = value; }

        [SerializeField] private TMP_InputField searchField;
        public virtual TMP_InputField SearchField { get => searchField; set => searchField = value; }

        [SerializeField] private Button showOptionsButton;
        public Button ShowOptionsButton { get => showOptionsButton; set => showOptionsButton = value; }

        [SerializeField] private Button quitButton;
        public Button QuitButton { get => quitButton; set => quitButton = value; }
    }
}