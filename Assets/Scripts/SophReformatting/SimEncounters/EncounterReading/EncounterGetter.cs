using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;


namespace ClinicalTools.SimEncounters.EncounterReader
{
    public class EncounterGetter
    {

        protected virtual EncounterXml EncounterXml { get; } = new EncounterXml();
        protected virtual Task<SectionsData> SectionsData { get; set; }
        protected virtual Task<ImagesData> ImagesData { get; set; }
        public virtual Task<Encounter> Encounter { get; set; }

        protected virtual NodeInfo ContentInfo { get; } = new NodeInfo("content");
        protected virtual NodeInfo ImagesInfo { get; } = new NodeInfo("images");

        EncounterDataFactory EncounterDataFactory => new EncounterDataFactory();
        ImageDataFactory ImageDataFactory => new ImageDataFactory();


        protected virtual void StartEncounterTasks()
        {
            SectionsData = Task.Run(GetSectionsData);
            ImagesData = Task.Run(GetImagesData);
            Encounter = Task.Run(GetEncounter);
        }

        protected async virtual Task<SectionsData> GetSectionsData()
        {
            while (EncounterXml.CurrentEncounterCed == null)
                Thread.Sleep(1);

            var sectionsXml = EncounterXml.CurrentEncounterCed;
            await sectionsXml;

            var deserializer = new XmlDeserializer(sectionsXml.Result);
            return DeserializeSectionsData(deserializer);
        }
        protected virtual SectionsData DeserializeSectionsData(XmlDeserializer deserializer) => deserializer.GetValue(ContentInfo, EncounterDataFactory);

        protected async virtual Task<ImagesData> GetImagesData()
        {
            while (EncounterXml.CurrentEncounterCei == null)
                Thread.Sleep(1);

            var imagesXml = EncounterXml.CurrentEncounterCei;
            await imagesXml;

            var deserializer = new XmlDeserializer(imagesXml.Result);
            return DeserializeImagesData(deserializer);
        }
        protected virtual ImagesData DeserializeImagesData(XmlDeserializer deserializer) => deserializer.GetValue(ImagesInfo, ImageDataFactory);

        protected async virtual Task<Encounter> GetEncounter()
        {
            await SectionsData;
            await ImagesData;

            return new Encounter(SectionsData.Result, ImagesData.Result);
        }
    }
}