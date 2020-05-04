using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
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