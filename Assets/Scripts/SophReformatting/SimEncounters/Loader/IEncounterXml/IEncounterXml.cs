using ClinicalTools.SimEncounters.Data;
using System;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterXml
    {
        event Action<XmlDocument, XmlDocument> Completed;

        void GetEncounterXml(User user, EncounterInfoGroup encounterInfoGroup);
    }
}