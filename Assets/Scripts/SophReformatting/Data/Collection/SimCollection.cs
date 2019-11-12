using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace SimEncounters
{
    /// <summary>
    /// Stores a collection of unique values by randomly generated key strings.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the values in the collection. Type cannot be a string.
    /// </typeparam>
    /// <remarks>
    /// As old data can be in an incorrect format, GetLegacyKey and ValidLegacyNodeName 
    /// exist solely to be overridden when necessary.
    /// </remarks>
    public abstract class SimCollection<T> : IEnumerable<KeyValuePair<string, T>>
    {
        /// <summary>
        /// Dictionary of values in the collection.
        /// This dictionary is bidirectional through the use of the Keys dictionary, 
        /// so all values must be unique.
        /// </summary>
        protected virtual IDictionary<string, T> Collection { get; } = new Dictionary<string, T>();

        /// <summary>
        /// Dictionary of keys in the collection.
        /// </summary>
        protected virtual IDictionary<T, string> KeyCollection { get; } = new Dictionary<T, string>();

        public virtual T this[string key] => Get(key);
        public virtual string this[T value] => GetKey(value);

        public virtual IEnumerable<string> Keys => Collection.Keys.AsEnumerable();
        public virtual IEnumerable<T> Values => Collection.Values.AsEnumerable();
        public int Count => Collection.Count;


        /// <summary>
        /// Name of the XML tag used to serialize the values.
        /// </summary>
        public abstract string CollectionNodeName { get; }
        /// <summary>
        /// Name of the XML tag used to serialize a value.
        /// </summary>
        protected abstract string ValueNodeName { get; }

        public SimCollection() { }
        public SimCollection(XmlNode encounterNode)
        {
            XmlNode collectionNode = encounterNode[CollectionNodeName];
            if (collectionNode == null) {
                collectionNode = GetLegacyCollectionNode(encounterNode);

                if (collectionNode == null)
                    return;
            }

            foreach (XmlNode valNode in collectionNode)
                AddValueFromNode(valNode);
        }

        /// <summary>
        /// Adds a value to the collection from an XmlNode.
        /// </summary>
        /// <param name="valNode">Node representing the value to add</param>
        protected virtual void AddValueFromNode(XmlNode valNode)
        {
            if (valNode.Name != ValueNodeName && !ValidLegacyNodeName(valNode.Name))
                return;

            var key = GetKey(valNode);
            if (key == null)
                return;

            if (Collection.ContainsKey(key)) {
                Debug.LogError($"Key already exists in collection({CollectionNodeName}):" +
                    $"\n{key}\n{valNode.OuterXml}");
                return;
            }

            T val = ReadValueNode(valNode);

            if (val != null)
                Add(key, val);
        }

        /// <summary>
        /// Gets the collection node for legacy versions of the data.
        /// </summary>
        /// <param name="encounterNode">Base node for the entire encounter</param>
        /// <returns>An XmlNode representing the collection, or null if not found.</returns>
        protected virtual XmlNode GetLegacyCollectionNode(XmlNode encounterNode) => null;

        /// <summary>
        /// Reads the key from a value node.
        /// </summary>
        /// <param name="valueNode">Value node with the key to read</param>
        /// <returns>A string representation of the key, or null if not found.</returns>
        protected virtual string GetKey(XmlNode valueNode)
        {
            var key = valueNode.Attributes["key"]?.InnerText;
            if (key == null)
                key = GetLegacyKey(valueNode);
            if (key == null) {
                Debug.LogError($"Could not read key ({CollectionNodeName}):\n{valueNode.OuterXml}");
                return null;
            }

            key = key.Replace(GlobalData.EMPTY_WIDTH_SPACE + "", "");
            return key;
        }


        /// <summary>
        /// Reads the node for legacy versions of the data.
        /// </summary>
        /// <param name="valueNode">Value node</param>
        /// <returns>A string representation of the key, or null if not found.</returns>
        /// <remarks>This function only exists to be overwritten when relevant.</remarks>
        protected virtual string GetLegacyKey(XmlNode valueNode) => null;

        /// <summary>
        /// If old data saved node names differently, check if the node name is in that format.
        /// </summary>
        /// <param name="valueNodeName">String representing the name of a value node</param>
        /// <returns>True if the node name is valid for a value node in a legacy format</returns>
        /// <remarks>This function only exists to be overwritten when relevant.</remarks>
        protected virtual bool ValidLegacyNodeName(string valueNodeName) => false;


        /// <summary>
        /// Creates a unique key for the collection item using Guid.NewGuid()
        /// </summary>
        /// <returns>The unique key</returns>
        protected virtual string GenerateKey()
        {
            var key = Guid.NewGuid().ToString("N").Substring(0, 10);
            //If duplicate, recalculate UID
            if (Collection.Keys.Contains(key))
                return GenerateKey();


            return key;
        }

        /// <summary>
        /// Adds a value to the collection.
        /// The value must be unique.
        /// </summary>
        /// <param name="value">Value to add</param>
        /// <returns>The key of the added value, or null if unable to add the value</returns>
        public virtual string Add(T value)
        {
            if (KeyCollection.ContainsKey(value)) {
                Debug.LogError($"Value already exists in collection ({CollectionNodeName}):\n" +
                    $"{value.ToString()}");
                return null;
            }

            var key = GenerateKey();

            Add(key, value);
            
            return key;
        }

        protected virtual void Add(string key, T value)
        {
            Collection.Add(key, value);
            KeyCollection.Add(value, key);
        }

        public virtual string GetKey(T value) => KeyCollection[value];

        /// <summary>
        /// Gets a value from the collection by its key.
        /// </summary>
        /// <param name="key">Key of the value to get</param>
        /// <returns>
        /// The value for the passed key, or null if the key isn't in the collection
        /// </returns>
        public virtual T Get(string key)
        {
            if (Collection.ContainsKey(key))
                return Collection[key];
            else
                return default;
        }

        /// <summary>
        /// Removes a value from the collection by its key.
        /// </summary>
        /// <param name="key">Key of the value to remove</param>
        public virtual void Remove(string key) {
            var item = Collection[key];
            KeyCollection.Remove(item);
            Collection.Remove(key);
        }

        public virtual bool ContainsKey(string key) => Collection.ContainsKey(key);

        /// <summary>
        /// Reads a node 
        /// </summary>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        protected abstract T ReadValueNode(XmlNode valueNode);

        /// <summary>
        /// Creates an XmlElement representing the entire collection.
        /// </summary>
        /// <returns>An XmlElement representing the entire collection</returns>
        /// <remarks>
        /// Element values will be seen as "InnerText" and not "Value,"
        /// so this data should be used exclusively for storing the data.
        /// </remarks>
        public virtual XmlElement GetXml()
        {
            var elem = XmlHelper.CreateElement(CollectionNodeName);
            foreach (var keyValPair in Collection) {
                var valElem = XmlHelper.CreateElement(ValueNodeName, elem);
                valElem.SetAttribute("key", keyValPair.Key);
                WriteValueElem(valElem, keyValPair.Value);
            }

            return elem;
        }

        /// <summary>
        /// Modifies an XmlElement to represent the passed value.
        /// </summary>
        /// <param name="valueElement"></param>
        /// <param name="value">Value to create an XmlElement represented</param>
        /// <returns>The modified XmlElement</returns>
        protected abstract void WriteValueElem(XmlElement valueElement, T value);


        IEnumerator IEnumerable.GetEnumerator() => Collection.GetEnumerator();
        public virtual IEnumerator<KeyValuePair<string, T>> GetEnumerator() => Collection.GetEnumerator();
    }
}