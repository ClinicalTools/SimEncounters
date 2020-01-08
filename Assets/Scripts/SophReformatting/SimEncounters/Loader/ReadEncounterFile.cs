using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Loading
{
    public class ReadEncounterXml
    {
        protected virtual string EncryptionKey { get; }
        protected virtual string EncryptionIV { get; }

        public virtual async Task<XmlDocument> ReadXml(string path)
        {
            var xmlDoc = new XmlDocument();
            string text = null;
            using (StreamReader read = new StreamReader(path)) {
                text = await read.ReadToEndAsync();
            }

            if (text == null || text.Equals("")) {
                Debug.Log("Nothing to load!");
                //Maybe ask if they want to check the server here?
                //return;
                return null;
            }

            try {
                xmlDoc.LoadXml(text); //this loads the local file
                                      // Decrypt the data
            } catch (XmlException) {
                text = await DecryptXml(path);
                xmlDoc.LoadXml(text);
            }

            return xmlDoc;
        }

        protected virtual async Task<string> DecryptXml(string path)
        {
            AesManaged aes = new AesManaged();
            ICryptoTransform decrypt = aes.CreateDecryptor(Encoding.UTF8.GetBytes(EncryptionKey), Encoding.UTF8.GetBytes(EncryptionIV));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (CryptoStream cs = new CryptoStream(fs, decrypt, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs)) {
                return await sr.ReadToEndAsync();
            }
        }
    }
}