using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class LoadingReaderSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public WaitableResult<UserEncounter> Encounter { get; }
        public List<MenuEncounter> SuggestedEncounters { get; } = new List<MenuEncounter>();

        public WaitableResult<ReaderSceneInfo> Result { get; } = new WaitableResult<ReaderSceneInfo>();

        public LoadingReaderSceneInfo(User user, ILoadingScreen loadingScreen, WaitableResult<UserEncounter> encounter)
        {
            User = user;
            LoadingScreen = loadingScreen;
            Encounter = encounter;
            Encounter.AddOnCompletedListener(EncounterRetrieved);
        }

        private void EncounterRetrieved(UserEncounter encounter)
        {
            var loadedInfo = new ReaderSceneInfo(this);
            Result.SetResult(loadedInfo);
        }
    }
}