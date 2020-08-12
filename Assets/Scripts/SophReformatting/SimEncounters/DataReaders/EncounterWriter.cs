using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.SimEncounters.Writer
{
    public class EncounterWriter : IEncounterWriter
    {
        protected IEncounterWriter MainDataWriter { get; }
        public EncounterWriter(IEncounterWriter mainDataWriter)
        {
            MainDataWriter = mainDataWriter;
        }

        private readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public void Save(User user, Encounter encounter)
        {
            encounter.Metadata.DateModified = (long)(DateTime.UtcNow - unixEpoch).TotalSeconds;
            encounter.Metadata.AuthorName = user.Name;
            encounter.Metadata.AuthorAccountId = user.AccountId;
            MainDataWriter.Save(user, encounter);
        }
    }
}