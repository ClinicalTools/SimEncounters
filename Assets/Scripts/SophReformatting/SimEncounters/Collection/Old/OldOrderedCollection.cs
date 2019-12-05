using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    /// <summary>
    /// Stores an ordered collection of unique values by randomly generated key strings.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the values in the collection. Type cannot be a string or an int.
    /// </typeparam>
    public abstract class OldOrderedCollection<T> : SimCollection<T>, IEnumerable<KeyValuePair<string, T>>
    {
        // An ordered list of values
        protected virtual List<T> ValueList { get; set; } = new List<T>();
        // An ordered list of the key value pairs
        // This could be made to use less memory, but both parts may be used frequently, so this is simpler
        protected virtual List<KeyValuePair<string, T>> PairList { get; set; } = new List<KeyValuePair<string, T>>();

        /// <summary>
        /// Ordered array of values in the collection.
        /// </summary>
        public virtual T[] ValueArr => ValueList.ToArray();
        public override IEnumerable<T> Values => ValueList;

        public virtual KeyValuePair<string, T> this[int index] => Get(index);

        public OldOrderedCollection() : base() { }
        public OldOrderedCollection(XmlNode encounterNode) : base(encounterNode) { }

        protected override void Add(string key, T value)
        {
            var pair = new KeyValuePair<string, T>(key, value);
            Add(pair);
        }
        protected override void Add(KeyValuePair<string, T> pair)
        {
            base.Add(pair);
            PairList.Add(pair);
            ValueList.Add(pair.Value);
        }

        public override void Remove(string key)
        {
            var index = ValueList.IndexOf(Collection[key]);
            ValueList.RemoveAt(index);
            PairList.RemoveAt(index);
            
            base.Remove(key);
        }

        public virtual KeyValuePair<string, T> Get(int val) => PairList[val];

        public virtual void MoveValue(int newIndex, int currentIndex)
        {
            ValueList.Insert(newIndex, ValueList[currentIndex]);
            if (newIndex < currentIndex)
                currentIndex++;

            ValueList.RemoveAt(currentIndex);
        }
        public virtual void MoveValue(int newIndex, T value)
        {
            ValueList.Remove(value);
            ValueList.Insert(newIndex, value);
        }

        IEnumerator IEnumerable.GetEnumerator() => Collection.GetEnumerator();
        public override IEnumerator<KeyValuePair<string, T>> GetEnumerator() => PairList.GetEnumerator();
    }
}
