using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class LoadingEncounterSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public WaitableResult<FullEncounter> Encounter { get; }
        public List<MenuEncounter> SuggestedEncounters { get; } = new List<MenuEncounter>();

        public WaitableResult<EncounterSceneInfo> Result { get; } = new WaitableResult<EncounterSceneInfo>();

        public LoadingEncounterSceneInfo(User user, ILoadingScreen loadingScreen, WaitableResult<FullEncounter> encounter)
        {
            User = user;
            LoadingScreen = loadingScreen;
            Encounter = encounter;
            Encounter.AddOnCompletedListener(EncounterRetrieved);
        }

        private void EncounterRetrieved(FullEncounter encounter)
        {
            var loadedInfo = new EncounterSceneInfo(this);
            Result.SetResult(loadedInfo);
        }
    }

    public class EncounterSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public FullEncounter Encounter { get; }
        public List<MenuEncounter> SuggestedEncounters { get; } = new List<MenuEncounter>();

        public EncounterSceneInfo(LoadingEncounterSceneInfo loadingEncounterSceneInfo)
        {
            User = loadingEncounterSceneInfo.User;
            LoadingScreen = loadingEncounterSceneInfo.LoadingScreen;
            Encounter = loadingEncounterSceneInfo.Encounter.Result;
            SuggestedEncounters = loadingEncounterSceneInfo.SuggestedEncounters;
        }
    }

    public class LoadingMenuSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public WaitableResult<List<Category>> Categories { get; }
        public WaitableResult<MenuSceneInfo> Result = new WaitableResult<MenuSceneInfo>();

        public LoadingMenuSceneInfo(User user, ILoadingScreen loadingScreen, WaitableResult<List<Category>> categories)
        {
            User = user;
            LoadingScreen = loadingScreen;
            Categories = categories;
            Categories.AddOnCompletedListener(CategoriesRetrieved);
        }

        private void CategoriesRetrieved(List<Category> categories)
        {
            var loadedInfo = new MenuSceneInfo(this);
            Result.SetResult(loadedInfo);
        }
    }

    public class MenuSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public List<Category> Categories { get; }

        public MenuSceneInfo(LoadingMenuSceneInfo loadingMenuSceneInfo)
        {
            User = loadingMenuSceneInfo.User;
            LoadingScreen = loadingMenuSceneInfo.LoadingScreen;
            Categories = loadingMenuSceneInfo.Categories.Result;
        }
    }
}