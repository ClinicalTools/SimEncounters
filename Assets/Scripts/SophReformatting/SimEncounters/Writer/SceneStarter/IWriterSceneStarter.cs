using ClinicalTools.SimEncounters.Writer;

namespace ClinicalTools.SimEncounters
{
    public interface IWriterSceneStarter
    {
        void StartScene(LoadingWriterSceneInfo encounterSceneInfo);
    }
}