using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.Loading;
using System;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public delegate void EncounterXmlRetrievedEventHandler(object sender, EncounterXmlRetrievedEventArgs e);
    public class EncounterXmlRetrievedEventArgs
    {
        public XmlDocument DataXml { get; }
        public XmlDocument ImagesXml { get; }

        public EncounterXmlRetrievedEventArgs(XmlDocument dataXml, XmlDocument imagesXml)
        {
            DataXml = dataXml;
            ImagesXml = imagesXml;
        }
    }
    public interface IEncounterXml
    {
        event EncounterXmlRetrievedEventHandler Completed;

        void GetEncounterXml(User user, EncounterInfoGroup encounterInfoGroup);
    }

    public class EncounterGetter : IEncounterGetter
    {
        public event Action<Encounter> Completed;
        public bool IsDone { get; protected set; }
        public Encounter Encounter { get; protected set; }

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

        public void GetAutosaveEncounter(User user, EncounterInfoGroup encounterInfoGroup)
        {
            encounterInfoGroup.CurrentInfo = encounterInfoGroup.AutosaveInfo;
            GetEncounter(user, encounterInfoGroup, AutoSaveXml);
        }

        public void GetLocalEncounter(User user, EncounterInfoGroup encounterInfoGroup)
        {
            encounterInfoGroup.CurrentInfo = encounterInfoGroup.LocalInfo;
            GetEncounter(user, encounterInfoGroup, FileXml);
        }

        public void GetServerEncounter(User user, EncounterInfoGroup encounterInfoGroup)
        {
            encounterInfoGroup.CurrentInfo = encounterInfoGroup.ServerInfo;
            GetEncounter(user, encounterInfoGroup, ServerXml);
        }

        protected void GetEncounter(User user, EncounterInfoGroup encounterInfoGroup, IEncounterXml encounterXml)
        {
            encounterXml.Completed += (sender, e) => EncounterXml_Completed(encounterInfoGroup, e);
            encounterXml.GetEncounterXml(user, encounterInfoGroup);
        }

        private void EncounterXml_Completed(EncounterInfoGroup encounterInfoGroup, EncounterXmlRetrievedEventArgs e)
        {
            var encounter = EncounterLoader.ReadEncounter(encounterInfoGroup, e.DataXml, e.ImagesXml);
            Encounter = encounter;
            IsDone = true;
            Completed?.Invoke(encounter);
        }
    }
}