using ClinicalTools.ClinicalEncounters.Loader;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.Loading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class OverviewUI : MonoBehaviour
    {
        public GameObject GameObject => gameObject;

        [SerializeField] private EncounterInfoUI infoViewer;
        public virtual EncounterInfoUI InfoViewer { get => infoViewer; set => infoViewer = value; }

        [SerializeField] private EncounterButtonsUI encounterButtons;
        public virtual EncounterButtonsUI EncounterButtons { get => encounterButtons; set => encounterButtons = value; }

        [SerializeField] private EncounterButtonsUI templateButtons;
        public virtual EncounterButtonsUI TemplateButtons { get => templateButtons; set => templateButtons = value; }

        [SerializeField] private Button downloadButton;
        public virtual Button DownloadButton { get => downloadButton; set => downloadButton = value; }

        [SerializeField] private Button deleteButton;
        public virtual Button DeleteButton { get => deleteButton; set => deleteButton = value; }

        [SerializeField] private List<Button> hideOverviewButtons;
        public virtual List<Button> HideOverviewButtons { get => hideOverviewButtons; set => hideOverviewButtons = value; }

        protected EncounterInfo CurrentEncounterDetails { get; set; }
        public virtual void Display(InfoNeededForMainMenuToHappen data, EncounterInfo encounterInfo)
        {
            CurrentEncounterDetails = encounterInfo;

            if (InfoViewer != null) {
                new EncounterInfoDisplay(InfoViewer, encounterInfo.MetaGroup.GetLatestInfo());
            }

            EncounterButtons.ReadButton.onClick.RemoveAllListeners();
            EncounterButtons.ReadButton.onClick.AddListener(() => ReadCase(data.User, encounterInfo));
        }

        public virtual void ReadCase(User user, EncounterInfo encounterInfo)
        {
            EncounterGetter encounterGetter =
                new EncounterGetter(
                    new ClinicalEncounterLoader(),
                    new ServerXml(new DownloadEncounter(new WebAddress()), new DownloadEncounter(new WebAddress())),
                    new AutoSaveXml(new FilePathManager(), new FileXmlReader()),
                    new FileXml(new FilePathManager(), new FileXmlReader()));

            if (encounterInfo.MetaGroup.LocalInfo != null)
                encounterGetter.GetLocalEncounter(user, encounterInfo.MetaGroup);
            else if (encounterInfo.MetaGroup.ServerInfo != null)
                encounterGetter.GetServerEncounter(user, encounterInfo.MetaGroup);

            EncounterSceneManager.EncounterInstance.StartReaderScene(user, encounterInfo, encounterGetter);
        }
    }

    public class OverviewDisplay
    {
        public MainMenuScene MainMenu { get; }
        public EncounterMetaGroup EncounterInfo { get; }

        public OverviewDisplay(MainMenuScene mainMenu, OverviewUI overviewUI, EncounterMetaGroup encounterInfo)
        {
            MainMenu = mainMenu;
            EncounterInfo = encounterInfo;

            if (overviewUI.InfoViewer != null) {
                new EncounterInfoDisplay(overviewUI.InfoViewer, encounterInfo.GetLatestInfo());
            }

            overviewUI.EncounterButtons.ReadButton.onClick.RemoveAllListeners();
            overviewUI.EncounterButtons.ReadButton.onClick.AddListener(ReadCase);
        }

        protected void ReadCase()
        {
            EncounterGetter encounterGetter =
                new EncounterGetter(
                    new ClinicalEncounterLoader(),
                    new ServerXml(new DownloadEncounter(new WebAddress()), new DownloadEncounter(new WebAddress())),
                    new AutoSaveXml(new FilePathManager(), new FileXmlReader()),
                    new FileXml(new FilePathManager(), new FileXmlReader()));

            if (EncounterInfo.LocalInfo != null)
                encounterGetter.GetLocalEncounter(MainMenu.User, EncounterInfo);
            else if (EncounterInfo.ServerInfo != null)
                encounterGetter.GetServerEncounter(MainMenu.User, EncounterInfo);

            EncounterSceneManager.EncounterInstance.StartReaderScene(MainMenu.User, null, encounterGetter);
        }
    }
}