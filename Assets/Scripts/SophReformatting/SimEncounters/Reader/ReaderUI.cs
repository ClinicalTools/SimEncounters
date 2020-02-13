using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderUI : SceneUI
    {
        [SerializeField] private Button infoButton;
        public virtual Button InfoButton { get => infoButton; set => infoButton = value; }

        [SerializeField] private Button settingsButton;
        public virtual Button SettingsButton { get => settingsButton; set => settingsButton = value; }

        [SerializeField] private Button helpButton;
        public virtual Button HelpButton { get => helpButton; set => helpButton = value; }

        [SerializeField] private Button mainMenuButton;
        public virtual Button MainMenuButton { get => mainMenuButton; set => mainMenuButton = value; }

        [SerializeField] private ReaderSectionsUI sections;
        public virtual ReaderSectionsUI Sections { get => sections; set => sections = value; }

        [SerializeField] private ReaderPinsUI pins;
        public virtual ReaderPinsUI Pins { get => pins; set => pins = value; }

        [SerializeField] private ReaderPopupsUI popups;
        public virtual ReaderPopupsUI Popups { get => popups; set => popups = value; }

        [SerializeField] private ReaderFooterUI footer;
        public virtual ReaderFooterUI Footer { get => footer; set => footer = value; }

        [SerializeField] private MouseInput mouse;
        public virtual MouseInput Mouse { get => mouse; set => mouse = value; }

        [SerializeField] private ReaderEncounterInfoUI encounterInfo;
        public virtual ReaderEncounterInfoUI EncounterInfo { get => encounterInfo; set => encounterInfo = value; }
    }
}