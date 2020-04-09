using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using TMPro;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncountersInfo
    {
        protected virtual MainMenuEncountersUI EncountersUI { get; }
        protected virtual MainMenuScene MainMenu { get; }

        public MainMenuEncountersInfo(MainMenuScene mainMenu, MainMenuEncountersUI encountersUI)
        {
            MainMenu = mainMenu;
            EncountersUI = encountersUI;


            IMenuEncountersReader casesDownloader = null; // implement
            
            //casesDownloader.Completed += EncountersRetrieved;
            //casesDownloader.GetEncounterInfos(mainMenu.User);
        }

        protected virtual void EncountersRetrieved(List<MenuEncounter> encounters)
        {
            //var info = new InfoNeededForMainMenuToHappen(MainMenu.User, LoadingScreen.Instance, null);
            foreach (var encounter in encounters)
            {
                //if (!encounter.GetLatestMetadata().IsTemplate)
                  //  info.AddEncounterDetail(encounter);
            }
//            EncountersUI.Display(info);
        }
    }

}