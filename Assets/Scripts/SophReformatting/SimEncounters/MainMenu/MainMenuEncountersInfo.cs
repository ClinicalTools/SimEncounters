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
            casesDownloader.Completed += EncountersRetrieved;
            casesDownloader.GetEncounterInfos(mainMenu.User);
        }

        protected virtual void EncountersRetrieved(List<EncounterInfo> encounters)
        {
            var info = new InfoNeededForMainMenuToHappen(MainMenu.User, LoadingScreen.Instance, null);
            foreach (var encounter in encounters)
            {
                if (!encounter.MetaGroup.GetLatestInfo().IsTemplate)
                    info.AddEncounterDetail(encounter);
            }
            EncountersUI.Display(info);
        }

        private void EncountersView_Selected(EncounterMetaGroup encounterInfo)
        {
            //EncountersUI.Overview.GameObject.SetActive(true);
            //new OverviewDisplay(MainMenu, EncountersUI.Overview, encounterInfo);
        }
    }

    public class MainMenuEncountersViewDisplay
    {
        protected virtual MainMenuEncountersViewUI EncountersViewUI { get; }
        protected virtual MainMenuScene MainMenu { get; }
        public event Action<EncounterMetaGroup> Selected;

        public MainMenuEncountersViewDisplay(MainMenuScene mainMenu, MainMenuEncountersViewUI encountersViewUI, List<EncounterInfo> encounters)
        {
            MainMenu = mainMenu;
            EncountersViewUI = encountersViewUI;

            SetCases(encounters);
        }

        List<MainMenuEncounterDisplay> EncounterDisplays = new List<MainMenuEncounterDisplay>();
        public virtual void SetCases(List<EncounterInfo> encounters)
        {
            foreach (var encounter in encounters)
            {
                if (encounter.MetaGroup.GetLatestInfo().IsTemplate)
                    continue;

                var encounterUI = UnityEngine.Object.Instantiate(EncountersViewUI.OptionPrefab, EncountersViewUI.OptionsParent);
                var encounterDisplay = new MainMenuEncounterDisplay(MainMenu, encounterUI, encounter.MetaGroup);
                encounterDisplay.Selected += (selectedEncounter) => Selected?.Invoke(selectedEncounter);
                EncounterDisplays.Add(encounterDisplay);
            }
        }

        protected virtual void EncountersRetrieved(List<EncounterMetaGroup> encounters)
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
        public event Action<EncounterMetaGroup> Selected;
        protected MainMenuScene MainMenu { get; }

        public MainMenuEncounterDisplay(MainMenuScene mainMenu, MainMenuEncounterUI encounterUI, EncounterMetaGroup encounterInfo)
        {
            MainMenu = mainMenu;

            if (encounterUI.InfoViewer != null)
            {
                if (encounterInfo.GetLatestInfo() == null)
                    UnityEngine.Debug.Log("what");
                new EncounterInfoDisplay(encounterUI.InfoViewer, encounterInfo.GetLatestInfo());
            }
            encounterUI.SelectButton.onClick.AddListener(() => Selected?.Invoke(encounterInfo));
        }
    }

    public class EncounterInfoDisplay
    {
        public EncounterInfoDisplay(EncounterInfoUI encounterInfoUI, EncounterMetadata encounterMetadata)
        {
            if (encounterMetadata == null)
                UnityEngine.Debug.Log($"A{encounterInfoUI == null} B {encounterMetadata == null}");
            SetLabelText(encounterInfoUI.AudienceLabel, encounterMetadata.Audience);
            SetLabelText(encounterInfoUI.DescriptionLabel, encounterMetadata.Description);
            SetLabelText(encounterInfoUI.SubtitleLabel, encounterMetadata.Subtitle);

            // TODO: change to use encounter metagroup and metadata
            SetAuthor(encounterInfoUI.AuthorLabel, "CTI Staff");
            SetTitle(encounterInfoUI.TitleLabel, encounterMetadata.Title);
            SetDifficulty(encounterInfoUI.Difficulty, encounterMetadata.Difficulty);
            SetDateModified(encounterInfoUI.DateModifiedLabel, encounterMetadata.DateModified);
            SetCategories(encounterInfoUI.CategoriesLabel, encounterMetadata.Categories);
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
        protected virtual void SetAuthor(TextMeshProUGUI label, string author)
        {
            if (label != null)
                label.text = $"by {author.Replace('_', ' ').Trim()}";
        }
        protected virtual void SetDateModified(TextMeshProUGUI label, long dateModified)
        {
            if (label == null)
                return;
            var time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            time = time.AddSeconds(dateModified);
            if (time > DateTime.UtcNow || time.Year < 2015)
            {
                UnityEngine.Debug.LogError("Invalid time");
                label.text = "";
                return;
            }

            var timeString = time.ToLocalTime().ToString("MMMM d, yyyy");
            label.text = $"Last updated: {timeString}";
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