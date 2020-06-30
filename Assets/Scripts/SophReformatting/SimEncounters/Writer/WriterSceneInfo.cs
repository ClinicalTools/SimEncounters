using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public Encounter Encounter { get; }

        public WriterSceneInfo(LoadingWriterSceneInfo loadingEncounterSceneInfo)
        {
            User = loadingEncounterSceneInfo.User;
            LoadingScreen = loadingEncounterSceneInfo.LoadingScreen;
            Encounter = loadingEncounterSceneInfo.Encounter.Result.Value;
        }
    }
}