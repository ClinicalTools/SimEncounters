using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterReader : IUserEncounterReader
    {
        private readonly IEncounterReader dataReader;
        private readonly IDetailedStatusReader detailedStatusReader;
        public UserEncounterReader(IEncounterReader dataReader, IDetailedStatusReader detailedStatusReader)
        {
            this.dataReader = dataReader;
            this.detailedStatusReader = detailedStatusReader;
        }

        public WaitableResult<UserEncounter> GetUserEncounter(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus)
        {
            var encounterData = dataReader.GetEncounter(user, metadata);
            var detailedStatus = detailedStatusReader.GetDetailedStatus(user, metadata, basicStatus);

            var encounter = new WaitableResult<UserEncounter>();
            void processResults() => ProcessResults(user, encounter, metadata, encounterData, detailedStatus);
            encounterData.AddOnCompletedListener((result) => processResults());
            detailedStatus.AddOnCompletedListener((result) => processResults());

            return encounter;
        }

        protected void ProcessResults(User user,
            WaitableResult<UserEncounter> result,
            EncounterMetadata metadata,
            WaitableResult<Encounter> encounterData,
            WaitableResult<EncounterStatus> detailedStatus)
        {
            if (result.IsCompleted || !encounterData.IsCompleted || !detailedStatus.IsCompleted)
                return;

            //var encounter = new FullEncounter(metadata, encounterData.Result, detailedStatus.Result);
            var encounter = new UserEncounter(user, metadata, encounterData.Result, detailedStatus.Result);
            result.SetResult(encounter);
        }
    }
}