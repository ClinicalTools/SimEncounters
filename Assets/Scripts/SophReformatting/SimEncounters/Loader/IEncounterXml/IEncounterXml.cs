using System.Threading.Tasks;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterXml
    {
        Task<XmlDocument> DataXml { get; }
        Task<XmlDocument> ImagesXml { get; }
    }
}