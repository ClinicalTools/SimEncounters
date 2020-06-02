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
        protected IUserEncounterReaderSelector EncounterReaderSelector { get; set; }
        public EncounterReadStarter(IReaderSceneStarter sceneStarter, IUserEncounterReaderSelector encounterReaderSelector)
        {
            SceneStarter = sceneStarter;
            EncounterReaderSelector = encounterReaderSelector;
        }

        public virtual void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (menuEncounter.Status == null)
                menuEncounter.Status = new EncounterBasicStatus();

            var metadata = menuEncounter.GetLatestTypedMetada();
            IUserEncounterReader encounterReader = EncounterReaderSelector.GetUserEncounterReader(metadata.Key);

            var encounter = encounterReader.GetUserEncounter(sceneInfo.User, metadata.Value, menuEncounter.Status);
            var encounterSceneInfo = new LoadingReaderSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(encounterSceneInfo);
        }
    }
    public class EncounterEditStarter : IEncounterStarter
    {
        protected IWriterSceneStarter SceneStarter { get; set; }
        protected IEncounterReaderSelector EncounterReaderSelector { get; set; }
        public EncounterEditStarter(IWriterSceneStarter sceneStarter, IEncounterReaderSelector encounterReaderSelector)
        {
            SceneStarter = sceneStarter;
            EncounterReaderSelector = encounterReaderSelector;
        }

        public virtual void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (menuEncounter.Status == null)
                menuEncounter.Status = new EncounterBasicStatus();

            var metadata = menuEncounter.GetLatestTypedMetada();
            var encounterReader = EncounterReaderSelector.GetEncounterReader(metadata.Key);
            var encounter = encounterReader.GetEncounter(sceneInfo.User, metadata.Value);
            var encounterSceneInfo = new LoadingWriterSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(encounterSceneInfo);
        }
    }

    public interface IMetadataSelector
    {
        WaitableResult<EncounterMetadata> GetMetadata(MenuEncounter menuEncounter);
    }

    public class MetadataSelector : MonoBehaviour, IMetadataSelector
    {
        // pick case
        // newer server case than local
        // newer autosave than local
        public WaitableResult<EncounterMetadata> GetMetadata(MenuEncounter menuEncounter)
        {
            if (!menuEncounter.Metadata.ContainsKey(SaveType.Local))
                return new WaitableResult<EncounterMetadata>(menuEncounter.GetLatestMetadata());

            var localMetadata = menuEncounter.Metadata[SaveType.Local];
            //if (!)

            // finish this. show popup

                return new WaitableResult<EncounterMetadata>(menuEncounter.GetLatestMetadata());
        }
    }
}