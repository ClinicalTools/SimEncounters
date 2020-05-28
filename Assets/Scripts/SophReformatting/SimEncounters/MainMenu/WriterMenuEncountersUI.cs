using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class WriterMenuEncountersUI : BaseMenuSceneDrawer
    {
        public BaseEncounterSelector EncounterSelector { get => encounterSelector; set => encounterSelector = value; }
        [SerializeField] private BaseEncounterSelector encounterSelector;
        public GameObject DownloadingCases { get => downloadingCases; set => downloadingCases = value; }
        [SerializeField] private GameObject downloadingCases;
        public ChangeSidePanelScript EncountersToggle { get => encountersToggle; set => encountersToggle = value; }
        [SerializeField] private ChangeSidePanelScript encountersToggle;
        public ChangeSidePanelScript TemplatesToggle { get => templateToggle; set => templateToggle = value; }
        [SerializeField] private ChangeSidePanelScript templateToggle;
        public OverviewUI Overview { get => overview; set => overview = value; }
        [SerializeField] private OverviewUI overview;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;

        public MenuSceneInfo SceneInfo { get; set; }

        protected virtual void Awake()
        {
            EncounterSelector.EncounterSelected += EncounterSelected;
            EncountersToggle.Selected += DisplayEncounters;
            EncountersToggle.Selected += DisplayTemplates;
        }

        private void EncounterSelected(Data.MenuEncounter encounter) => Overview.DisplayForEdit(SceneInfo, encounter);

        public void Initialize()
        {
            EncountersToggle.Select();
            DownloadingCases.SetActive(true);
            EncounterSelector.Initialize();
        }

        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
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

            if (EncountersToggle.IsOn())
                DisplayEncounters();
            else
                DisplayTemplates();

        }

        protected virtual void DisplayEncounters() => DisplayEncounters(SceneInfo.MenuEncountersInfo.GetEncounters());
        protected virtual void DisplayTemplates() => DisplayEncounters(SceneInfo.MenuEncountersInfo.GetTemplates());
        protected virtual void DisplayEncounters(IEnumerable<Data.MenuEncounter> encounters) 
            => EncounterSelector.Display(SceneInfo, encounters);

    }
}