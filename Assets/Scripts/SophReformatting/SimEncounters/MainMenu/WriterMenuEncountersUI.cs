using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class WriterMenuEncountersUI : BaseMenuSceneDrawer
    {
        public BaseEncounterSelector EncounterSelector { get => encounterSelector; set => encounterSelector = value; }
        [SerializeField] private BaseEncounterSelector encounterSelector;
        public GameObject DownloadingCases { get => downloadingCases; set => downloadingCases = value; }
        [SerializeField] private GameObject downloadingCases;
        public ChangeSidePanelScript ShowEncountersToggle { get => showEncountersToggle; set => showEncountersToggle = value; }
        [SerializeField] private ChangeSidePanelScript showEncountersToggle;
        public ChangeSidePanelScript ShowTemplatesToggle { get => showTemplatesToggle; set => showTemplatesToggle = value; }
        [SerializeField] private ChangeSidePanelScript showTemplatesToggle;
        public OverviewUI Overview { get => overview; set => overview = value; }
        [SerializeField] private OverviewUI overview;

        public MenuSceneInfo SceneInfo { get; set; }

        protected virtual bool IsOn { get; set; }
        protected virtual void AddListeners()
        {
            if (IsOn)
                return;
            IsOn = true;

            EncounterSelector.EncounterSelected += EncounterSelected;
            ShowEncountersToggle.Selected += DisplayEncounters;
            ShowTemplatesToggle.Selected += DisplayTemplates;
        }
        protected virtual void RemoveListeners()
        {
            if (!IsOn)
                return;
            IsOn = false;

            EncounterSelector.EncounterSelected -= EncounterSelected;
            ShowEncountersToggle.Selected -= DisplayEncounters;
            ShowTemplatesToggle.Selected -= DisplayTemplates;
        }

        private void EncounterSelected(Data.MenuEncounter encounter) => Overview.DisplayForEdit(SceneInfo, encounter);

        public void Initialize()
        {
            AddListeners();
            ShowEncountersToggle.Select();
            DownloadingCases.SetActive(true);
            EncounterSelector.Initialize();
        }

        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            ShowTemplatesToggle.Display();
            ShowEncountersToggle.Display();
            SceneInfo = null;
            Initialize();
            ShowCasesLoading();
            loadingSceneInfo.Result.AddOnCompletedListener(ShowEncounters);
        }

        protected virtual void ShowCasesLoading() => DownloadingCases.SetActive(true);

        protected virtual void ShowEncounters(MenuSceneInfo sceneInfo)
        {
            sceneInfo.LoadingScreen?.Stop();
            DownloadingCases.SetActive(false);

            SceneInfo = sceneInfo;

            if (ShowEncountersToggle.IsOn())
                DisplayEncounters();
            else
                DisplayTemplates();
        }

        protected virtual void DisplayEncounters()
        {
            Overview.Hide();
            if (SceneInfo != null)
                DisplayEncounters(SceneInfo.MenuEncountersInfo.GetEncounters());
        }
        protected virtual void DisplayTemplates()
        {
            Overview.Hide();
            if (SceneInfo != null) 
                DisplayEncounters(SceneInfo.MenuEncountersInfo.GetTemplates());
        }
        protected virtual void DisplayEncounters(IEnumerable<Data.MenuEncounter> encounters)
                => EncounterSelector.DisplayForEdit(SceneInfo, encounters);

        public override void Hide()
        {
            RemoveListeners();
            EncounterSelector.Hide();
            ShowTemplatesToggle.Hide();
            ShowEncountersToggle.Hide();
            Overview.Hide();
        }
    }
}