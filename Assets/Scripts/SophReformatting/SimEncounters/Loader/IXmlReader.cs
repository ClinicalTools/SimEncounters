using System.Xml;

namespace ClinicalTools.SimEncounters.Loading
{
    public interface IXmlReader
    {
        XmlDocument ReadXml(string path);
    }
}