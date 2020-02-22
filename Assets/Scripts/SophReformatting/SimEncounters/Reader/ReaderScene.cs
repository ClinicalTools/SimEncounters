﻿using ClinicalTools.SimEncounters.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderScene : EncounterScene
    {
        protected virtual object User { get; }
        public virtual Encounter Encounter { get; }
        public virtual ReaderSectionsGroup SectionsGroup { get; }
        protected ReaderUI ReaderUI { get; }
        public virtual ReaderPinManager Pins { get; }
        public virtual ReaderPopupManager Popups { get; }
        public virtual ReaderFooter Footer { get; }
        public MouseInput Mouse { get; internal set; }
        public ReaderTabDisplayFactory TabDisplayFactory { get; }

        public HashSet<string> ReadTabs { get; } = new HashSet<string>();


        // combine user/loading screen and maybe encounter?
        public ReaderScene(User user, LoadingScreen loadingScreen, Encounter encounter, ReaderUI readerUI)
            : base(readerUI)
        {
            User = user;
            ReaderUI = readerUI;
            Encounter = encounter; 
            TabDisplayFactory = new ReaderTabDisplayFactory(this);

            Pins = CreatePinManager();
            Popups = CreatePopupManager();
            Footer = CreateFooter();
            SectionsGroup = CreateSectionsGroup();
            Mouse = readerUI.Mouse;
            var encounterInfo = new ReaderEncounterInfo(this, ReaderUI.EncounterInfo, encounter.Info);

            AddListeners(ReaderUI);
            loadingScreen.Stop();
        }

        protected virtual ReaderSectionsGroup CreateSectionsGroup() => new ReaderSectionsGroup(this, ReaderUI.Sections, Encounter.Content);
        protected virtual ReaderPinManager CreatePinManager() => new ReaderPinManager(this, ReaderUI.Pins);
        protected virtual ReaderPopupManager CreatePopupManager() => new ReaderPopupManager(this, ReaderUI.Popups);
        protected virtual ReaderFooter CreateFooter() => new ReaderFooter(this, ReaderUI.Footer, Encounter.Content.Sections);


        protected virtual void AddListeners(ReaderUI readerUI)
        {
            readerUI.MainMenuButton.onClick.AddListener(ShowMainMenu);
        }

        public void Help()
        {

        }

        public void ShowMainMenu()
        {

        }
    }
}