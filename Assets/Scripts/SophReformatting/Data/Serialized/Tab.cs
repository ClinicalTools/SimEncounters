using SimEncounters.Xml;
using System.Collections.Generic;
using System.Xml;

namespace SimEncounters
{
    public class Tab : IXmlSerializable
    {
        public virtual string Name { get; set; } //Display name, not formatted
        public virtual string Type { get; }
        public virtual XmlNode Data { get; set; }
        public virtual bool Persistent { get; } //If true, cannot change
        public virtual List<string> Conditions { get; set; }


        public Tab(string type)
        {
            Type = type;
            Name = type;
        }

        public Tab(XmlDeserializer deserializer)
        {
            Type = deserializer.GetString("type");
            Name = deserializer.GetString("name");
            Conditions = deserializer.GetStringList("conditions", "condition");
        }

        public void GetObjectData(XmlSerializer serializer)
        {
            serializer.AddString("type", Type);
            serializer.AddString("name", Name);
            serializer.AddStringList("conditions", "condition", Conditions);
        }

        /**
         * Data script to hold info for every tab. Stored in SectionDataScript.Dict
         */
        public Tab(string type, string name, bool persistent, XmlNode data = null, List<string> conditions = null)
        {
            Type = type;
            Name = name;
            Data = data;
            Persistent = persistent;

            Conditions = conditions;
        }

    }
}