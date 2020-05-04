using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Writer
{
    public class LoadingWriterSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public WaitableResult<Encounter> Encounter { get; }

        public WaitableResult<WriterSceneInfo> Result { get; } = new WaitableResult<WriterSceneInfo>();

        public LoadingWriterSceneInfo(User user, ILoadingScreen loadingScreen, WaitableResult<Encounter> encounter)
        {
            User = user;
            LoadingScreen = loadingScreen;
            Encounter = encounter;
            Encounter.AddOnCompletedListener(EncounterRetrieved);
        }

        private void EncounterRetrieved(Encounter encounter)
        {
            var loadedInfo = new WriterSceneInfo(this);
            Result.SetResult(loadedInfo);
        }
    }
}