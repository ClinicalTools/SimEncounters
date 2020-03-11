using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Reader;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneLoader
    {
        protected IScenePathData ScenePathData { get; }

        public ReaderSceneLoader(IScenePathData scenePathData)
        {
            ScenePathData = scenePathData;
        }

        public virtual void StartScene(EncounterSceneManager sceneManager, User user, IEncounterGetter encounterGetter)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePathData.ReaderPath);
            loading.completed += (asyncOperation) => InitializeScene(sceneManager, user, encounterGetter);
        }

        protected virtual void InitializeScene(EncounterSceneManager sceneManager, User user, IEncounterGetter encounterGetter)
        {
            if (encounterGetter.IsDone)
                StartReader(sceneManager, user, encounterGetter.Encounter);
            else
                encounterGetter.Completed += (encounter) => StartReader(sceneManager, user, encounter);
        }

        public virtual void StartReader(EncounterSceneManager sceneManager, User user, Encounter encounter)
        {
            new ReaderScene(user, null, encounter, (ReaderUI)sceneManager.SceneUI);
        }
    }
}