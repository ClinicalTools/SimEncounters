using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Writer;
using System.Collections.Generic;
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
        protected BaseMetadataSelector MetadataSelector { get; set; }
        public EncounterReadStarter(IReaderSceneStarter sceneStarter, IUserEncounterReader encounterReader, BaseMetadataSelector metadataSelector)
        {
            SceneStarter = sceneStarter;
            EncounterReader = encounterReader;
            MetadataSelector = metadataSelector;
        }

        public virtual void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (MetadataSelector == null) {
                MetadataSelected(sceneInfo, menuEncounter.Status, menuEncounter.GetLatestTypedMetada());
                return;
            }

            var result = MetadataSelector.GetMetadata(menuEncounter);
            result.AddOnCompletedListener((value) => MetadataSelected(sceneInfo, menuEncounter.Status, value));
        }

        protected virtual void MetadataSelected(MenuSceneInfo sceneInfo, EncounterBasicStatus status, WaitedResult<KeyValuePair<SaveType, EncounterMetadata>> metadata)
            => MetadataSelected(sceneInfo, status, metadata.Value);

        protected virtual void MetadataSelected(MenuSceneInfo sceneInfo, EncounterBasicStatus status, KeyValuePair<SaveType, EncounterMetadata> metadata)
        {
            if (status == null)
                status = new EncounterBasicStatus();

            var encounter = EncounterReader.GetUserEncounter(sceneInfo.User, metadata.Value, status, metadata.Key);
            var encounterSceneInfo = new LoadingReaderSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(encounterSceneInfo);
        }
    }
    public class EncounterEditStarter : IEncounterStarter
    {
        protected IWriterSceneStarter SceneStarter { get; set; }
        protected IEncounterReader EncounterReader { get; set; }
        protected BaseMetadataSelector MetadataSelector { get; set; }
        public EncounterEditStarter(IWriterSceneStarter sceneStarter, IEncounterReader encounterReader, BaseMetadataSelector metadataSelector)
        {
            SceneStarter = sceneStarter;
            EncounterReader = encounterReader;
            MetadataSelector = metadataSelector;
        }

        public virtual void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (MetadataSelector == null) {
                MetadataSelected(sceneInfo, menuEncounter.GetLatestTypedMetada());
                return;
            }

            var result = MetadataSelector.GetMetadata(menuEncounter);
            result.AddOnCompletedListener((value) => MetadataSelected(sceneInfo, value));
        }

        protected virtual void MetadataSelected(MenuSceneInfo sceneInfo, WaitedResult<KeyValuePair<SaveType, EncounterMetadata>> metadata)
            => MetadataSelected(sceneInfo, metadata.Value);
        protected virtual void MetadataSelected(MenuSceneInfo sceneInfo,  KeyValuePair<SaveType, EncounterMetadata> metadata)
        {
            if (metadata.Value == null)
                return;

            var encounter = EncounterReader.GetEncounter(sceneInfo.User, metadata.Value, metadata.Key);
            var encounterSceneInfo = new LoadingWriterSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(encounterSceneInfo);
        }
    }
}