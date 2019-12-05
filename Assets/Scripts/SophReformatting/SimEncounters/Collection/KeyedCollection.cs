using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    /// <summary>
    /// A bidirectional dictionary with automatically generated string keys.
    /// </summary>
    /// <typeparam name="T">Type of values stored in the collection</typeparam>
    public class KeyedCollection<T> : IEnumerable<KeyValuePair<string, T>>
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

        public KeyedCollection() { }
        public KeyedCollection(IEnumerable<KeyValuePair<string, T>> keyValuePairs)
        {
            foreach (var pair in keyValuePairs)
                Add(pair);
        }

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
                Debug.LogError($"Value already exists in collection:\n" +
                    $"{value.ToString()}");
                return null;
            }

            var key = GenerateKey();

            Add(key, value);

            return key;
        }

        public virtual void Add(KeyValuePair<string, T> pair)
        {
            Collection.Add(pair);
            KeyCollection.Add(pair.Value, pair.Key);
        }

        public virtual void Add(string key, T value)
        {
            if (Collection.ContainsKey(key)) {
                Debug.LogError($"Key already exists in collection:\n" +
                    $"{value.ToString()}");
                return;
            }
            if (KeyCollection.ContainsKey(value)) {
                Debug.LogError($"Value already exists in collection:\n" +
                    $"{value.ToString()}");
                return;
            }

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
        public virtual void Remove(string key)
        {
            var item = Collection[key];
            KeyCollection.Remove(item);
            Collection.Remove(key);
        }

        public virtual bool ContainsKey(string key) => Collection.ContainsKey(key);
        public virtual bool Contains(T value) => KeyCollection.ContainsKey(value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public virtual IEnumerator<KeyValuePair<string, T>> GetEnumerator() => Collection.GetEnumerator();
    }
}