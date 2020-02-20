using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncountersUI : MonoBehaviour
    {
        [SerializeField] private ToggleViewButtonUI toggleViewButton;
        public ToggleViewButtonUI ToggleViewButton { get => toggleViewButton; set => toggleViewButton = value; }

        [SerializeField] private MainMenuEncountersViewUI listView;
        public MainMenuEncountersViewUI ListView { get => listView; set => listView = value; }

        [SerializeField] private MainMenuEncountersViewUI gridView;
        public MainMenuEncountersViewUI GridView { get => gridView; set => gridView = value; }

        [SerializeField] private SidebarUI sidebar;
        public SidebarUI Sidebar { get => sidebar; set => sidebar = value; }

        [SerializeField] private OverviewUI overview;
        public OverviewUI Overview { get => overview; set => overview = value; }

        [SerializeField] private PanelButtonsUI panelButtons;
        public PanelButtonsUI PanelButtons { get => panelButtons; set => panelButtons = value; }
        
        [SerializeField] private PageButtonsUI pageButtons;
        public PageButtonsUI PageButtons { get => pageButtons; set => pageButtons = value; }

        [SerializeField] private GameObject downloadingCases;
        public GameObject DownloadingCasesObject { get => downloadingCases; set => downloadingCases = value; }
    }
}