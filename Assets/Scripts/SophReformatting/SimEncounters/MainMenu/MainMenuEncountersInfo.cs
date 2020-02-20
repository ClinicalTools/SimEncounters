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

            var casesDownloader = new EncountersInfoReader();
            EncountersUI = encountersUI;
            casesDownloader.Completed += EncountersRetrieved;
            casesDownloader.GetEncounterInfos(mainMenu.User);
        }

        protected virtual void EncountersRetrieved(List<EncounterInfoGroup> encounters)
        {
            new MainMenuEncountersViewDisplay(MainMenu, EncountersUI.GridView, encounters);
            EncountersUI.DownloadingCasesObject.SetActive(false);
            EncountersUI.GridView.GameObject.SetActive(true);

        }
    }

    public class MainMenuEncountersViewDisplay
    {
        protected virtual MainMenuEncountersViewUI EncountersViewUI { get; }
        protected virtual MainMenuScene MainMenu { get; }

        public MainMenuEncountersViewDisplay(MainMenuScene mainMenu, MainMenuEncountersViewUI encountersViewUI, List<EncounterInfoGroup> encounters)
        {
            MainMenu = mainMenu;
            EncountersViewUI = encountersViewUI;

            SetCases(encounters);
        }

        List<MainMenuEncounterDisplay> EncounterDisplays = new List<MainMenuEncounterDisplay>();
        public virtual void SetCases(List<EncounterInfoGroup> encounters)
        {
            foreach (var encounter in encounters) {
                var encounterUI = UnityEngine.Object.Instantiate(EncountersViewUI.OptionPrefab, EncountersViewUI.OptionsParent);
                var encounterDisplay = new MainMenuEncounterDisplay(MainMenu, encounterUI, encounter);
                EncounterDisplays.Add(encounterDisplay);

            }
        }

        protected virtual void EncountersRetrieved(List<EncounterInfoGroup> encounters)
        {
            EncountersViewUI.GameObject.SetActive(true);

        }

        public void Show()
        {

        }
        public void Hide()
        {

        }
    }

    public class MainMenuEncounterDisplay
    {
        public MainMenuEncounterDisplay(MainMenuScene mainMenu, MainMenuEncounterUI encounterUI, EncounterInfoGroup encounter)
        {
            if (encounterUI.InfoViewer != null) {
                if (encounter.GetLatestInfo() == null)
                    UnityEngine.Debug.Log("what");
                new EncounterInfoDisplay(encounterUI.InfoViewer, encounter.GetLatestInfo());
            }
        }


    }
    public class EncounterInfoDisplay
    {
        public EncounterInfoDisplay(EncounterInfoUI encounterInfoUI, EncounterInfo encounterInfo)
        {
            if (encounterInfo == null)
                UnityEngine.Debug.Log($"A{encounterInfoUI == null} B {encounterInfo == null}");
            SetLabelText(encounterInfoUI.AudienceLabel, encounterInfo.Audience);
            SetLabelText(encounterInfoUI.DescriptionLabel, encounterInfo.Description);
            SetLabelText(encounterInfoUI.SubtitleLabel, encounterInfo.Subtitle);

            SetAutor(encounterInfoUI.AuthorLabel, encounterInfo.AuthorName);
            SetTitle(encounterInfoUI.TitleLabel, encounterInfo.Title);
            SetDifficulty(encounterInfoUI.Difficulty, encounterInfo.Difficulty);
            SetDateModified(encounterInfoUI.DateModifiedLabel, encounterInfo.DateModified);
            SetCategories(encounterInfoUI.CategoriesLabel, encounterInfo.Categories);
        }

        protected virtual void SetDifficulty(DifficultyUI difficultyUI, Difficulty difficulty)
        {
            if (difficultyUI != null)
                new DifficultyDisplay(difficultyUI, difficulty);
        }
        protected virtual void SetTitle(TextMeshProUGUI label, string title)
        {
            if (label != null)
                label.text = title.Replace('_', ' ').Trim();
        }
        protected virtual void SetAutor(TextMeshProUGUI label, string author)
        {
            if (label != null)
                label.text = $"by {author.Replace('_', ' ').Trim()}";
        }
        protected virtual void SetDateModified(TextMeshProUGUI label, long dateModified)
        {
            if (label == null)
                return;
            var time = new DateTime(dateModified);
            label.text = time.ToString();
        }
        protected virtual string CategoryConcatenator => ", ";
        protected virtual void SetCategories(TextMeshProUGUI label, IEnumerable<string> categories)
        {
            if (label != null)
                label.text = string.Join(CategoryConcatenator, categories);
        }

        protected virtual void SetLabelText(TextMeshProUGUI label, string text)
        {
            if (label != null)
                label.text = text;
        }
    }
}