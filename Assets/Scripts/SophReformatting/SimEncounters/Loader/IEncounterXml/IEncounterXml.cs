using System;
using System.Threading.Tasks;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterXml
    {
        event Action<XmlDocument, XmlDocument> Completed; 

        Task<XmlDocument> DataXml { get; }
        Task<XmlDocument> ImagesXml { get; }

        void GetEncounterXml();
    }
}