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
        public ChangeSidePanelScript ShowEncountersToggle { get => encountersToggle; set => encountersToggle = value; }
        [SerializeField] private ChangeSidePanelScript encountersToggle;
        public ChangeSidePanelScript ShowTemplatesToggle { get => templateToggle; set => templateToggle = value; }
        [SerializeField] private ChangeSidePanelScript templateToggle;
        public OverviewUI Overview { get => overview; set => overview = value; }
        [SerializeField] private OverviewUI overview;

        public MenuSceneInfo SceneInfo { get; set; }

        protected virtual void Awake()
        {
            EncounterSelector.EncounterSelected += EncounterSelected;
            ShowEncountersToggle.Selected += DisplayEncounters;
            ShowTemplatesToggle.Selected += DisplayTemplates;
        }

        private void EncounterSelected(Data.MenuEncounter encounter) => Overview.DisplayForEdit(SceneInfo, encounter);

        public void Initialize()
        {
            ShowEncountersToggle.Select();
            DownloadingCases.SetActive(true);
            EncounterSelector.Initialize();
        }

        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            SceneInfo = null;
            Initialize();
            ShowCasesLoading();
            loadingSceneInfo.Result.AddOnCompletedListener(ShowEncounters);
        }

        protected virtual void ShowCasesLoading() => DownloadingCases.SetActive(true);

        protected virtual void ShowEncounters(MenuSceneInfo sceneInfo)
        {
            sceneInfo.LoadingScreen.Stop();
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
                => EncounterSelector.Display(SceneInfo, encounters);

        public override void Hide()
        {
            EncounterSelector.Hide();
            ShowTemplatesToggle.Hide();
            ShowEncountersToggle.Hide();
            Overview.Hide();
        }
    }
}