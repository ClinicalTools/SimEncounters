using ClinicalTools.SimEncounters.Data;
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

        void GetEncounterXml(User user, EncounterInfo encounterInfo);
    }
}