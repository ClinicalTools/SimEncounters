using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.Loading;
using System;

namespace ClinicalTools.SimEncounters
{
    public class EncounterDataReader : IEncounterDataReader
    {
        public event Action<EncounterData> Completed;
        public bool IsDone { get; protected set; }
        public EncounterData EncounterData { get; protected set; }

        protected EncounterLoader EncounterLoader { get; set; }
        protected IEncounterXmlReader EncounterXml { get; set; }

        public EncounterDataReader(EncounterLoader encounterLoader, IEncounterXmlReader encounterXml)
        {
            EncounterLoader = encounterLoader;
            EncounterXml = encounterXml;
        }

        public void DoStuff(User user, EncounterInfo info)
        {
            EncounterXml.Completed += EncounterXml_Completed;
            EncounterXml.GetEncounterXml(user, info);
        }

        private void EncounterXml_Completed(object sender, EncounterXmlRetrievedEventArgs e)
        {
            var encounter = EncounterLoader.ReadEncounter(e.DataXml, e.ImagesXml);
            EncounterData = encounter;
            IsDone = true;
            Completed?.Invoke(encounter);
        }
    }

    public class EncounterGetter : IEncounterDataReader
    {
        public event Action<EncounterData> Completed;
        public bool IsDone { get; protected set; }
        public EncounterData EncounterData { get; protected set; }

        protected EncounterLoader EncounterLoader { get; set; }
        protected ServerXml ServerXml { get; set; }
        protected AutoSaveXml AutoSaveXml { get; set; }
        protected FileXml FileXml { get; set; }

        public EncounterGetter(EncounterLoader encounterLoader, ServerXml serverXml, AutoSaveXml autoSaveXml, FileXml fileXml)
        {
            EncounterLoader = encounterLoader;
            ServerXml = serverXml;
            FileXml = fileXml;
            AutoSaveXml = autoSaveXml;
        }

        public void GetAutosaveEncounter(User user, EncounterMetaGroup encounterInfoGroup)
        {
            GetEncounter(user, encounterInfoGroup, AutoSaveXml);
        }

        public void GetLocalEncounter(User user, EncounterMetaGroup encounterInfoGroup)
        {
            GetEncounter(user, encounterInfoGroup, FileXml);
        }

        public void GetServerEncounter(User user, EncounterMetaGroup encounterInfoGroup)
        {
            GetEncounter(user, encounterInfoGroup, ServerXml);
        }

        protected void GetEncounter(User user, EncounterMetaGroup encounterInfoGroup, IEncounterXmlReader encounterXml)
        {
            encounterXml.Completed += (sender, e) => EncounterXml_Completed(encounterInfoGroup, e);
            encounterXml.GetEncounterXml(user, null);
        }

        private void EncounterXml_Completed(EncounterMetaGroup encounterInfoGroup, EncounterXmlRetrievedEventArgs e)
        {
            var encounter = EncounterLoader.ReadEncounter(e.DataXml, e.ImagesXml);
            EncounterData = encounter;
            IsDone = true;
            Completed?.Invoke(encounter);
        }

        public void DoStuff(User user, EncounterInfo info)
        {
            throw new NotImplementedException();
        }
    }
}