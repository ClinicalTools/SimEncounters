using System;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public interface IDownloadEncounter
    {
        event Action<XmlDocument> Completed;

        void GetXml(User user, EncounterInfo encounterInfo, XmlType type);
    }
}