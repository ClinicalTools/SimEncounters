using ClinicalTools.SimEncounters.Data;
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

    public class EncounterGetter
    {
        protected ServerXml ServerXml { get; set; }
        protected AutoSaveXml AutoSaveXml { get; set; }
        protected FileXml FileXml { get; set; }

        public IEncounterXml EncounterXml { get; set; }

        public EncounterGetter(ServerXml serverXml, AutoSaveXml autoSaveXml, FileXml fileXml)
        {
            ServerXml = serverXml;
            FileXml = fileXml;
            AutoSaveXml = autoSaveXml;
        }

        public void GetAutosaveEncounter(EncounterInfoGroup encounterInfoGroup)
        {
            ServerXml.Completed += ServerXml_Completed;

        }

        private void ServerXml_Completed(object sender, EncounterXmlRetrievedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void GetLocalEncounter(EncounterInfoGroup encounterInfoGroup)
        {

        }

        public void GetServerEncounter(EncounterInfoGroup encounterInfoGroup)
        {

        }
    }
}