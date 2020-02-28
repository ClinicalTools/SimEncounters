using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Loading
{
    public class FileXmlReader : IXmlReader
    {
        protected virtual string EncryptionKey { get; } = "obexOpm1wWM7NGPV";
        protected virtual string EncryptionIV { get; } = "fTfB28G5j3Pmsw1p";

        public virtual XmlDocument ReadXml(string path)
        {
            var text = File.ReadAllText(path);

            if (text == null || text.Equals("")) {
                Debug.Log("Nothing to load!");
                return null;
            }

            var xmlDoc = new XmlDocument();
            try {
                xmlDoc.LoadXml(text);
            } catch (XmlException) {
                text = DecryptXml(path);
                xmlDoc.LoadXml(text);
            }

            return xmlDoc;
        }

        protected virtual string DecryptXml(string path)
        {
            AesManaged aes = new AesManaged();
            ICryptoTransform decrypt = aes.CreateDecryptor(Encoding.UTF8.GetBytes(EncryptionKey), Encoding.UTF8.GetBytes(EncryptionIV));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (CryptoStream cs = new CryptoStream(fs, decrypt, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs)) {
                return sr.ReadToEnd();
            }
        }
    }
}