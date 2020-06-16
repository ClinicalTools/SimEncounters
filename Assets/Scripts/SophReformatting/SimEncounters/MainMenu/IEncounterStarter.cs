using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Writer;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public interface IEncounterStarter
    {
        void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter);
    }
    public class EncounterReadStarter : IEncounterStarter
    {
        protected IReaderSceneStarter SceneStarter { get; set; }
        protected IUserEncounterReader EncounterReader { get; set; }
        public EncounterReadStarter(IReaderSceneStarter sceneStarter, IUserEncounterReader encounterReader)
        {
            SceneStarter = sceneStarter;
            EncounterReader = encounterReader;
        }

        public virtual void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (menuEncounter.Status == null)
                menuEncounter.Status = new EncounterBasicStatus();

            var metadata = menuEncounter.GetLatestTypedMetada();
            var encounter = EncounterReader.GetUserEncounter(sceneInfo.User, metadata.Value, menuEncounter.Status, metadata.Key);
            var encounterSceneInfo = new LoadingReaderSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(encounterSceneInfo);
        }
    }
    public class EncounterEditStarter : IEncounterStarter
    {
        protected IWriterSceneStarter SceneStarter { get; set; }
        protected IEncounterReader EncounterReader { get; set; }
        public EncounterEditStarter(IWriterSceneStarter sceneStarter, IEncounterReader encounterReader)
        {
            SceneStarter = sceneStarter;
            EncounterReader = encounterReader;
        }

        public virtual void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (menuEncounter.Status == null)
                menuEncounter.Status = new EncounterBasicStatus();

            var metadata = menuEncounter.GetLatestTypedMetada();
            var encounter = EncounterReader.GetEncounter(sceneInfo.User, metadata.Value, metadata.Key);
            var encounterSceneInfo = new LoadingWriterSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(encounterSceneInfo);
        }
    }

    public interface IMetadataSelector
    {
        WaitableResult<IEncounterMetadata> GetMetadata(MenuEncounter menuEncounter);
    }

    public class MetadataSelector : MonoBehaviour, IMetadataSelector
    {
        // pick case
        // newer server case than local
        // newer autosave than local
        public WaitableResult<IEncounterMetadata> GetMetadata(MenuEncounter menuEncounter)
        {
            if (!menuEncounter.Metadata.ContainsKey(SaveType.Local))
                return new WaitableResult<IEncounterMetadata>(menuEncounter.GetLatestMetadata());

            var localMetadata = menuEncounter.Metadata[SaveType.Local];
            //if (!)

            // finish this. show popup

                return new WaitableResult<IEncounterMetadata>(menuEncounter.GetLatestMetadata());
        }
    }
}